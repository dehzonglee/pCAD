using System;
using System.Collections.Generic;
using Model;
using UI;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private SketchStyle _sketchStyle;

    [Serializable]
    public struct UI
    {
        public CoordinateSystemUI coordinateSystemUI;
        public RectanglesUI rectanglesUI;
    }

    [Serializable]
    private struct Model
    {
        public bool IsDragging => draggedCoordinate != null;
        public CoordinateSystem coordinateSystem;
        public (Coordinate x, Coordinate y, Coordinate z)? nextPosition;
        public Coordinate draggedCoordinate;
        public RectangleModel nextRectangle;
        public List<RectangleModel> rectangles;
    }

    public class RectangleModel
    {
        public (Coordinate x, Coordinate y, Coordinate z)? P0;
        public (Coordinate x, Coordinate y, Coordinate z)? P1;
        public bool IsBaked;
    }

    private void Start()
    {
        _model.coordinateSystem = new CoordinateSystem();
        _model.rectangles = new List<RectangleModel>();
//        _model.coordinateSystem.CoordinateSystemChangedEvent += UpdateUI;
        _ui.coordinateSystemUI.Initialize();
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.ManipulateCoordinates:
                // delete next position preview
                if (_model.nextPosition.HasValue)
                {
                    var p = _model.nextPosition.Value;
                    if (p.x.IsPreview) p.x.Delete();
                    if (p.y.IsPreview) p.y.Delete();
                    if (p.z.IsPreview) p.z.Delete();
                    _model.nextPosition = null;
                }

                // delete next rectangle
                if (_model.nextRectangle != null)
                {
                    _model.nextRectangle.P0.Value.x.UnregisterGeometryAndTryToDelete(_model.nextRectangle);
                    _model.nextRectangle.P0.Value.y.UnregisterGeometryAndTryToDelete(_model.nextRectangle);
                    _model.nextRectangle.P0.Value.z.UnregisterGeometryAndTryToDelete(_model.nextRectangle);

                    _model.nextRectangle = null;
                }

                break;
            case State.DrawRectangle:
                break;
        }

        _state = newState;
    }

    private void Update()
    {
        if (_model.draggedCoordinate != null)
        {
            Debug.Log($"dragging: {_model.draggedCoordinate.Parameter}");
        }

        // switch input state
        if (Input.GetKeyDown(ManipulateCoordinatesStateKey))
            SetState(State.ManipulateCoordinates);

        if (Input.GetKeyDown(DrawRectanglesStateKey))
            SetState(State.DrawRectangle);

        switch (_state)
        {
            case State.ManipulateCoordinates:
                if (Input.GetKeyDown(DrawKey))
                    _model.draggedCoordinate = CoordinateManipulation.TryStartDrag(_ui.coordinateSystemUI);
                if (Input.GetKey(DrawKey) && _model.draggedCoordinate != null)
                    _model.draggedCoordinate.Parameter =
                        CoordinateManipulation.UpdateDrag(_model.draggedCoordinate,
                            _model.coordinateSystem.AxisThatContainsCoordinate(_model.draggedCoordinate));
                if (Input.GetKeyUp(DrawKey) && _model.IsDragging)
                    _model.draggedCoordinate = null;
                break;

            case State.DrawRectangle:
                // update next position
                if (_model.nextPosition.HasValue)
                {
                    var p = _model.nextPosition.Value;
                    if (p.x.IsPreview) p.x.Delete();
                    if (p.y.IsPreview) p.y.Delete();
                    if (p.z.IsPreview) p.z.Delete();
                }

                _model.nextPosition = GetOrCreatePositionAtMousePosition(_model.coordinateSystem, true);

                // delete
                if (Input.GetKeyDown(DeleteKey))
                {
                    var p = GetOrCreatePositionAtMousePosition(_model.coordinateSystem);
                    p.x.Delete();
                    p.y.Delete();
                    p.z.Delete();
                }

                // set anchor
                if (Input.GetKeyDown(SetAnchorKey))
                {
                    var mousePosition = MouseInput.RaycastPosition;
                    _model.coordinateSystem.SetAnchorPosition(mousePosition);
                }

                // draw
                if (Input.GetKeyDown(DrawKey))
                {
                    _model.nextPosition.Value.x.Bake();
                    _model.nextPosition.Value.y.Bake();
                    _model.nextPosition.Value.z.Bake();

                    _model.coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

                    if (_model.nextRectangle == null)
                    {
                        //start new rectangle
                        _model.nextRectangle = new RectangleModel();
                        _model.rectangles.Add(_model.nextRectangle);

                        var p = _model.nextPosition.Value;
                        _model.nextRectangle.P0 = p;

                        p.x.AddAttachedGeometry(_model.nextRectangle);
                        p.y.AddAttachedGeometry(_model.nextRectangle);
                        p.z.AddAttachedGeometry(_model.nextRectangle);
                    }
                    else
                    {
                        //complete rectangle
                        _model.nextRectangle.P1 = _model.nextPosition.Value;
                        _model.nextRectangle.IsBaked = true;
                        _model.nextRectangle = null;
                    }
                }

                //update rectangle while drawing
                if (_model.nextRectangle != null)
                    _model.nextRectangle.P1 = _model.nextPosition.Value;

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        _ui.coordinateSystemUI.UpdateUI(_model.coordinateSystem, _sketchStyle.CoordinateUIStyle);
        _ui.rectanglesUI.UpdateUI(_model.rectangles, _sketchStyle.GeometryStyle.Rectangle);
    }

    private static (Coordinate x, Coordinate y, Coordinate z) GetOrCreatePositionAtMousePosition(
        CoordinateSystem coordinateSystem, bool asPreview = false)
    {
        var position = coordinateSystem.GetParametricPosition(MouseInput.RaycastPosition, asPreview);
        return position;
    }

    private enum State
    {
        ManipulateCoordinates,
        DrawRectangle,
    }

    private Model _model;

    private State _state = State.ManipulateCoordinates;

    private const KeyCode ManipulateCoordinatesStateKey = KeyCode.Alpha1;
    private const KeyCode DrawRectanglesStateKey = KeyCode.Alpha2;
    private const KeyCode DrawKey = KeyCode.Mouse0;
    private const KeyCode SetAnchorKey = KeyCode.Mouse1;
    private const KeyCode DeleteKey = KeyCode.Mouse2;
}
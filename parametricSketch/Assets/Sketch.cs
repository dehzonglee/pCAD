using System;
using System.Collections.Generic;
using Model;
using UI;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField] private UI _ui;
    private Model _model;

    [Serializable]
    public struct UI
    {
        public CoordinateSystemUI CoordinateSystem;
        public RectanglesUI Rectangles;
    }

    [Serializable]
    private struct Model
    {
        public bool IsDragging => CapturedDrag != null;
        public CoordinateSystem CoordinateSystem;
        public (Coordinate x, Coordinate y, Coordinate z)? NextPosition;
        public (Axis DraggedAxis, Coordinate DraggedCoordinate)? CapturedDrag;
        public RectangleModel NextRectangle;
        public List<RectangleModel> Rectangles;
    }

    public class RectangleModel
    {
        public (Coordinate x, Coordinate y, Coordinate z)? P0;
        public (Coordinate x, Coordinate y, Coordinate z)? P1;
    }

    private void Start()
    {
        _model.CoordinateSystem = new CoordinateSystem();
        _model.Rectangles = new List<RectangleModel>();
        _ui.CoordinateSystem.Initialize(_model.CoordinateSystem, ModelChangeRequestHandler);
    }

    private void Update()
    {
        // switch input state
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // delete if only preview
            if (_model.NextPosition.HasValue)
            {
                var p = _model.NextPosition.Value;
                if (p.x.IsPreview) p.x.Delete();
                if (p.y.IsPreview) p.y.Delete();
                if (p.z.IsPreview) p.z.Delete();
            }

            _model.NextPosition = null;

            if (_model.NextRectangle != null)
            {
                _model.NextRectangle.P0.Value.x.UnregisterGeometryAndTryToDelete(_model.NextRectangle);
                _model.NextRectangle.P0.Value.y.UnregisterGeometryAndTryToDelete(_model.NextRectangle);
                _model.NextRectangle.P0.Value.z.UnregisterGeometryAndTryToDelete(_model.NextRectangle);
                _model.NextRectangle = null;
            }

            _state = State.ManipulateCoordinates;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _state = State.DrawRectangle;

        switch (_state)
        {
            case State.ManipulateCoordinates:
                if (Input.GetMouseButtonDown(0))
                    TryStartDrag();

                if (Input.GetMouseButton(0) && _model.IsDragging)
                    UpdateDrag();

                if (Input.GetMouseButtonUp(0) && _model.IsDragging)
                    CompleteDrag();

                break;

            case State.DrawRectangle:

                //update preview
                // delete if only preview
                if (_model.NextPosition.HasValue)
                {
                    var p = _model.NextPosition.Value;
                    if (p.x.IsPreview) p.x.Delete();
                    if (p.y.IsPreview) p.y.Delete();
                    if (p.z.IsPreview) p.z.Delete();
                }

                _model.NextPosition = GetOrCreatePositionAtMousePosition(true);

                // delete
                if (Input.GetMouseButtonDown(2))
                {
                    var p = GetOrCreatePositionAtMousePosition();
                    p.x.Delete();
                    p.y.Delete();
                    p.z.Delete();
                }

                // set anchor
                if (Input.GetMouseButtonDown(1))
                {
                    var mousePosition = MouseInput.RaycastPosition;
                    _model.CoordinateSystem.SetAnchorPosition(mousePosition);
                }

                // draw
                if (Input.GetMouseButtonDown(0))
                {
                    _model.NextPosition.Value.x.Bake();
                    _model.NextPosition.Value.y.Bake();
                    _model.NextPosition.Value.z.Bake();

                    _model.CoordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

                    if (_model.NextRectangle == null)
                    {
                        //start new rectangle
                        _model.NextRectangle = new RectangleModel();
                        _model.Rectangles.Add(_model.NextRectangle);

                        var p = _model.NextPosition.Value;
                        _model.NextRectangle.P0 = (p.x, p.y, p.z);

                        p.x.AddAttachedGeometry(_model.NextRectangle);
                        p.y.AddAttachedGeometry(_model.NextRectangle);
                        p.z.AddAttachedGeometry(_model.NextRectangle);
                    }
                    else
                    {
                        //complete rectangle
                        var p = _model.NextPosition.Value;
                        _model.NextRectangle.P1 = _model.NextPosition.Value;
                        _model.NextRectangle = null;
                    }
                }

                //update rectangle
                if (_model.NextRectangle != null)
                {
                    _model.NextRectangle.P1 = _model.NextPosition.Value;
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        _ui.CoordinateSystem.UpdateUI();
        _ui.Rectangles.UpdateUI(_model.Rectangles);
    }

    private (Coordinate x, Coordinate y, Coordinate z) GetOrCreatePositionAtMousePosition(bool asPreview = false)
    {
        var mousePosition = MouseInput.RaycastPosition;
        var position = _model.CoordinateSystem.GetParametricPosition(mousePosition, asPreview);
//        _model.CoordinateSystem.SetAnchorPosition(new Vector3(position.x.Value, position.y.Value, position.z.Value));

        return position;
    }

    private void TryStartDrag()
    {
        _model.CapturedDrag = MouseInput.RaycastCoordinateUI;
    }

    private void UpdateDrag()
    {
        var coordinate = _model.CapturedDrag.Value.DraggedCoordinate;
        var axis = _model.CapturedDrag.Value.DraggedAxis;
        ModelChangeRequestHandler(_model.CapturedDrag.Value.DraggedAxis, _model.CapturedDrag.Value.DraggedCoordinate,
            MousePositionToParameter(MouseInput.RaycastPosition, coordinate, axis));
    }

    private float MousePositionToParameter(Vector3 mouseWorldPosition, Coordinate coordinate, Axis axis)
    {
        return Vector3.Dot(mouseWorldPosition, axis.Direction) - coordinate.ParentValue;
    }

    private void CompleteDrag()
    {
        _model.CapturedDrag = null;
    }

    private void ModelChangeRequestHandler(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;
    }

    private enum State
    {
        ManipulateCoordinates,
        DrawRectangle,
    }

    private State _state = State.ManipulateCoordinates;
}
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
        public CoordinateSystem coordinateSystem;
        public Vec<Coordinate> focusPosition;
        public Coordinate draggedCoordinate;
        public RectangleModel nextRectangle;
        public List<RectangleModel> rectangles;
        public KeyboardInput.Model keyboardInputModel;
    }

    public class RectangleModel
    {
        public Vec<Coordinate> P0;
        public Vec<Coordinate> P1;
        public bool IsBaked;
    }

    private void Initialize(Vec<float> mousePositionAsOrigin)
    {
        _model.coordinateSystem = new CoordinateSystem(mousePositionAsOrigin);
        _model.rectangles = new List<RectangleModel>();
        _model.keyboardInputModel = new KeyboardInput.Model();
//        _model.coordinateSystem.CoordinateSystemChangedEvent += UpdateUI;
        _ui.coordinateSystemUI.Initialize();

        //start drawing first rectangle
        _state = State.DrawRectangle;
        _model.focusPosition = CoordinateCreation.UpdateCursorPosition(
            _model.focusPosition,
            _model.coordinateSystem,
            _model.keyboardInputModel
        );
        StartDrawing();
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.ManipulateCoordinates:
                // delete next position preview
                if (_model.focusPosition != null)
                {
                    _model.focusPosition.ForEach(c =>
                    {
                        if (c.IsPreview) c.Delete();
                    });
                    _model.focusPosition = null;
                }

                // delete next rectangle
                if (_model.nextRectangle != null)
                {
                    RectangleCreation.AbortRectangle(_model.nextRectangle);
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
        var hasBeenInitialized = _model.coordinateSystem != null;
        if (!hasBeenInitialized)
        {
            if (Input.GetKeyDown(PrimaryMouse))
                Initialize(MouseInput.RaycastPosition);

            return;
        }

        if (_model.draggedCoordinate != null)
        {
//            Debug.Log($"dragging: {_model.draggedCoordinate.Parameter}");
        }

        // switch input state
        if (Input.GetKeyDown(ManipulateCoordinatesStateKey))
            SetState(State.ManipulateCoordinates);

        if (Input.GetKeyDown(DrawRectanglesStateKey))
            SetState(State.DrawRectangle);

        switch (_state)
        {
            case State.ManipulateCoordinates:

                // start drag
                if (Input.GetKeyDown(PrimaryMouse))
                    _model.draggedCoordinate = CoordinateManipulation.TryStartDrag(_ui.coordinateSystemUI);

                // update drag
                if (Input.GetKey(PrimaryMouse) && _model.draggedCoordinate != null)
                {
                    var (value, pointsInNegativeDirection) = CoordinateManipulation.NewUpdateDrag(
                        _model.draggedCoordinate,
                        _model.coordinateSystem.AxisThatContainsCoordinate(_model.draggedCoordinate));

                    _model.draggedCoordinate.Parameter.Value = value;
                    //quick fix: for now, only mue coordinates can be dragged
                    ((Mue) _model.draggedCoordinate).PointsInNegativeDirection = pointsInNegativeDirection;
                }

                // stop drag
                if (Input.GetKeyUp(PrimaryMouse) && _model.draggedCoordinate != null)
                    _model.draggedCoordinate = null;

                break;

            case State.DrawRectangle:

                KeyboardInput.UpdateKeyboardInput(ref _model.keyboardInputModel,
                    _model.coordinateSystem.GetAllParameters()
                );

                _model.focusPosition = CoordinateCreation.UpdateCursorPosition(
                    _model.focusPosition,
                    _model.coordinateSystem,
                    _model.keyboardInputModel
                );

                if (_model.focusPosition == null)
                {
                    Debug.LogError($"Focus Position should always be != null if state == DrawRectangles");
                    return;
                }

                // delete
                if (Input.GetKeyDown(DeleteKey))
                {
                    CoordinateCreation.DeletePositionAtMousePosition(_model.coordinateSystem);
                }

                // set anchor
                if (Input.GetKeyDown(SetAnchorKey))
                {
                    var mousePosition = MouseInput.RaycastPosition;
                    _model.coordinateSystem.SetAnchorPosition(mousePosition);
                }

                // draw
                if (Input.GetKeyDown(PrimaryMouse) || Input.GetKeyDown(KeyCode.Return))
                {
                    StartDrawing();
                }

                //update rectangle while drawing
                if (_model.nextRectangle != null)
                    RectangleCreation.UpdateRectangle(_model.nextRectangle, _model.focusPosition);

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdateUI();
    }

    private void StartDrawing()
    {
        CoordinateCreation.BakePosition(_model.focusPosition);

        _model.coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

        if (_model.nextRectangle == null)
        {
            _model.nextRectangle = RectangleCreation.StartNewRectangle(_model.focusPosition);
            _model.rectangles.Add(_model.nextRectangle);
        }
        else
        {
            RectangleCreation.CompleteRectangle(_model.nextRectangle, _model.focusPosition);
            _model.nextRectangle = null;
        }

        // reset input
        _model.keyboardInputModel = new KeyboardInput.Model();
    }

    private void UpdateUI()
    {
        _ui.coordinateSystemUI.UpdateUI(
            _model.coordinateSystem,
            _sketchStyle.CoordinateUIStyle,
            _model.keyboardInputModel);
        _ui.rectanglesUI.UpdateUI(_model.rectangles, _sketchStyle.GeometryStyle.Rectangle);
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
    private const KeyCode PrimaryMouse = KeyCode.Mouse0;
    private const KeyCode SetAnchorKey = KeyCode.Mouse1;
    private const KeyCode DeleteKey = KeyCode.Mouse2;
}
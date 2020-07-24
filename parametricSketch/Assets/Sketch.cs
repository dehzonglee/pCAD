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
        public GeometryUIManager geometryUI;
        public ParameterUI paramterUI;
    }

    [Serializable]
    private struct Model
    {
        public CoordinateSystem coordinateSystem;
        public Vec<Coordinate> focusPosition;
        public Coordinate draggedCoordinate;
        public GeometryModel incompleteGeometry;
        public List<GeometryModel> geometries;
        public KeyboardInput.Model keyboardInputModel;
    }

    public class RectangleModel : GeometryModel
    {
        public Vec<Coordinate> P1;
    }

    public class PointModel : GeometryModel
    {
    }

    public class LineModel : GeometryModel
    {
        public Vec<Coordinate> P1;
    }

    public class GeometryModel
    {
        public Vec<Coordinate> P0;
        public bool IsBaked;
    }

    private void Initialize(Vec<float> mousePositionAsOrigin)
    {
        _model.coordinateSystem = new CoordinateSystem(mousePositionAsOrigin);
        _model.geometries = new List<GeometryModel>();
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
        Draw();
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

        // switch input state
        if (Input.GetKeyDown(ManipulateCoordinatesStateKey))
            SetState(State.ManipulateCoordinates);

        if (Input.GetKeyDown(DrawRectanglesStateKey))
            SetState(State.DrawRectangle);

        if (Input.GetKeyDown(SwitchGeometry))
        {
            switch (_currentGeometryType)
            {
                case GeometryType.Point:
                    _currentGeometryType = GeometryType.Line;
                    break;
                case GeometryType.Line:
                    _currentGeometryType = GeometryType.Rectangle;
                    break;
                case GeometryType.Rectangle:
                    _currentGeometryType = GeometryType.Point;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CleanUpIncompleteGeometry();
            Debug.Log($"Set draw mode to {_currentGeometryType}");
        }

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
                    Draw();
                }

                // update geometry while drawing
                switch (_model.incompleteGeometry)
                {
                    case RectangleModel rectangleModel:
                        RectangleCreation.UpdateRectangle(rectangleModel, _model.focusPosition);
                        break;
                    case LineModel lineModel:
                        LineCreation.UpdateLine(lineModel, _model.focusPosition);
                        break;
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdateUI();
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.ManipulateCoordinates:
                CleanUpFocusPosition();
                CleanUpIncompleteGeometry();
                break;
            case State.DrawRectangle:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        _state = newState;
    }

    private void Draw()
    {
        CoordinateCreation.BakePosition(_model.focusPosition);
        _model.coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

        switch (_currentGeometryType)
        {
            case GeometryType.Point:
                _model.geometries.Add(PointCreation.NewPoint(_model.focusPosition));
                break;
            case GeometryType.Line:
                if (!(_model.incompleteGeometry is LineModel))
                {
                    _model.incompleteGeometry = LineCreation.StartNewLine(_model.focusPosition);
                    _model.geometries.Add(_model.incompleteGeometry);
                }
                else
                {
                    LineCreation.CompleteLine(_model.incompleteGeometry as LineModel, _model.focusPosition);
                    _model.incompleteGeometry = null;
                }

                break;

            case GeometryType.Rectangle:
                if (!(_model.incompleteGeometry is RectangleModel))
                {
                    _model.incompleteGeometry = RectangleCreation.StartNewRectangle(_model.focusPosition);
                    _model.geometries.Add(_model.incompleteGeometry);
                }
                else
                {
                    RectangleCreation.CompleteRectangle(_model.incompleteGeometry as RectangleModel,
                        _model.focusPosition);
                    _model.incompleteGeometry = null;
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        // reset input
        _model.keyboardInputModel = new KeyboardInput.Model();
    }

    private void UpdateUI()
    {
        _ui.coordinateSystemUI.UpdateUI(
            _model.coordinateSystem,
            _sketchStyle.CoordinateUIStyle,
            _model.keyboardInputModel,
            _model.draggedCoordinate);

        _ui.geometryUI.UpdateUI(_model.geometries, _sketchStyle._geometryStyleAsset.Set);
        _ui.paramterUI.UpdateUI(_model.coordinateSystem.GetAllParameters());
    }

    private void CleanUpIncompleteGeometry()
    {
        if (_model.incompleteGeometry == null)
            return;

        _model.incompleteGeometry.P0.ForEach(c =>
            c.UnregisterGeometryAndTryToDelete(_model.incompleteGeometry));
        _model.geometries.Remove(_model.incompleteGeometry);
        _model.incompleteGeometry = null;
    }

    private void CleanUpFocusPosition()
    {
        if (_model.focusPosition == null)
            return;

        _model.focusPosition.ForEach(c =>
        {
            if (c.IsCurrentlyDrawn) c.Delete();
        });
        _model.focusPosition = null;
    }

    private enum State
    {
        ManipulateCoordinates,
        DrawRectangle,
    }

    private Model _model;
    private State _state = State.ManipulateCoordinates;
    private GeometryType _currentGeometryType;

    private enum GeometryType
    {
        Point,
        Line,
        Rectangle
    }

    private const KeyCode ManipulateCoordinatesStateKey = KeyCode.Alpha1;
    private const KeyCode DrawRectanglesStateKey = KeyCode.Alpha2;
    private const KeyCode PrimaryMouse = KeyCode.Mouse0;
    private const KeyCode SetAnchorKey = KeyCode.Mouse1;
    private const KeyCode DeleteKey = KeyCode.Mouse2;
    private const KeyCode SwitchGeometry = KeyCode.Alpha3;
}
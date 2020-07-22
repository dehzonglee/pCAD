using System;
using System.Collections.Generic;
using Model;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Sketch : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private SketchStyle _sketchStyle;

    [Serializable]
    public struct UI
    {
        public CoordinateSystemUI coordinateSystemUI;
        public RectanglesUI rectanglesUI;
        public PointsUI pointsUI;
        [FormerlySerializedAs("LinesUI")] public LinesUI linesUI;
    }

    [Serializable]
    private struct Model
    {
        public CoordinateSystem coordinateSystem;
        public Vec<Coordinate> focusPosition;
        public Coordinate draggedCoordinate;
        public RectangleModel nextRectangle;
        public LineModel nextLine;
        public List<RectangleModel> rectangles;
        public List<PointModel> points;
        public List<LineModel> lines;
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
        _model.rectangles = new List<RectangleModel>();
        _model.points = new List<PointModel>();
        _model.lines = new List<LineModel>();
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
                        if (c.IsCurrentlyDrawn) c.Delete();
                    });
                    _model.focusPosition = null;
                }

                // delete next rectangle
                if (_model.nextRectangle != null)
                {
                    RectangleCreation.AbortRectangle(_model.nextRectangle);
                    _model.nextRectangle = null;
                }

                // delete next rectangle
                if (_model.nextLine != null)
                {
                    LineCreation.AbortLine(_model.nextLine);
                    _model.nextLine = null;
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

                //update rectangle while drawing
                if (_model.nextRectangle != null)
                    RectangleCreation.UpdateRectangle(_model.nextRectangle, _model.focusPosition);
                //update rectangle while drawing
                if (_model.nextLine != null)
                    LineCreation.UpdateLine(_model.nextLine, _model.focusPosition);

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdateUI();
    }

    private void Draw()
    {
        CoordinateCreation.BakePosition(_model.focusPosition);

        _model.coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

        switch (_currentGeometryType)
        {
            case GeometryType.Point:
                _model.points.Add(PointCreation.NewPoint(_model.focusPosition));
                break;
            case GeometryType.Line:
                if (_model.nextLine == null)
                {
                    _model.nextLine = LineCreation.StartNewLine(_model.focusPosition);
                    _model.lines.Add(_model.nextLine);
                }
                else
                {
                    LineCreation.CompleteLine(_model.nextLine, _model.focusPosition);
                    _model.nextLine = null;
                }

                break;
            case GeometryType.Rectangle:
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
        _ui.rectanglesUI.UpdateUI(_model.rectangles, _sketchStyle.GeometryStyle.Rectangle);
        _ui.pointsUI.UpdateUI(_model.points, _sketchStyle.GeometryStyle.Points);
        _ui.linesUI.UpdateUI(_model.lines, _sketchStyle.GeometryStyle.Lines);
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
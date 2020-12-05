using System;
using System.Collections.Generic;
using System.Linq;
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
    private struct InteractionState
    {
        public Coordinate draggedCoordinate;
        public Vec<Coordinate> focusPosition;
        public KeyboardInput.Model keyboardInputModel;
        public GeometryModel incompleteGeometry;

        public void Reset()
        {
            draggedCoordinate = null;
            keyboardInputModel = new KeyboardInput.Model();
            incompleteGeometry = null;
            focusPosition = null;
        }
    }

    private void Initialize(Vec<float> mousePositionAsOrigin)
    {
        _model.coordinateSystem = new CoordinateSystem(mousePositionAsOrigin);
        _model.geometries = new List<GeometryModel>();
        _interactionState.keyboardInputModel = new KeyboardInput.Model();
//        _model.coordinateSystem.CoordinateSystemChangedEvent += UpdateUI;
        _ui.coordinateSystemUI.Initialize();

        //start drawing first rectangle
        _state = State.DrawRectangle;
        _interactionState.focusPosition = CoordinateCreation.UpdateCursorPosition(
            _interactionState.focusPosition,
            _model.coordinateSystem,
            _interactionState.keyboardInputModel
        );
        _history = new History(HistoryPositionChangedHandler);
        AddPointToDrawing();
    }


    private void HistoryPositionChangedHandler(SketchModel.Serialization serializationToSet)
    {
        _model.SetSerialization(serializationToSet);
        _interactionState.Reset();
    }

    private void SaveToHistory()
    {
        _history.AddToHistory(_model.Serialize());
    }

    public Texture2D DefaultCursor;
    public Texture2D VerticalManipulationCursor;
    public Texture2D HorizontalManipulationCursor;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Update()
    {
        var hasBeenInitialized = _model.coordinateSystem != null;
        if (!hasBeenInitialized)
        {
            if (Input.GetKeyDown(PrimaryMouse))
                Initialize(MouseInput.RaycastPosition);

            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _model.SetSerialization(_history.Undo());
            _interactionState.Reset();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _model.SetSerialization(_history.Redo());
            _interactionState.Reset();
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

                var hitResult = CoordinateManipulation.TryGetCoordinateAtPosition(_ui.coordinateSystemUI);

                var cursor = DefaultCursor;
                if (hitResult.HasValue)
                {
                    cursor = hitResult.Value.axis == Vec.AxisID.X
                        ? HorizontalManipulationCursor
                        : VerticalManipulationCursor;
                }

                Cursor.SetCursor(cursor, hotSpot, cursorMode);

                Debug.Log(hitResult);

                // start drag
                if (Input.GetKeyDown(PrimaryMouse) && hitResult.HasValue)
                {
                    _interactionState.draggedCoordinate = hitResult.Value.coordinate;
                    CoordinateManipulation.TryGetCoordinateAtPosition(_ui.coordinateSystemUI);
                }

                // update drag
                if (Input.GetKey(PrimaryMouse) && _interactionState.draggedCoordinate != null)
                {
                    var (value, pointsInNegativeDirection) = CoordinateManipulation.UpdateDrag(
                        _interactionState.draggedCoordinate,
                        _model.coordinateSystem.AxisThatContainsCoordinate(_interactionState.draggedCoordinate));

                    _interactionState.draggedCoordinate.Parameter.Value = value;
                    //quick fix: for now, only mue coordinates can be dragged
                    ((Mue) _interactionState.draggedCoordinate).PointsInNegativeDirection =
                        pointsInNegativeDirection;
                }

                // stop drag
                if (Input.GetKeyUp(PrimaryMouse) && _interactionState.draggedCoordinate != null)
                {
                    SaveToHistory();
                    _interactionState.draggedCoordinate = null;
                }

                break;

            case State.DrawRectangle:

                KeyboardInput.UpdateKeyboardInput(ref _interactionState.keyboardInputModel,
                    _model.coordinateSystem.GetAllParameters()
                );

                _interactionState.focusPosition = CoordinateCreation.UpdateCursorPosition(
                    _interactionState.focusPosition,
                    _model.coordinateSystem,
                    _interactionState.keyboardInputModel
                );

                if (_interactionState.focusPosition == null)
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
                    AddPointToDrawing();
                }

                // update geometry while drawing
                switch (_interactionState.incompleteGeometry)
                {
                    case RectangleModel rectangleModel:
                        RectangleCreation.UpdateRectangle(rectangleModel, _interactionState.focusPosition);
                        break;
                    case LineModel lineModel:
                        LineCreation.UpdateLine(lineModel, _interactionState.focusPosition);
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

    private void AddPointToDrawing()
    {
        CoordinateCreation.BakePosition(_interactionState.focusPosition);
        _model.coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

        switch (_currentGeometryType)
        {
            case GeometryType.Point:
                _model.geometries.Add(PointCreation.NewPoint(_interactionState.focusPosition));
                SaveToHistory();
                break;
            case GeometryType.Line:
                if (!(_interactionState.incompleteGeometry is LineModel))
                {
                    _interactionState.incompleteGeometry =
                        LineCreation.StartNewLine(_interactionState.focusPosition);
                    _model.geometries.Add(_interactionState.incompleteGeometry);
                }
                else
                {
                    LineCreation.CompleteLine(_interactionState.incompleteGeometry as LineModel,
                        _interactionState.focusPosition);
                    _interactionState.incompleteGeometry = null;
                    SaveToHistory();
                }

                break;

            case GeometryType.Rectangle:
                if (!(_interactionState.incompleteGeometry is RectangleModel))
                {
                    _interactionState.incompleteGeometry =
                        RectangleCreation.StartNewRectangle(_interactionState.focusPosition);
                    _model.geometries.Add(_interactionState.incompleteGeometry);
                }
                else
                {
                    RectangleCreation.CompleteRectangle(_interactionState.incompleteGeometry as RectangleModel,
                        _interactionState.focusPosition);
                    _interactionState.incompleteGeometry = null;
                    SaveToHistory();
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        // reset input
        _interactionState.keyboardInputModel = new KeyboardInput.Model();
    }

    private void UpdateUI()
    {
        _ui.coordinateSystemUI.UpdateUI(
            _model.coordinateSystem,
            _sketchStyle.CoordinateUIStyle,
            _interactionState.keyboardInputModel,
            _interactionState.draggedCoordinate);

        _ui.geometryUI.UpdateUI(_model.geometries, _sketchStyle._geometryStyleAsset.Set);
        _ui.paramterUI.UpdateUI(_model.coordinateSystem.GetAllParameters());
    }

    private void CleanUpIncompleteGeometry()
    {
        if (_interactionState.incompleteGeometry == null)
            return;

        _interactionState.incompleteGeometry.P0.ForEach(c =>
            c.UnregisterGeometryAndTryToDelete(_interactionState.incompleteGeometry));
        _model.geometries.Remove(_interactionState.incompleteGeometry);
        _interactionState.incompleteGeometry = null;
    }

    private void CleanUpFocusPosition()
    {
        if (_interactionState.focusPosition == null)
            return;

        _interactionState.focusPosition.ForEach(c =>
        {
            if (c.IsCurrentlyDrawn) c.Delete();
        });
        _interactionState.focusPosition = null;
    }

    private enum State
    {
        ManipulateCoordinates,
        DrawRectangle,
    }

    private SketchModel _model;
    private InteractionState _interactionState;
    private State _state = State.ManipulateCoordinates;
    private GeometryType _currentGeometryType;
    private History _history;

    
    private const KeyCode ManipulateCoordinatesStateKey = KeyCode.Alpha1;
    private const KeyCode DrawRectanglesStateKey = KeyCode.Alpha2;
    private const KeyCode PrimaryMouse = KeyCode.Mouse0;
    private const KeyCode SetAnchorKey = KeyCode.Mouse1;
    private const KeyCode DeleteKey = KeyCode.Mouse2;
    private const KeyCode SwitchGeometry = KeyCode.Alpha3;
}
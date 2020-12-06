using System;
using System.Collections.Generic;
using Model;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Sketch : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private SketchStyle _sketchStyle;

    [Serializable]
    public struct UI
    {
        public CoordinateSystemUI coordinateSystemUI;
        public GeometryUIManager geometryUI;
        [FormerlySerializedAs("paramterUI")] public ParameterUI parameterUI;
        public CursorUI cursorUI;
        public ButtonsUI _buttonsUI;
        public ControlPanel _controlPanel;
    }

    [Serializable]
    public struct ButtonsUI
    {
        public Button DrawPointsButton;
        public Button DrawLinesButton;
        public Button DrawRectsButton;
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
_ui._controlPanel.Initialize(HandleCommand);
        _ui._buttonsUI.DrawPointsButton.onClick.AddListener(() =>
        {
            _currentGeometryType = GeometryType.Point;
            CleanUpIncompleteGeometry();
            UpdateUI();
        });
        _ui._buttonsUI.DrawLinesButton.onClick.AddListener(() =>
        {
            _currentGeometryType = GeometryType.Line;
            CleanUpIncompleteGeometry();
            UpdateUI();
        });
        _ui._buttonsUI.DrawRectsButton.onClick.AddListener(() =>
        {
            _currentGeometryType = GeometryType.Rectangle;
            CleanUpIncompleteGeometry();
            UpdateUI();
        });

        //start drawing first rectangle
        _state = State.Drawing;
        _interactionState.focusPosition = CoordinateCreation.UpdateCursorPosition(
            _interactionState.focusPosition,
            _model.coordinateSystem,
            _interactionState.keyboardInputModel
        );
        _history = new History(HistoryPositionChangedHandler);
        AddPointToDrawing();
    }

    private void HandleCommand(Command buttonType)
    {
        switch (buttonType)
        {
            case Command.Transform:
                SetState(State.Manipulating);

                break;
            case Command.Undo:
                _model.SetSerialization(_history.Undo());
                _interactionState.Reset();
                break;
            case Command.Redo:
                _model.SetSerialization(_history.Redo());
                _interactionState.Reset();
                break;
            case Command.DrawPoint:
                SetState(State.Drawing);
                _currentGeometryType = GeometryType.Point;
                CleanUpIncompleteGeometry();
                break;
            case Command.DrawLine:
                SetState(State.Drawing);
                _currentGeometryType = GeometryType.Line;
                CleanUpIncompleteGeometry();
                break;
            case Command.DrawRect:
                SetState(State.Drawing);
                _currentGeometryType = GeometryType.Rectangle;
                CleanUpIncompleteGeometry();
                break;
            case Command.ColorBlack:
                break;
            case Command.ColorGrey:
                break;
            case Command.ColorWhite:
                break;
            case Command.Help:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
        }
    }

    private void Update()
    {
        var hasBeenInitialized = _model.coordinateSystem != null;
        if (!hasBeenInitialized && Input.GetKeyDown(PrimaryMouse) && IsMouseOnDrawArea())
        {
            Initialize(MouseInput.RaycastPosition);
            return;
        }

        var hotKeyCommand = HotKeyInput.Update();
        if(hotKeyCommand.HasValue)
            HandleCommand(hotKeyCommand.Value);

        switch (_state)
        {
            case State.Manipulating:

                var hitResult = CoordinateManipulation.TryGetCoordinateAtPosition(_ui.coordinateSystemUI);

                _ui.cursorUI.UpdateCursor(hitResult);

                // start drag
                if (Input.GetKeyDown(PrimaryMouse) && hitResult.HasValue && IsMouseOnDrawArea())
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

            case State.Drawing:

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
                if (Input.GetKeyDown(PrimaryMouse) && IsMouseOnDrawArea() || Input.GetKeyDown(KeyCode.Return))
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

    private void HistoryPositionChangedHandler(SketchModel.Serialization serializationToSet)

    {
        _model.SetSerialization(serializationToSet);
        _interactionState.Reset();
    }

    private void SaveToHistory()
    {
        _history.AddToHistory(_model.Serialize());
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.Manipulating:
                CleanUpFocusPosition();
                CleanUpIncompleteGeometry();
                break;
            case State.Drawing:
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
        _ui.parameterUI.UpdateUI(_model.coordinateSystem.GetAllParameters());
    }

    private bool IsMouseOnDrawArea() => Input.mousePosition.x > 30;

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
        Manipulating,
        Drawing,
    }

    private SketchModel _model;
    private InteractionState _interactionState;
    private State _state = State.Manipulating;
    private GeometryType _currentGeometryType;
    private History _history;


    
    private const KeyCode PrimaryMouse = KeyCode.Mouse0;
    private const KeyCode SetAnchorKey = KeyCode.Mouse1;
    private const KeyCode DeleteKey = KeyCode.Mouse2;
}
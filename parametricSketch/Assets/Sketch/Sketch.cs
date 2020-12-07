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
        public ControlPanel _controlPanel;
    }

    [Serializable]
    private struct InteractionState
    {
        public Coordinate draggedCoordinate;
        public Vec<Coordinate> focusPosition;
        public KeyboardInput.Model keyboardInputModel;
        public GeometryModel incompleteGeometry;
        public ColorAsset currentGeometryColor;

        public void Reset()
        {
            draggedCoordinate = null;
            keyboardInputModel = new KeyboardInput.Model();
            incompleteGeometry = null;
            focusPosition = null;
            currentGeometryColor = null;
        }
    }

    private void Initialize(Vec<float> mousePositionAsOrigin)
    {
        _sketchModel.coordinateSystem = new CoordinateSystem(mousePositionAsOrigin);
        _sketchModel.geometries = new List<GeometryModel>();
        _interactionState.keyboardInputModel = new KeyboardInput.Model();
//        _model.coordinateSystem.CoordinateSystemChangedEvent += UpdateUI;
        _ui.coordinateSystemUI.Initialize();
        _ui._controlPanel.Initialize(HandleCommand);

        _appModel = new AppModel()
        {
            Tool = Tool.Drawing,
            CurrentGeometryType = GeometryType.Rectangle,
            CurrentgeometryColor = GeometryStyleAsset.GeometryColor.White,
        };

        //start drawing first rectangle
        _interactionState.focusPosition = CoordinateCreation.UpdateCursorPosition(
            _interactionState.focusPosition,
            _sketchModel.coordinateSystem,
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
                SetState(Tool.Transform);
                break;
            case Command.Undo:
                _sketchModel.SetSerialization(_history.Undo());
                _interactionState.Reset();
                break;
            case Command.Redo:
                _sketchModel.SetSerialization(_history.Redo());
                _interactionState.Reset();
                break;
            case Command.DrawPoint:
                SetState(Tool.Drawing);
                _appModel.CurrentGeometryType = GeometryType.Point;
                CleanUpIncompleteGeometry();
                break;
            case Command.DrawLine:
                SetState(Tool.Drawing);
                _appModel.CurrentGeometryType = GeometryType.Line;
                CleanUpIncompleteGeometry();
                break;
            case Command.DrawRect:
                SetState(Tool.Drawing);
                _appModel.CurrentGeometryType = GeometryType.Rectangle;
                CleanUpIncompleteGeometry();
                break;
            case Command.ColorBlack:
                _appModel.CurrentgeometryColor = GeometryStyleAsset.GeometryColor.Black;
                break;
            case Command.ColorGrey:
                _appModel.CurrentgeometryColor = GeometryStyleAsset.GeometryColor.Grey;
                break;
            case Command.ColorWhite:
                _appModel.CurrentgeometryColor = GeometryStyleAsset.GeometryColor.White;
                break;
            case Command.Help:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
        }
    }

    private void Update()
    {
        var hasBeenInitialized = _sketchModel.coordinateSystem != null;
        if (!hasBeenInitialized)
        {
            if (Input.GetKeyDown(PrimaryMouse) && IsMouseOnDrawArea())
                Initialize(MouseInput.RaycastPosition);
            return;
        }

        var hotKeyCommand = HotKeyInput.Update();
        if (hotKeyCommand.HasValue)
            HandleCommand(hotKeyCommand.Value);

        switch (_appModel.Tool)
        {
            case Tool.Transform:

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
                        _sketchModel.coordinateSystem.AxisThatContainsCoordinate(_interactionState.draggedCoordinate));

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

            case Tool.Drawing:

                KeyboardInput.UpdateKeyboardInput(ref _interactionState.keyboardInputModel,
                    _sketchModel.coordinateSystem.GetAllParameters()
                );

                _interactionState.focusPosition = CoordinateCreation.UpdateCursorPosition(
                    _interactionState.focusPosition,
                    _sketchModel.coordinateSystem,
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
                    CoordinateCreation.DeletePositionAtMousePosition(_sketchModel.coordinateSystem);
                }

                // set anchor
                if (Input.GetKeyDown(SetAnchorKey))
                {
                    var mousePosition = MouseInput.RaycastPosition;
                    _sketchModel.coordinateSystem.SetAnchorPosition(mousePosition);
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
        _sketchModel.SetSerialization(serializationToSet);
        _interactionState.Reset();
    }

    private void SaveToHistory()
    {
        _history.AddToHistory(_sketchModel.Serialize());
    }

    private void SetState(Tool newTool)
    {
        switch (newTool)
        {
            case Tool.Transform:
                CleanUpFocusPosition();
                CleanUpIncompleteGeometry();
                break;
            case Tool.Drawing:
                _ui.cursorUI.ResetCursor();

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newTool), newTool, null);
        }

        _appModel.Tool = newTool;
    }

    private void AddPointToDrawing()
    {
        CoordinateCreation.BakePosition(_interactionState.focusPosition);
        _sketchModel.coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

        switch (_appModel.CurrentGeometryType)
        {
            case GeometryType.Point:
                _sketchModel.geometries.Add(PointCreation.NewPoint(_interactionState.focusPosition));
                SaveToHistory();
                break;
            case GeometryType.Line:
                if (!(_interactionState.incompleteGeometry is LineModel))
                {
                    _interactionState.incompleteGeometry =
                        LineCreation.StartNewLine(_interactionState.focusPosition);
                    _sketchModel.geometries.Add(_interactionState.incompleteGeometry);
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
                        RectangleCreation.StartNewRectangle(_interactionState.focusPosition,
                            _appModel.CurrentgeometryColor);
                    _sketchModel.geometries.Add(_interactionState.incompleteGeometry);
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
            _sketchModel.coordinateSystem,
            _sketchStyle.CoordinateUIStyle,
            _interactionState.keyboardInputModel,
            _interactionState.draggedCoordinate);

        _ui.geometryUI.UpdateUI(_sketchModel.geometries, _sketchStyle._geometryStyleAsset.Set);
        _ui.parameterUI.UpdateUI(_sketchModel.coordinateSystem.GetAllParameters());
        _ui._controlPanel.UpdateUI(_appModel);
    }

    private bool IsMouseOnDrawArea() => Input.mousePosition.x > 30;

    private void CleanUpIncompleteGeometry()
    {
        if (_interactionState.incompleteGeometry == null)
            return;

        _interactionState.incompleteGeometry.P0.ForEach(c =>
            c.UnregisterGeometryAndTryToDelete(_interactionState.incompleteGeometry));
        _sketchModel.geometries.Remove(_interactionState.incompleteGeometry);
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

    public enum Tool
    {
        Transform,
        Drawing,
    }

    private SketchModel _sketchModel;
    private AppModel _appModel;
    private InteractionState _interactionState;
    private History _history;

    public struct AppModel
    {
        public Tool Tool;
        public GeometryType CurrentGeometryType;
        public GeometryStyleAsset.GeometryColor CurrentgeometryColor;
    }

    private const KeyCode PrimaryMouse = KeyCode.Mouse0;
    private const KeyCode SetAnchorKey = KeyCode.Mouse1;
    private const KeyCode DeleteKey = KeyCode.Mouse2;
}
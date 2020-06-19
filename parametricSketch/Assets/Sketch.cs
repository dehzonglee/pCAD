using System;
using Model;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField] private UI _ui;

    [Serializable]
    public struct UI
    {
        public CoordinateSystemUI CoordinateSystemUi;
        public Rectangle RectanglePrefab;
    }

    private (Axis DraggedAxis, Coordinate DraggedCoordinate)? _draggedCoordinate;

    private void Start()
    {
        _coordinateSystem = new CoordinateSystem();
        _ui.CoordinateSystemUi.Initialize(_coordinateSystem, ModelChangeRequestHandler);
    }

    private ParametricPosition _nextPosition;

    private bool _isDragging => _draggedCoordinate != null;

    private void Update()
    {
        // switch input state
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _nextPosition?.RemovePreview();
            _nextPosition = null;

            if (_nextRectangle != null)
            {
                _nextRectangle.Abort();
                _nextRectangle = null;
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

                if (Input.GetMouseButton(0) && _isDragging)
                    UpdateDrag();

                if (Input.GetMouseButtonUp(0) && _isDragging)
                    CompleteDrag();

                break;

            case State.DrawRectangle:

                //update preview
                _nextPosition?.RemovePreview();
                _nextPosition = GetOrCreatePositionAtMousePosition(true);

                // delete
                if (Input.GetMouseButtonDown(2))
                {
                    var p = GetOrCreatePositionAtMousePosition();
                    p.Remove();
                }

                // set anchor
                if (Input.GetMouseButtonDown(1))
                {
                    var mousePosition = MouseInput.RaycastPosition;
                    _coordinateSystem.SetAnchorPosition(mousePosition);
                }

                // draw
                if (Input.GetMouseButtonDown(0))
                {
                    _nextPosition.BakePreview();
                    _coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

                    if (_nextRectangle == null)
                    {
                        _nextRectangle = Instantiate(_ui.RectanglePrefab);
                        _nextRectangle.Initialize();
                        _nextRectangle.SetFirstPosition(_nextPosition);
                    }
                    else
                    {
                        _nextRectangle.SetSecondPosition(_nextPosition);
                        _nextRectangle = null;
                    }
                }

                if (_nextRectangle != null)
                {
                    _nextRectangle.SetSecondPosition(_nextPosition);
                }

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        _ui.CoordinateSystemUi.UpdateUI();
    }

    private ParametricPosition GetOrCreatePositionAtMousePosition(bool asPreview = false)
    {
        var mousePosition = MouseInput.RaycastPosition;
        var position = _coordinateSystem.GetParametricPosition(mousePosition, asPreview);
        _coordinateSystem.SetAnchorPosition(position.Value);

        return position;
    }

    private void TryStartDrag()
    {
        _draggedCoordinate = MouseInput.RaycastCoordinateUI;
    }

    private void UpdateDrag()
    {
        var coordinate = _draggedCoordinate.Value.DraggedCoordinate;
        var axis = _draggedCoordinate.Value.DraggedAxis;
        ModelChangeRequestHandler(_draggedCoordinate.Value.DraggedAxis, _draggedCoordinate.Value.DraggedCoordinate,
            MousePositionToParameter(MouseInput.RaycastPosition, coordinate, axis));
    }

    private float MousePositionToParameter(Vector3 mouseWorldPosition, Coordinate coordinate, Axis axis)
    {
        return Vector3.Dot(mouseWorldPosition, axis.Direction) - coordinate.ParentValue;
    }


    private void CompleteDrag()
    {
      _draggedCoordinate= null;
    }

    private void ModelChangeRequestHandler(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;
    }

    private CoordinateSystem _coordinateSystem;
    private Rectangle _nextRectangle;

    private enum State
    {
        ManipulateCoordinates,
        DrawRectangle,
    }

    private State _state = State.ManipulateCoordinates;
}
using System;
using Model;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField] private CoordinateSystemUI _coordinateSystemUi;
    [SerializeField] private Line _linePrefab;

    [SerializeField] private Rectangle _rectanglePrefab;

    private void Start()
    {
        _coordinateSystem = new CoordinateSystem();
        _coordinateSystemUi.Initialize(_coordinateSystem, ModelChangeRequestHandler);
    }

    private ParametricPosition _nextPosition;

    private bool _isDragging => _draggedCoordinateUi != null;

    private CoordinateUI _draggedCoordinateUi;

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
                    _coordinateSystemUi.UpdateUI();
                }

                // draw
                if (Input.GetMouseButtonDown(0))
                {
                    _nextPosition.BakePreview();
                    _coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);

                    if (_nextRectangle == null)
                    {
                        _nextRectangle = Instantiate(_rectanglePrefab);
                        _nextRectangle.Initialize();
                        _nextRectangle.SetFirstPosition(_nextPosition);
                    }
                    else
                    {
//                        _nextRectangle.SetSecondPosition(_nextPosition);
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

        _coordinateSystemUi.UpdateUI();
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
        _draggedCoordinateUi = MouseInput.RaycastCoordinateUI;
    }

    private void UpdateDrag()
    {
        _draggedCoordinateUi.ManipulateCoordinate(MouseInput.RaycastPosition);
        _coordinateSystemUi.UpdateUI();
    }

    private void CompleteDrag()
    {
        _draggedCoordinateUi = null;
    }

    private void ModelChangeRequestHandler(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;
        _coordinateSystemUi.UpdateUI();
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
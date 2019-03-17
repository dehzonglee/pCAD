using Model;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField] private Line _linePrefab;

    [SerializeField] private Rectangle _rectanglePrefab;

    private void Start()
    {
        _coordinateSystem = new CoordinateSystem();
        _coordinateSystemUi = GetComponent<CoordinateSystemUI>();
        _coordinateSystemUi.Initialize(_coordinateSystem, ModelChangeRequestHandler);
    }

    private ParametricPosition _nextPosition;

    private bool _isDragging => _draggedCoordinateUi != null;

    private CoordinateUI _draggedCoordinateUi;

    private void Update()
    {
        _nextPosition?.RemovePreview();
        _nextPosition = GeneratePositionAtMousePosition(true);
        _coordinateSystemUi.UpdateUI();

        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            UpdateDrag();
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            CompleteDrag();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            var p = GeneratePositionAtMousePosition();
            p.RemovePreview();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            var mousePosition = MouseInput.RaycastPosition;
            _coordinateSystem.SetAnchorPosition(mousePosition);
            _coordinateSystemUi.UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _nextPosition.BakePreview();
            _coordinateSystem.SetAnchorPosition(MouseInput.RaycastPosition);
            _coordinateSystemUi.UpdateUI();
//            var position = GeneratePositionAtMousePosition();


//
//            if (_nextRectangle == null)
//            {
//                _nextRectangle = Instantiate(_rectanglePrefab);
//                _nextRectangle.SetFirstPosition(position);
//            }
//
//            else
//            {
//                _nextRectangle.SetSecondPosition(position);
//                _nextRectangle = null;
//            }
        }
    }

    private ParametricPosition GeneratePositionAtMousePosition(bool asPreview = false)
    {
        var mousePosition = MouseInput.RaycastPosition;
        var position = _coordinateSystem.GetParametricPosition(mousePosition, asPreview);
/*
        _coordinateSystem.SetAnchorPosition(position.Value);
*/
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

    private CoordinateSystemUI _coordinateSystemUi;
    private CoordinateSystem _coordinateSystem;
    private Rectangle _nextRectangle;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField]
    Line _linePrefab;

    [SerializeField]
    Rectangle _rectanglePrefab;

    void Start()
    {
        _coordinateSystem = new CoordinateSystem();
        _coordinateSystemUI = GetComponent<CoordinateSystemUI>();
        _coordinateSystemUI.Initialize(_coordinateSystem, ModelChangeRequestHandler);
    }

    private bool _isDragging
    {
        get { return _draggedCoordinateUI != null; }
    }

    private CoordinateUI _draggedCoordinateUI;

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            var mousePosition = MouseInput.RaycastPosition;
            _coordinateSystem.SetAnchorPosition(mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var mousePosition = MouseInput.RaycastPosition;
            var position = _coordinateSystem.GetParametricPosition(mousePosition);
            _coordinateSystem.SetAnchorPosition(position.Value);
            if (_nextRectangle == null)
            {
                _nextRectangle = Instantiate(_rectanglePrefab);
                _nextRectangle.SetFirstPosition(position);
            }
            else
            {
                _nextRectangle.SetSecondPosition(position);
                _nextRectangle = null;
            }
            _coordinateSystemUI.UpdateUI();
        }
    }

    private void TryStartDrag()
    {
        _draggedCoordinateUI = MouseInput.RaycastCoordinateUI;
    }

    private void UpdateDrag()
    {
        _draggedCoordinateUI.ManipulateCoordinate(MouseInput.RaycastPosition);
    }

    private void CompleteDrag()
    {
        _draggedCoordinateUI = null;
    }


    private void ModelChangeRequestHandler(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;
    }


    private CoordinateSystemUI _coordinateSystemUI;
    private CoordinateSystem _coordinateSystem;
    private Rectangle _nextRectangle;
}

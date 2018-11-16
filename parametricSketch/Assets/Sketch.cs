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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var mousePosition = MouseInput.WorldSpacePosition;
            _coordinateSystem.SetAnchorPosition(mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var mousePosition = MouseInput.WorldSpacePosition;
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

    private void ModelChangeRequestHandler(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;
    }


    private CoordinateSystemUI _coordinateSystemUI;
    private CoordinateSystem _coordinateSystem;
    private Rectangle _nextRectangle;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    [SerializeField]
    Line _linePrefab;

    void Start()
    {
        _coordinateSystem = new CoordinateSystem();
        GetComponent<CoordinateSystemUI>().Initialize(_coordinateSystem, ModelChangeRequest);


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
            if (_nextLine == null)
            {
                _nextLine = Instantiate(_linePrefab);
                _nextLine.SetFirstPosition(position);
                return;
            }
            _nextLine.SetSecondPosition(position);
            _nextLine = Instantiate(_linePrefab);
            _nextLine.SetFirstPosition(position);
        }
    }

    private void ModelChangeRequest(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;
    }

    private CoordinateSystem _coordinateSystem;
    private Line _nextLine;
}

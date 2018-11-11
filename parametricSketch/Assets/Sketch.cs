using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(MouseInput))]
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
            _coordinateSystem.SetAnchor(mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var mousePosition = MouseInput.WorldSpacePosition;
            var position = _coordinateSystem.GetParametricPosition(mousePosition);
            if (_nextLine == null)
            {
                _nextLine = Instantiate(_linePrefab);
                _nextLine.SetFirstPosition(position);
                return;
            }
            _nextLine.SetSecondPosition(position);
            _nextLine = null;
            // _coordinateSystem.SetAnchor(position.Value);
        }
    }

    private void ModelChangeRequest(Axis axis, Coordinate coordinate, float value)
    {
        coordinate.Parameter = value;

    }

    private CoordinateSystem _coordinateSystem;
    private Line _nextLine;
}

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
    }

    void Update()
    {
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
        }
    }

    private CoordinateSystem _coordinateSystem;
    private Line _nextLine;
}

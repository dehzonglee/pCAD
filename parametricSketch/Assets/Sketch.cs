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
        // _mouseInput = GetComponent<MouseInput>();
        _axis = new Dictionary<int, Axis>();
        _axis.Add(Dimensions.X, new Axis());
        _axis.Add(Dimensions.Y, new Axis());
        _axis.Add(Dimensions.Z, new Axis());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var mousePosition = MouseInput.WorldSpacePosition;
            var x = _axis[Dimensions.X].GetCoordiante(mousePosition.x);
            var y = _axis[Dimensions.Y].GetCoordiante(mousePosition.y);
            var z = _axis[Dimensions.Z].GetCoordiante(mousePosition.z);
            var position = new Position(x, y, z);
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

    // private MouseInput _mouseInput;
    private Dictionary<int, Axis> _axis;
    private Line _nextLine;
    private Coordinate[] _origin;
}

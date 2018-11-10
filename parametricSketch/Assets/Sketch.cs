using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sketch : MonoBehaviour
{
    void Start()
    {
        var oX = new Origin();
        var oY = new Origin();
        var oZ = new Origin();
        _origin = new Coordinate[] { oX, oY, oZ };

        _anchor = new Dictionary<int, Coordinate>();
        _anchor.Add(Dimensions.X, oX);
        _anchor.Add(Dimensions.Y, oY);
        _anchor.Add(Dimensions.Z, oZ);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var position = MouseInput.WorldSpaceXZPosition;
            if (_nextLine == null)
            {
                _nextLine = new Line();
                // _nextLine.SetFirstPosition(position);
            }
        }
    }



    private Dictionary<int, Coordinate> _anchor;

    private Line _nextLine;
    private Coordinate[] _origin;
}

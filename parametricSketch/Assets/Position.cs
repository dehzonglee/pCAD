using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    public Position(Coordinate[] coordinates)
    {
        _coordinates = coordinates;
    }

    public Vector2 Value
    {
        get
        {
            var x = _coordinates[Dimensions.X].Value;
            var y = _coordinates[Dimensions.Y].Value;
            return new Vector2(x, y);
        }
    }

    Coordinate[] _coordinates;
}

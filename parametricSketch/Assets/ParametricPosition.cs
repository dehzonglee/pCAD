using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametricPosition
{
    public event Action PositionChangedEvent;

    public ParametricPosition(Coordinate x, Coordinate y, Coordinate z)
    {
        _coordinates = new Coordinate[] { x, y, z };
        x.ValueChangedEvent += PositionChangedEvent;
        y.ValueChangedEvent += PositionChangedEvent;
        z.ValueChangedEvent += PositionChangedEvent;
    }

    public Vector3 Value
    {
        get
        {
            var x = _coordinates[Dimensions.X].Value;
            var y = _coordinates[Dimensions.Y].Value;
            var z = _coordinates[Dimensions.Z].Value;
            return new Vector3(x, y, z);
        }
    }

    Coordinate[] _coordinates;
}

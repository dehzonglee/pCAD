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
        x.ValueChangedEvent += () =>
        {
            PositionChangedEvent();
        };
        y.ValueChangedEvent += () =>
        {
            PositionChangedEvent();
        };
        z.ValueChangedEvent += () =>
        {
            PositionChangedEvent();
        };
    }

    public void SetCoordinate(Coordinate coordinate, int dimension)
    {
        var oldCoordinate = _coordinates[dimension];
        oldCoordinate.ValueChangedEvent -= PositionChangedEvent;
        _coordinates[dimension] = coordinate;
        coordinate.ValueChangedEvent += PositionChangedEvent;
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

    public Coordinate X { get { return _coordinates[Dimensions.X]; } }
    public Coordinate Y { get { return _coordinates[Dimensions.Y]; } }
    public Coordinate Z { get { return _coordinates[Dimensions.Z]; } }

    Coordinate[] _coordinates;
}

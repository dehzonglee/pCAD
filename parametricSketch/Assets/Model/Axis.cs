using System;
using System.Collections.Generic;
using UnityEngine;

public class Axis
{
    public event Action AnchorChangedEvent;
    public List<Coordinate> Coordinates { get { return _coordinates; } }
    public Axis()
    {
        _origin = new Origin();
        _coordinates.Add(_origin);
        _anchor = _origin;
    }

    public Coordinate GetCoordiante(float position)
    {
        var closestCoordinate = FindClosestCoordinate(position);
        var distance = Mathf.Abs(position - closestCoordinate.Value);
        if (distance < SNAP_RADIUS)
            return closestCoordinate;

        var delta = position - _anchor.Value;
        var newCoordinate = new Mue(_anchor, delta);
        _coordinates.Add(newCoordinate);
        return newCoordinate;
    }

    public Coordinate GetAnchor()
    {
        return _anchor;
    }

    public float SmallestValue
    {
        get
        {
            var smallesValue = float.PositiveInfinity;
            foreach (var c in _coordinates)
            {
                if (c.Value < smallesValue)
                    smallesValue = c.Value;
            }
            return smallesValue;
        }
    }


    public Coordinate SetAnchor(float position)
    {
        _anchor = FindClosestCoordinate(position);
        if (AnchorChangedEvent != null)
            AnchorChangedEvent();
        return _anchor;
    }

    private Coordinate FindClosestCoordinate(float position)
    {
        Coordinate closestCoordinate = _origin;
        var closestDistance = Mathf.Abs(_origin.Value - position);
        foreach (var c in _coordinates)
        {
            var distance = Mathf.Abs(c.Value - position);
            if (distance < closestDistance)
            {
                closestCoordinate = c;
                closestDistance = distance;
            }
        }
        return closestCoordinate;
    }

    private const float SNAP_RADIUS = 0.01f;
    private List<Coordinate> _coordinates = new List<Coordinate>();
    private Coordinate _origin;
    private Coordinate _anchor;
}
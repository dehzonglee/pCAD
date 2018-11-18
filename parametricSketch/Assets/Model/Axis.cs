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
        _anchor = new AnchorCoordinates(_origin, _origin);
    }

    public Coordinate GetCoordiante(float position)
    {
        var closestCoordinate = GetClosestCoordinateInSnapRadius(position);
        if (closestCoordinate != null)
            return closestCoordinate;

        var newCoordinate = AddNewMueCoordiante(position);
        return newCoordinate;
    }

    public AnchorCoordinates Anchor { get { return _anchor; } }

    public void RemoveUnusedCoordinate()
    {
        var cleanList = new List<Coordinate>();
        foreach (var c in _coordinates)
        {
            if (c.IsUsed)
                cleanList.Add(c);
        }
        _coordinates = cleanList;
    }

    public float SmallestValue
    {
        get
        {
            var smallesValue = float.PositiveInfinity;
            foreach (var c in _coordinates)
            {
                if (c.Value < smallesValue)
                {
                    smallesValue = c.Value;
                }
            }
            return smallesValue;
        }
    }

    public AnchorCoordinates SnapAnchorToClosestCoordinate(float position)
    {
        _anchor.SetPrimaryCoordinate(FindClosestCoordinate(position));
        if (AnchorChangedEvent != null)
            AnchorChangedEvent();
        return _anchor;
    }

    private Coordinate AddNewMueCoordiante(float position)
    {
        var delta = position - _anchor.PrimaryCoordinate.Value;
        var newCoordinate = new Mue(_anchor.PrimaryCoordinate, delta);
        _coordinates.Add(newCoordinate);
        return newCoordinate;
    }

    private Coordinate GetClosestCoordinateInSnapRadius(float position)
    {
        var closestCoordinate = FindClosestCoordinate(position);
        var distanceToClosestCoordinate = Mathf.Abs(position - closestCoordinate.Value);
        var distanceToLambdaCoordiante = DistanceToLambdaCoordiante(position);

        if (distanceToClosestCoordinate > SNAP_RADIUS && distanceToLambdaCoordiante > SNAP_RADIUS)
            return null;

        if (distanceToClosestCoordinate < distanceToLambdaCoordiante)
            return closestCoordinate;

        return AddLambdaCoordinateBetweenAnchors();
    }

    private Lambda AddLambdaCoordinateBetweenAnchors()
    {
        var newLambda = new Lambda(_anchor.PrimaryCoordinate, _anchor.SecondaryCoordinate, 0.5f);
        _coordinates.Add(newLambda);
        return newLambda;
    }

    private float DistanceToLambdaCoordiante(float position)
    {
        //todo: quick fix to catch undefined lambda if anchors match
        if (_anchor.AnchorsMatch) return float.PositiveInfinity;

        var lambdaPosition = (_anchor.PrimaryCoordinate.Value + _anchor.SecondaryCoordinate.Value) / 2f;
        return Mathf.Abs(position - lambdaPosition);
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
    private AnchorCoordinates _anchor;
}
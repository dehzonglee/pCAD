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
        _primaryAnchor = _origin;
        _secondaryAnchor = _origin;
    }

    public Coordinate GetCoordiante(float position)
    {
        var closestCoordinate = GetClosestCoordinateInSnapRadius(position);
        if (closestCoordinate != null)
            return closestCoordinate;

        var newCoordinate = AddNewMueCoordiante(position);
        return newCoordinate;
    }

    private Coordinate AddNewMueCoordiante(float position)
    {
        var delta = position - _primaryAnchor.Value;
        var newCoordinate = new Mue(_primaryAnchor, delta);
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
        var newLambda = new Lambda(_primaryAnchor, _secondaryAnchor, 0.5f);
        _coordinates.Add(newLambda);
        return newLambda;
    }

    private float DistanceToLambdaCoordiante(float position)
    {
        //todo: quick fix to catch undefined lambda if anchors match
        if (_primaryAnchor == _secondaryAnchor) return float.PositiveInfinity;

        var lambdaPosition = (_primaryAnchor.Value + _secondaryAnchor.Value) / 2f;
        return Mathf.Abs(position - lambdaPosition);
    }

    public Coordinate GetAnchor()
    {
        return _primaryAnchor;
    }

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


    public Coordinate SetAnchorFromExistingCoordinates(float position)
    {
        _secondaryAnchor = _primaryAnchor;
        _primaryAnchor = FindClosestCoordinate(position);
        if (AnchorChangedEvent != null)
            AnchorChangedEvent();
        return _primaryAnchor;
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
    private Coordinate _primaryAnchor;
    private Coordinate _secondaryAnchor;
}
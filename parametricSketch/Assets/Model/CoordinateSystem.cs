using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem
{
    public Dictionary<int, Axis> Axes { get { return _axes; } }

    public CoordinateSystem()
    {
        _axes = new Dictionary<int, Axis>();
        _axes.Add(Dimensions.X, new Axis());
        _axes.Add(Dimensions.Y, new Axis());
        _axes.Add(Dimensions.Z, new Axis());

        var xAnchorCoordinate = _axes[Dimensions.X].GetAnchor();
        var yAnchorCoordinate = _axes[Dimensions.Y].GetAnchor();
        var zAnchorCoordinate = _axes[Dimensions.Z].GetAnchor();
        _anchorPosition = new ParametricPosition(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
    }

    public ParametricPosition GetParametricPosition(Vector3 position)
    {
        var x = _axes[Dimensions.X].GetCoordiante(position.x);
        var y = _axes[Dimensions.Y].GetCoordiante(position.y);
        var z = _axes[Dimensions.Z].GetCoordiante(position.z);
        return new ParametricPosition(x, y, z);
    }

    public ParametricPosition GetAnchorPosition()
    {
        return _anchorPosition;
    }

    public void SetAnchorPosition(Vector3 position)
    {
        SetAnchorPosition(position.x, Dimensions.X);
        SetAnchorPosition(position.y, Dimensions.Y);
        SetAnchorPosition(position.z, Dimensions.Z);
    }

    private void SetAnchorPosition(float value, int dimension)
    {
        var coordinate = _axes[dimension].SetAnchorFromExistingCoordinates(value);
        _anchorPosition.SetCoordinate(coordinate, dimension);
    }

    private ParametricPosition _anchorPosition;

    private Dictionary<int, Axis> _axes;
}

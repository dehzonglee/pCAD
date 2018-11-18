using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem
{
    public Axis XAxis
    {
        get { return _axes[0]; }
        set { _axes[0] = value; }
    }
    public Axis YAxis
    {
        get { return _axes[1]; }
        set { _axes[1] = value; }
    }
    public Axis ZAxis
    {
        get { return _axes[2]; }
        set { _axes[2] = value; }
    }

    public CoordinateSystem()
    {
        XAxis = new Axis();
        YAxis = new Axis();
        ZAxis = new Axis();

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

    private Axis[] _axes = new Axis[3];
}

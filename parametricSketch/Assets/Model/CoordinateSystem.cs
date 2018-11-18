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

        var xAnchorCoordinate = XAxis.Anchor;
        var yAnchorCoordinate = YAxis.Anchor;
        var zAnchorCoordinate = ZAxis.Anchor;
        _anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
    }

    public ParametricPosition GetParametricPosition(Vector3 position)
    {
        var x = _axes[Dimensions.X].GetCoordiante(position.x);
        var y = _axes[Dimensions.Y].GetCoordiante(position.y);
        var z = _axes[Dimensions.Z].GetCoordiante(position.z);
        return new ParametricPosition(x, y, z);
    }

    public Anchor GetAnchor()
    {
        return _anchor;
    }

    public void SetAnchorPosition(Vector3 position)
    {
        XAxis.SnapAnchorToClosestCoordinate(position.x);
        YAxis.SnapAnchorToClosestCoordinate(position.y);
        ZAxis.SnapAnchorToClosestCoordinate(position.z);
    }

    private Anchor _anchor;

    private Axis[] _axes = new Axis[3];
}

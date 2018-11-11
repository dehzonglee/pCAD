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
    }

    public ParametricPosition GetParametricPosition(Vector3 position)
    {
        var x = _axes[Dimensions.X].GetCoordiante(position.x);
        var y = _axes[Dimensions.Y].GetCoordiante(position.y);
        var z = _axes[Dimensions.Z].GetCoordiante(position.z);
        return new ParametricPosition(x, y, z);
    }

    public void SetAnchor(Vector3 position)
    {
        _axes[Dimensions.X].SetAnchor(position.x);
        _axes[Dimensions.Y].SetAnchor(position.y);
        _axes[Dimensions.Z].SetAnchor(position.z);
    }

    private Dictionary<int, Axis> _axes;
}

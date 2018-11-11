using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem
{
    public CoordinateSystem()
    {
        _axis = new Dictionary<int, Axis>();
        _axis.Add(Dimensions.X, new Axis());
        _axis.Add(Dimensions.Y, new Axis());
        _axis.Add(Dimensions.Z, new Axis());
    }

    public ParametricPosition GetParametricPosition(Vector3 position)
    {
        var x = _axis[Dimensions.X].GetCoordiante(position.x);
        var y = _axis[Dimensions.Y].GetCoordiante(position.y);
        var z = _axis[Dimensions.Z].GetCoordiante(position.z);
        return new ParametricPosition(x, y, z);
    }

    public void SetAnchor(Vector3 position)
    {
        _axis[Dimensions.X].SetAnchor(position.x);
        _axis[Dimensions.Y].SetAnchor(position.y);
        _axis[Dimensions.Z].SetAnchor(position.z);
    }

    private Dictionary<int, Axis> _axis;

}

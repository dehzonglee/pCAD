using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mue : Coordinate
{
    public override float Value => _parent.Value + _mue;
    public Mue(Coordinate parent, float mue)
    {
        _parent = parent;
        _mue = mue;
    }

    private Coordinate _parent;
    private float _mue;
}

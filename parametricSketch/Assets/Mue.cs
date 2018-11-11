using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mue : Coordinate
{
    public override float Value { get { return _parent.Value + Parameter; } }
    public Mue(Coordinate parent, float mue)
    {
        _parent = parent;
        Parameter = mue;
    }

    private Coordinate _parent;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mue : Coordinate
{
    public override float Value { get { return _parent.Value + Parameter; } }

    public override string Name { get { return "Mue"; } }

    public Mue(Coordinate parent, float mue)
    {
        _parent = parent;
        _parent.ValueChangedEvent += () => InvokeValueChanged();
        Parameter = mue;
    }
}

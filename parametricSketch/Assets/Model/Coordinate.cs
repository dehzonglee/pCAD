using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Coordinate
{
    public float ParentValue { get { return _parent.Value; } }
    public event Action ValueChangedEvent;
    public abstract string Name { get; }
    public abstract float Value { get; }
    public bool IsUsed { get { return ValueChangedEvent != null; } }
    public float Parameter
    {
        get { return _parameter; }
        set
        {
            _parameter = value;

            InvokeValueChanged();
        }
    }

    protected void InvokeValueChanged()
    {
        if (ValueChangedEvent != null)
        {
            ValueChangedEvent();
        }
    }

    protected Coordinate _parent;
    private float _parameter;
}

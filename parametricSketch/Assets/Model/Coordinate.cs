using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Coordinate
{
    public abstract float Value { get; }
    private float _parameter;
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
            Debug.LogFormat("Value canges");
            ValueChangedEvent();
        }
    }

    protected Coordinate _parent;
    public float ParentValue { get { return _parent.Value; } }
    public event Action ValueChangedEvent;

}

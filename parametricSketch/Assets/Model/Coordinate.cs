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
            Debug.LogFormat("Set parameter {0} to {1}", _parameter, value);
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
    public event Action ValueChangedEvent;

}

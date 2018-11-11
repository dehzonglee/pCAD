using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lambda : Coordinate
{
    public override float Value
    {
        get
        {
            return (1f - Parameter) * _parent0.Value + Parameter * _parent1.Value;
        }
    }

    public Lambda(Coordinate parent0, Coordinate parent1, float lambda)
    {
        _parent0 = parent0;
        _parent0.ValueChangedEvent += () => InvokeValueChanged();
        _parent1 = parent1;
        _parent1.ValueChangedEvent += () => InvokeValueChanged();
        Parameter = lambda;
    }

    private Coordinate _parent0;
    private Coordinate _parent1;
}
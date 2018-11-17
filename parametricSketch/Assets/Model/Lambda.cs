using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lambda : Coordinate
{
    public override string Name { get { return "Lambda"; } }
    public override float Value
    {
        get
        {
            return (1f - Parameter) * _parent.Value + Parameter * _secondParent.Value;
        }
    }

    public float SecondaryParentValue { get { return _secondParent.Value; } }

    public Lambda(Coordinate parent0, Coordinate parent1, float lambda)
    {
        _parent = parent0;
        _parent.ValueChangedEvent += () => InvokeValueChanged();
        _secondParent = parent1;
        _secondParent.ValueChangedEvent += () => InvokeValueChanged();
        Parameter = lambda;
    }


    private Coordinate _secondParent;
}
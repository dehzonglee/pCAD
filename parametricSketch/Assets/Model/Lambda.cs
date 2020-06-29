using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Lambda : Coordinate
{
    public override string Name => "Lambda";

    public override float Value => (1f - Parameter.Value) * ParentValue + Parameter.Value * SecondaryParentValue;

    public float SecondaryParentValue => Parents[1].Value;

    public Lambda(
        Coordinate parent0,
        Coordinate parent1,
        float lambda,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isPreview
    )
        : base(isPreview, onDeleted, onChanged, new List<Coordinate> {parent0, parent1})
    {
        Parameter = new MueParameter() {Value = lambda};
    }

    public override (float min, float max) GetBounds()
    {
        var min = Mathf.Min(ParentValue, SecondaryParentValue);
        var max = Mathf.Max(ParentValue, SecondaryParentValue);
        return (min, max);
    }
}
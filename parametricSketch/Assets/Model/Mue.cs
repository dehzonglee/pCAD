using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mue : Coordinate
{
    public override float Value => ParentValue + Parameter;

    public override (float min, float max) GetBounds()
    {
        var min = Mathf.Min(ParentValue, Value);
        var max = Mathf.Max(ParentValue, Value);
        return (min, max);
    }

    public override string Name => $"Mue: {Parameter}";

    public Mue(
        Coordinate parent,
        float mue,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isPreview)
        : base(isPreview, onDeleted, onChanged, new List<Coordinate>() {parent})
    {
        Parameter = mue;
    }
}
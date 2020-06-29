using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Mue : Coordinate
{
    public override float Value => ParentValue + Parameter.Value;

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
        var id = GUID.Generate().ToString();
//        Debug.Log($"create paramter with {mue} and id: {id}");

        if (Parameter == null)
            Parameter = new MueParameter(id, mue);
        else
            Parameter.Value = mue;
    }

    public Mue(
        Coordinate parent,
        MueParameter parameterReference,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isPreview)
        : base(isPreview, onDeleted, onChanged, new List<Coordinate>() {parent})
    {
//        Debug.Log($"create paramter with refrence {parameterReference.ID}");

        Parameter = parameterReference;
    }
}
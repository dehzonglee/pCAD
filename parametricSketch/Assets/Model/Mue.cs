using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Mue : Coordinate
{
    public override float Value =>
        PointsInNegativeDirection ? ParentValue - Parameter.Value : ParentValue + Parameter.Value;

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
        bool pointsInNegativeDirection,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isCurrentlyDrawn)
        : base(isCurrentlyDrawn, onDeleted, onChanged, new List<Coordinate>() {parent})
    {
        PointsInNegativeDirection = pointsInNegativeDirection;
        var id = GUID.Generate().ToString();
//        Debug.Log($"create paramter with {mue} and id: {id}");
        if (Parameter == null)
            Parameter = new Parameter(id, mue);
        else
            Parameter.Value = mue;
    }

    public Mue(
        Coordinate parent,
        Parameter parameterReference,
        bool pointsInNegativeDirection,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isCurrentlyDrawn)
        : base(isCurrentlyDrawn, onDeleted, onChanged, new List<Coordinate>() {parent})
    {
//        Debug.Log($"create paramter with refrence {parameterReference.ID}");

        PointsInNegativeDirection = pointsInNegativeDirection;
        Parameter = parameterReference;
    }

    [FormerlySerializedAs("_pointsInNegativeDirection")] public bool PointsInNegativeDirection;
}
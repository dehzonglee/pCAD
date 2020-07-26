using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Mue : Coordinate
{
    public override float Value =>
        PointsInNegativeDirection ? ParentValue - Parameter.Value : ParentValue + Parameter.Value;

    public bool PointsInNegativeDirection;

    public override (float min, float max) GetBounds()
    {
        var min = Mathf.Min(ParentValue, Value);
        var max = Mathf.Max(ParentValue, Value);
        return (min, max);
    }

    public override string Name => $"Mue: {Parameter}";

    [Serializable]
    public new class Serialization : Coordinate.Serialization
    {
        public bool PointsInNegativeDirection;
        public string ParentID;
    }

    public Serialization ToSerializableType(int index)
    {
        return new Serialization
        {
            Index = index, ParentID = Parents[0].ID, ParameterID = Parameter.ID, ID = ID,
            PointsInNegativeDirection = PointsInNegativeDirection
        };
    }


    public Mue(
        float mue,
        bool pointsInNegativeDirection,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isCurrentlyDrawn,
        Coordinate parent)
        : base(isCurrentlyDrawn, onDeleted, onChanged, new List<Coordinate>() {parent})
    {
        PointsInNegativeDirection = pointsInNegativeDirection;
        var id = GUID.Generate().ToString();
        if (Parameter == null)
            Parameter = new Parameter(id, mue);
        else
            Parameter.Value = mue;
    }

    public Mue(
        Parameter parameterReference,
        bool pointsInNegativeDirection,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isCurrentlyDrawn,
        Coordinate parent)
        : base(isCurrentlyDrawn, onDeleted, onChanged, new List<Coordinate>() {parent})
    {
        PointsInNegativeDirection = pointsInNegativeDirection;
        Parameter = parameterReference;
    }


    //used during deserialization, where the parent is set in later step, and coordinate must have a certain id
    public Mue(
        string id,
        Parameter parameterReference,
        bool pointsInNegativeDirection,
        Action<Coordinate> onDeleted,
        Action onChanged,
        bool isCurrentlyDrawn)
        : base(id, isCurrentlyDrawn, onDeleted, onChanged)
    {
        PointsInNegativeDirection = pointsInNegativeDirection;
        Parameter = parameterReference;
    }
}
using System;
using System.Collections.Generic;

public class Lambda : Coordinate
{
    public override string Name => "Lambda";

    public override float Value => (1f - Parameter) * ParentValue + Parameter * SecondaryParentValue;

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
        Parameter = lambda;
    }
}
using System;
using System.Collections.Generic;

public class Mue : Coordinate
{
    public override float Value => ParentValue + Parameter;

    public override string Name => "Mue";

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
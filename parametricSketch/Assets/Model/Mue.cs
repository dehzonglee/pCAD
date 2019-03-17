using System;

public class Mue : Coordinate
{
    public override float Value => _parent.Value + Parameter;

    public override string Name => "Mue";

    public Mue(
        Coordinate parent,
        float mue,
        Action<Coordinate> onCoordinateDeprecated,
        Action onCoordinateChanged,
        bool isPreview)
        : base(isPreview, onCoordinateDeprecated, onCoordinateChanged)
    {
        _parent = parent;
        Parameter = mue;
        _parent.ValueChangedEvent += FireValueChangedEvent;
    }
}
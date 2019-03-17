using System;

public class Lambda : Coordinate
{
    public override string Name => "Lambda";

    public override float Value => (1f - Parameter) * _parent.Value + Parameter * _secondParent.Value;

    public float SecondaryParentValue => _secondParent.Value;

    public Lambda(
        Coordinate parent0,
        Coordinate parent1,
        float lambda,
        Action<Coordinate> onCoordinateDeleted,
        Action onCoordinateChanged,
        bool isPreview
    )
        : base(isPreview, onCoordinateDeleted, onCoordinateChanged)
    {
        _parent = parent0;
        _secondParent = parent1;
        _parent.ValueChangedEvent += FireValueChangedEvent;
        _secondParent.ValueChangedEvent += FireValueChangedEvent;
        Parameter = lambda;
    }

    private readonly Coordinate _secondParent;
}
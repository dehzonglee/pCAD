using System;

public class Lambda : Coordinate
{
    public override string Name => "Lambda";

    public override float Value => (1f - Parameter) * _parent.Value + Parameter * _secondParent.Value;

    public float SecondaryParentValue => _secondParent.Value;

    public Lambda(Coordinate parent0, Coordinate parent1, float lambda, Action<Coordinate> OnCoordinateDeprecated)
    {
        _parent = parent0;
        _parent.ValueChangedEvent += InvokeValueChangedFromChildClass;
        _secondParent = parent1;
        _secondParent.ValueChangedEvent += InvokeValueChangedFromChildClass;
        Parameter = lambda;
        CoordinateDeprecatedEvent += OnCoordinateDeprecated;
    }

    private readonly Coordinate _secondParent;
}
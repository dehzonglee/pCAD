﻿using System;

public class Mue : Coordinate
{
    public override float Value => _parent.Value + Parameter;

    public override string Name => "Mue";

    public Mue(
        Coordinate parent,
        float mue,
        Action<Coordinate> onCoordinateDeprecated,
        Action onCoordinateChangedCallback)
    {
        _parent = parent;
        _parent.ValueChangedEvent += InvokeValueChangedFromChildClass;
        Parameter = mue;
        CoordinateDeprecatedEvent += onCoordinateDeprecated;
        CoordinateChangedEvent += onCoordinateChangedCallback;
    }
}
using System;
using UnityEngine;

public abstract class Coordinate
{
    public float ParentValue => _parent.Value;
    protected event Action<Coordinate> CoordinateDeprecatedEvent;
    public event Action ValueChangedEvent;
    public abstract string Name { get; }
    public abstract float Value { get; }
    public bool IsUsed => ValueChangedEvent != null;

    public float Parameter
    {
        get => _parameter;
        set
        {
            _parameter = value;

            InvokeValueChangedFromChildClass();
        }
    }

    public void Register(Action OnValueChanged)
    {
        ValueChangedEvent += OnValueChanged;
    }

    public void Unregister(Action OnValueChanged)
    {
        Debug.Log($"unregister {this}");
        ValueChangedEvent -= OnValueChanged;
        if (OnValueChanged == null)
            CoordinateDeprecatedEvent?.Invoke(this);
    }

    protected void InvokeValueChangedFromChildClass()
    {
        ValueChangedEvent?.Invoke();
    }

    protected Coordinate _parent;
    private float _parameter;
}
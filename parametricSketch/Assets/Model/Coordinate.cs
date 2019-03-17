using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Coordinate
{
    public abstract string Name { get; }
    public abstract float Value { get; }

    public bool IsPreview { get; private set; }
    public float ParentValue => Parents[0].Value;

    protected event Action ChangedEvent;
    protected event Action<Coordinate> DeletedEvent;

    protected Action FireValueChangedEvent => () => ChangedEvent?.Invoke();

    protected Coordinate(bool isPreview, Action<Coordinate> onDeleted, Action onChanged, List<Coordinate> parents)
    {
        Parents = parents;
        IsPreview = isPreview;
        DeletedEvent += onDeleted;
        ChangedEvent += onChanged;
        foreach (var p in parents)
        {
            p.RegisterCoordinate(this, FireValueChangedEvent);
        }
    }

    public float Parameter
    {
        get => _parameter;
        set
        {
            _parameter = value;
            ChangedEvent?.Invoke();
        }
    }

    public void RegisterCoordinate(Coordinate child, Action onValueChanged)
    {
        _dependentCoordinates.Add(child);
        ChangedEvent += onValueChanged;
    }

    public void UnregisterCoordinate(Coordinate child, Action onValueChanged)
    {
        _dependentCoordinates.Remove(child);
        ChangedEvent -= onValueChanged;
    }

    public void RegisterView(Action onValueChanged)
    {
        ChangedEvent += onValueChanged;
    }

    public void UnregisterView(Action onValueChanged)
    {
        ChangedEvent -= onValueChanged;
        Debug.Log(_dependentCoordinates.Count);
    }

    public void Remove()
    {
        if (_dependentCoordinates.Count != 0) return;
        DeletedEvent?.Invoke(this);
        ChangedEvent?.Invoke();
        foreach (var p in Parents)
        {
            p.UnregisterCoordinate(this, ChangedEvent);
        }
    }

    public void Bake()
    {
        IsPreview = false;
    }

    private readonly List<Coordinate> _dependentCoordinates = new List<Coordinate>();
    protected List<Coordinate> Parents;
    private float _parameter;
}
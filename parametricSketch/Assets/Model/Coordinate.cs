﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Coordinate
{
    public abstract string Name { get; }
    public abstract float Value { get; }

    public bool IsCurrentlyDrawn { get; private set; }
    public float ParentValue => Parents[0].Value;

    protected event Action ChangedEvent;
    protected event Action<Coordinate> DeletedEvent;
    public List<Coordinate> Parents;

    protected Action FireValueChangedEvent => () => ChangedEvent?.Invoke();

    protected Coordinate(bool isCurrentlyDrawn, Action<Coordinate> onDeleted, Action onChanged, List<Coordinate> parents)
    {
        Parents = parents;
        IsCurrentlyDrawn = isCurrentlyDrawn;
        DeletedEvent += onDeleted;
        ChangedEvent += onChanged;
        foreach (var p in parents)
        {
            p.RegisterCoordinate(this, FireValueChangedEvent);
        }
    }

    public Parameter Parameter
    {
        get => _parameter;
        set
        {
            _parameter = value;
            ChangedEvent?.Invoke();
        }
    }

    public abstract (float min, float max) GetBounds();
    
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

    public void UnregisterGeometryAndTryToDelete(Sketch.RectangleModel rectangleToUnregister)
    {
        _attachedGeometry.Remove(rectangleToUnregister);
        if(_attachedGeometry.Count==0)
            Delete();
    }
    
    public void Delete()
    {
        if (_dependentCoordinates.Count != 0) return;
        DeletedEvent?.Invoke(this);
//        ChangedEvent?.Invoke();
        foreach (var p in Parents)
        {
            p.UnregisterCoordinate(this, ChangedEvent);
        }
    }

    public void Bake()
    {
        IsCurrentlyDrawn = false;
    }

    public void AddAttachedGeometry(Sketch.GeometryModel rectangle)
    {
        _attachedGeometry.Add(rectangle);
    }

    public static List<Coordinate> GetPathToOrigin(Coordinate coordinate)
    {
        var pathWithDuplicates = GetPathRecursion(coordinate);
        var cleanPath = new List<Coordinate>();
        foreach (var c in pathWithDuplicates)
        {
            if (!cleanPath.Contains(c))
                cleanPath.Add(c);
        }

        return cleanPath;
        
        List<Coordinate> GetPathRecursion(Coordinate c)
        {
            var path = new List<Coordinate>(){c};
            foreach (var parent in c.Parents)
            {
                path.AddRange(GetPathToOrigin(parent));
            }

            return path;
        }
    }

    
    private readonly List<Coordinate> _dependentCoordinates = new List<Coordinate>();
    private readonly List<Sketch.GeometryModel> _attachedGeometry = new List<Sketch.GeometryModel>();
    private Parameter _parameter;
}
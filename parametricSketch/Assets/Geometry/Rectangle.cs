using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Model;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rectangle : MonoBehaviour
{
    public void Initialize()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    public void SetFirstPosition(ParametricPosition p0)
    {
        _p0 = p0;
        _p0.PositionChangedEvent += UpdateLine;
        _p0.AddDependentGeometry(this);
    }

    public void SetSecondPosition(ParametricPosition p1)
    {
        if (_p1 != null)
        {
            _p1.PositionChangedEvent -= UpdateLine;
            _p1.RemoveDependentGeometry(this);
        }

        _p1 = p1;
        _p1.PositionChangedEvent += UpdateLine;
        _p1.AddDependentGeometry(this);

        _lineRenderer.enabled = true;
        UpdateLine(_p1, _p1.X);
    }


    private void UpdateLine(ParametricPosition changedPosition, Coordinate changedCoordinate)
    {
        Debug.Log($"changed {changedCoordinate.Parameter} of {changedPosition.Value}");
        // generate the four corners of the rectangle from p0 and p1
        var positionValues = new[]
        {
            _p0.Value, new ParametricPosition(_p0.X, _p0.Y, _p1.Z).Value, _p1.Value,
            new ParametricPosition(_p1.X, _p0.Y, _p0.Z).Value
        };
        _lineRenderer.SetPositions(positionValues);
    }

    private ParametricPosition[] _corners = new ParametricPosition[2];
    private LineRenderer _lineRenderer;

    private ParametricPosition _p0;
    private ParametricPosition _p1;

    // expects that the first position is set but the second isnt
    public void Abort()
    {
        _p0.RemoveDependentGeometry(this);
    }
}
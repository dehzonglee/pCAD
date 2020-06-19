using System.Collections;
using System.Collections.Generic;
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
        p0.PositionChangedEvent += UpdateLine;
        p0.AddDependentGeometry(this);
        _p0 = p0;
    }

    public void SetSecondPosition(ParametricPosition p1)
    {
        _positions[0] = _p0;
        _positions[1] = new ParametricPosition(_p0.X, _p0.Y, p1.Z);
        _positions[2] = p1;
        _positions[3] = new ParametricPosition(p1.X, _p0.Y, _p0.Z);

        _positions[1].PositionChangedEvent += UpdateLine;
        _positions[2].PositionChangedEvent += UpdateLine;
        _positions[3].PositionChangedEvent += UpdateLine;
        _lineRenderer.enabled = true;
        UpdateLine();
    }

    private void UpdateLine()
    {
        var positionValues = new Vector3[4];
        for (int i = 0; i < _positions.Length; i++)
        {
            positionValues[i] = _positions[i].Value;
        }
        _lineRenderer.SetPositions(positionValues);
    }

    private ParametricPosition[] _positions = new ParametricPosition[4];
    private LineRenderer _lineRenderer;

    private ParametricPosition _p0;
    
    // expects that the first position is set but the second isnt
    public void Abort()
    {
        _p0.RemoveDependentGeometry(this);
    }
}

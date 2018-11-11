using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rectangle : MonoBehaviour
{
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetFirstPosition(ParametricPosition p0)
    {
        p0.PositionChangedEvent += UpdateLine;
        _positions[0] = p0;
    }

    public void SetSecondPosition(ParametricPosition p1)
    {
        var p0 = _positions[0];
        _positions[1] = new ParametricPosition(p0.X, p0.Y, p1.Z);
        _positions[2] = p1;
        _positions[3] = new ParametricPosition(p1.X, p0.Y, p0.Z);

        _positions[1].PositionChangedEvent += UpdateLine;
        _positions[2].PositionChangedEvent += UpdateLine;
        _positions[3].PositionChangedEvent += UpdateLine;
        UpdateLine();
    }

    private void UpdateLine()
    {
        Debug.Log("UpdateLine");
        var positionValues = new Vector3[4];
        for (int i = 0; i < _positions.Length; i++)
        {
            positionValues[i] = _positions[i].Value;
        }
        _lineRenderer.SetPositions(positionValues);
    }

    private ParametricPosition[] _positions = new ParametricPosition[4];
    private LineRenderer _lineRenderer;
}

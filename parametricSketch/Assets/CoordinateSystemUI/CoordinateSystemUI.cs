using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystemUI : MonoBehaviour
{
    [SerializeField]
    AxisUI _axisUIPrefab;

    [SerializeField]
    AnchorUI _anchorUIPrefab;

    private Dictionary<int, AxisUI> _axisUIs = new Dictionary<int, AxisUI>();

    public void Initialize(CoordinateSystem cs, Action<Axis, Coordinate, float> modelChangeRequest)
    {
        var xUI = Instantiate(_axisUIPrefab);
        var yUI = Instantiate(_axisUIPrefab);
        var zUI = Instantiate(_axisUIPrefab);
        xUI.Initialize(cs.Axes[Dimensions.X], modelChangeRequest);
        yUI.Initialize(cs.Axes[Dimensions.Y], modelChangeRequest);
        zUI.Initialize(cs.Axes[Dimensions.Z], modelChangeRequest);
        _axisUIs.Add(Dimensions.X, xUI);
        _axisUIs.Add(Dimensions.Y, yUI);
        _axisUIs.Add(Dimensions.Z, zUI);

        var anchorUI = Instantiate(_anchorUIPrefab);
        anchorUI.Initalize(cs.GetAnchorPosition());
    }

    public void UpdateUI()
    {
        _axisUIs[Dimensions.X].UpdateCoordinateUIs(Vector3.right, Vector3.forward);
        _axisUIs[Dimensions.Y].UpdateCoordinateUIs(Vector3.up, Vector3.up);
        _axisUIs[Dimensions.Z].UpdateCoordinateUIs(Vector3.forward, Vector3.right);

    }
}

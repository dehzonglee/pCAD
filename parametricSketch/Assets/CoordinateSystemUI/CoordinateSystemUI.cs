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
        _coordinateSystem = cs;

        var xUI = Instantiate(_axisUIPrefab);
        var yUI = Instantiate(_axisUIPrefab);
        var zUI = Instantiate(_axisUIPrefab);
        xUI.Initialize(cs.Axes[Dimensions.X], modelChangeRequest, Vector3.right);
        yUI.Initialize(cs.Axes[Dimensions.Y], modelChangeRequest, Vector3.up);
        zUI.Initialize(cs.Axes[Dimensions.Z], modelChangeRequest, Vector3.forward);
        _axisUIs.Add(Dimensions.X, xUI);
        _axisUIs.Add(Dimensions.Y, yUI);
        _axisUIs.Add(Dimensions.Z, zUI);

        var anchorUI = Instantiate(_anchorUIPrefab);
        anchorUI.Initalize(cs.GetAnchorPosition());
    }

    public void UpdateUI()
    {
        _axisUIs[Dimensions.X].UpdateCoordinateUIs(Vector3.forward, GetOrthogonalAxis(Dimensions.X).SmallestValue);
        _axisUIs[Dimensions.Y].UpdateCoordinateUIs(Vector3.up, GetOrthogonalAxis(Dimensions.Y).SmallestValue);
        _axisUIs[Dimensions.Z].UpdateCoordinateUIs(Vector3.right, GetOrthogonalAxis(Dimensions.Z).SmallestValue);

    }

    private Axis GetOrthogonalAxis(int dimension)
    {
        switch (dimension)
        {
            case Dimensions.X:
                return _coordinateSystem.Axes[Dimensions.Z];
            case Dimensions.Y:
                return _coordinateSystem.Axes[Dimensions.Y];
            case Dimensions.Z:
            default:
                return _coordinateSystem.Axes[Dimensions.X];
        }
    }

    private CoordinateSystem _coordinateSystem;
}

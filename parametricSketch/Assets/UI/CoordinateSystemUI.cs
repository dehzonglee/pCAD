using System;
using Model;
using UI;
using UnityEngine;

public class CoordinateSystemUI : MonoBehaviour
{
    [SerializeField] AxisUI _axisUIPrefab;
    [SerializeField] AnchorUI _anchorUIPrefab;
    
    public void Initialize(CoordinateSystem cs, Action<Axis, Coordinate, float> modelChangeRequest)
    {
        _coordinateSystem = cs;
        cs.CoordinateSystemChangedEvent += UpdateUI;

        _xAxisUI = Instantiate(_axisUIPrefab, transform);
        _xAxisUI.Initialize(cs.XAxis, modelChangeRequest, Vector3.right, "xAxisUI");

        _yAxisUI = Instantiate(_axisUIPrefab, transform);
        _yAxisUI.Initialize(cs.YAxis, modelChangeRequest, Vector3.up, "yAxisUI");

        _zAxisUI = Instantiate(_axisUIPrefab, transform);
        _zAxisUI.Initialize(cs.ZAxis, modelChangeRequest, Vector3.forward, "zAxisUI");

        _anchorUI = Instantiate(_anchorUIPrefab, transform);
        _anchorUI.Initalize(cs.GetAnchor());
    }

    public void UpdateUI()
    {
        _xAxisUI.UpdateCoordinateUIs(Vector3.forward, GetOrthogonalAxis(Dimensions.X).SmallestValue);
        _yAxisUI.UpdateCoordinateUIs(Vector3.up, GetOrthogonalAxis(Dimensions.Y).SmallestValue);
        _zAxisUI.UpdateCoordinateUIs(Vector3.right, GetOrthogonalAxis(Dimensions.Z).SmallestValue);
        _anchorUI.UpdateUI();
    }

    private Axis GetOrthogonalAxis(int dimension)
    {
        switch (dimension)
        {
            case Dimensions.X:
                return _coordinateSystem.ZAxis;
            case Dimensions.Y:
                return _coordinateSystem.YAxis;
            case Dimensions.Z:
            default:
                return _coordinateSystem.XAxis;
        }
    }

    private AxisUI _xAxisUI
    {
        get => _axisUIs[0];
        set => _axisUIs[0] = value;
    }

    private AxisUI _yAxisUI
    {
        get => _axisUIs[1];
        set => _axisUIs[1] = value;
    }

    private AxisUI _zAxisUI
    {
        get => _axisUIs[2];
        set => _axisUIs[2] = value;
    }

    private AnchorUI _anchorUI;
    private AxisUI[] _axisUIs = new AxisUI[3];
    private CoordinateSystem _coordinateSystem;
}
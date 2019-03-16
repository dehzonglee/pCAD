using System;
using Model;
using UnityEngine;

public class CoordinateSystemUI : MonoBehaviour
{
    [SerializeField]
    AxisUI _axisUIPrefab;

    [SerializeField]
    AnchorUI _anchorUIPrefab;


    public void Initialize(CoordinateSystem cs, Action<Axis, Coordinate, float> modelChangeRequest)
    {
        _coordinateSystem = cs;

        _xAxisUI = Instantiate(_axisUIPrefab);
        _xAxisUI.Initialize(cs.XAxis, modelChangeRequest, Vector3.right);

        _yAxisUI = Instantiate(_axisUIPrefab);
        _yAxisUI.Initialize(cs.YAxis, modelChangeRequest, Vector3.up);

        _zAxisUI = Instantiate(_axisUIPrefab);
        _zAxisUI.Initialize(cs.ZAxis, modelChangeRequest, Vector3.forward);

        _anchorUI = Instantiate(_anchorUIPrefab);
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
        get { return _axisUIs[0]; }
        set { _axisUIs[0] = value; }
    }

    private AxisUI _yAxisUI
    {
        get { return _axisUIs[1]; }
        set { _axisUIs[1] = value; }
    }

    private AxisUI _zAxisUI
    {
        get { return _axisUIs[2]; }
        set { _axisUIs[2] = value; }
    }
    private AnchorUI _anchorUI;
    private AxisUI[] _axisUIs = new AxisUI[3];
    private CoordinateSystem _coordinateSystem;
}

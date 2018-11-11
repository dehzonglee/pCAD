using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystemUI : MonoBehaviour
{
    [SerializeField]
    AnchorUI _anchorUIPrefab;

    [SerializeField]
    CoordinateUI _coordinateUIPrefab;

    [SerializeField]
    Transform _xUIContainer;

    [SerializeField]
    Transform _yUIContainer;

    [SerializeField]
    Transform _zUIContainer;


    public void Initialize(CoordinateSystem cs, Action<Axis, Coordinate, float> modelChangeRequest)
    {
        _modelChangeRequest = modelChangeRequest;
        _coordinateSystem = cs;

        var anchorUI = Instantiate(_anchorUIPrefab);
        anchorUI.Initalize(_coordinateSystem.GetAnchorPosition());
    }

    void Update()
    {
        if (_coordinateSystem == null)
            return;

        RenderAxis(_xUIContainer, _coordinateSystem.Axes[Dimensions.X], Vector3.right);
        RenderAxis(_yUIContainer, _coordinateSystem.Axes[Dimensions.Y], Vector3.up);
        RenderAxis(_zUIContainer, _coordinateSystem.Axes[Dimensions.Z], Vector3.forward);

    }

    private void RenderAxis(Transform container, Axis axis, Vector3 direction)
    {
        for (int i = 0; i < axis.Coordinates.Count; i++)
        {
            var c = axis.Coordinates[i];
            if (!_ui.ContainsKey(c))
            {
                var ui = Instantiate(_coordinateUIPrefab, container);
                ui.Initalize(c, direction, i, (coordinate, parameter) => _modelChangeRequest(axis, coordinate, parameter));
                _ui.Add(c, ui);
            }
        }
    }


    private Action<Axis, Coordinate, float> _modelChangeRequest;
    private CoordinateSystem _coordinateSystem;
    private Dictionary<Coordinate, CoordinateUI> _ui = new Dictionary<Coordinate, CoordinateUI>();
}

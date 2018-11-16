﻿using System;
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

        var xAxis = _coordinateSystem.Axes[Dimensions.X];
        var yAxis = _coordinateSystem.Axes[Dimensions.Y];
        var zAxis = _coordinateSystem.Axes[Dimensions.Z];

        AddNewCoordinateUIs(_xUIContainer, xAxis, Vector3.right, zAxis.SmallestValue);
        AddNewCoordinateUIs(_yUIContainer, yAxis, Vector3.up, 0f);
        AddNewCoordinateUIs(_zUIContainer, zAxis, Vector3.forward, xAxis.SmallestValue);

    }

    private void AddNewCoordinateUIs(Transform container, Axis axis, Vector3 direction, float orthogonalAnchor)
    {
        for (int i = 0; i < axis.Coordinates.Count; i++)
        {
            var c = axis.Coordinates[i];

            var layoutInfo = new CoordinateUI.LayoutInfo()
            {
                Direction = direction,
                Index = i,
                OrthogonalAnchor = orthogonalAnchor,
            };

            if (!_ui.ContainsKey(c))
            {
                var ui = Instantiate(_coordinateUIPrefab, container);
                ui.Initalize(c, (coordinate, parameter) => _modelChangeRequest(axis, coordinate, parameter));
                _ui.Add(c, ui);
            }

            _ui[c].UpdateUI(layoutInfo);
        }
    }

    private Action<Axis, Coordinate, float> _modelChangeRequest;
    private CoordinateSystem _coordinateSystem;
    private Dictionary<Coordinate, CoordinateUI> _ui = new Dictionary<Coordinate, CoordinateUI>();
}

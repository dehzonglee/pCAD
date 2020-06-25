﻿using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using TMPro;
using UnityEngine;

public class LambdaUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _parent0GizmoUI = null;
    [SerializeField] protected CoordinateGizmoUI _parent1GizmoUI = null;
    [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI = null;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI = null;
    [SerializeField] protected CoordinateLabelUI _coordinateLabelUI = null;

    public void UpdateUI(Lambda coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding,
        CoordinateUIStyle.LambdaUIStyle style)
    {
        var state = coordinate.IsPreview ? SketchStyle.State.Focus : SketchStyle.State.Default;
        //todo: set style in initialize method
        _coordinate = coordinate;
        var labelString = "1 / 2"; // coordinate.Parameter.ToString("F");
        gameObject.name = $"Mue2D:{labelString}";

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;

        var parent0CoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var parent1CoordinateUIPositionWorld = direction * coordinate.SecondaryParentValue + offset;
        var labelPosition = coordinateUIPositionWorld; 
        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection, style.GridLineStyle, state);
        _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld, style.CoordinateGizmoStyle, state);
        _parent0GizmoUI.UpdateUI(parent0CoordinateUIPositionWorld, style.CoordinateGizmoStyle, state);
        _parent1GizmoUI.UpdateUI(parent1CoordinateUIPositionWorld, style.CoordinateGizmoStyle, state);
        _coordinateDimensionLineUI.UpdateUI(parent0CoordinateUIPositionWorld, parent1CoordinateUIPositionWorld,
            style.DimensionLineStyle, state);
        _coordinateLabelUI.UpdateUI(labelString, labelPosition, style.LabelStyle, state);
    }

    public CoordinateManipulation.ScreenDistance GetScreenDistanceToCoordinate(Vector2 screenPos)
    {
        var distance = _gridLineUI.GetScreenDistanceToLine(screenPos);
        Debug.Log($"distance to {_coordinate.Parameter} is {distance}");
        return new CoordinateManipulation.ScreenDistance()
            {Coordinate = _coordinate, ScreenDistanceToCoordinate = distance};
    }

    private Lambda _coordinate;
}
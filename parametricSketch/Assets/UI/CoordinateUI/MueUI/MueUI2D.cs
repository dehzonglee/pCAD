using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MueUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI = null;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI = null;
    [SerializeField] protected CoordinateLabelUI _coordinateLabelUI = null;

    public void UpdateUI(Mue coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding,
        CoordinateUIStyle.MueStyleSet styleSet)
    {
        var style = coordinate.IsPreview ? styleSet.Focus : styleSet.Default;
        _coordinate = coordinate;
        var labelString = coordinate.Parameter.ToString("F");
        gameObject.name = $"Mue2D:{labelString}";

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;

        var parentCoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var labelOffset = padding * 0.5f * layoutInfo.OrthogonalDirection;
        var labelPosition = (coordinateUIPositionWorld + parentCoordinateUIPositionWorld) * 0.5f + labelOffset;
        
        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection, style.GridLineStyle);
        _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld,style.CoordinateGizmoStyle);
        _coordinateDimensionLineUI.UpdateUI(coordinateUIPositionWorld, parentCoordinateUIPositionWorld,
            coordinate.IsPreview, style.DimensionLineStyle);
        _coordinateLabelUI.UpdateUI(labelString, labelPosition, style.LabelStyle);
    }

    public CoordinateManipulation.ScreenDistance GetScreenDistanceToCoordinate(Vector2 screenPos)
    {
        var distance = _gridLineUI.GetScreenDistanceToLine(screenPos);
        Debug.Log($"distance to {_coordinate.Parameter} is {distance}");
        return new CoordinateManipulation.ScreenDistance()
            {Coordinate = _coordinate, ScreenDistanceToCoordinate = distance};
        
    }

    private Mue _coordinate;
}
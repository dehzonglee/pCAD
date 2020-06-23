using System;
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

    public void UpdateUI(Lambda coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding)
    {
        _coordinate = coordinate;
        var labelString = coordinate.Parameter.ToString("F");
        gameObject.name = $"Mue2D:{labelString}";

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;

//        var parentCoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var primaryParentCoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var secondaryParentCoordinateUIPositionWorld = direction * coordinate.SecondaryParentValue + offset;
        var labelOffset = padding * 0.5f * layoutInfo.OrthogonalDirection;
        var labelPosition = coordinateUIPositionWorld + labelOffset;

        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection);
        
        _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld);
        _parent0GizmoUI.UpdateUI(primaryParentCoordinateUIPositionWorld);
        _parent1GizmoUI.UpdateUI(secondaryParentCoordinateUIPositionWorld);
        
        _coordinateDimensionLineUI.UpdateUI(
            primaryParentCoordinateUIPositionWorld,
            secondaryParentCoordinateUIPositionWorld,
            coordinate.IsPreview);
        _coordinateLabelUI.UpdateUI(labelString, labelPosition);
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
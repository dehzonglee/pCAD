using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MueUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI = null;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI = null;
    [SerializeField] protected CoordinateLabelUI _coordinateLabelUI = null;

    public void UpdateUI(Mue coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction,float padding)
    {
        var labelString = coordinate.Parameter.ToString("F");
        gameObject.name = $"Mue2D:{labelString}";
        
        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;

        var parentCoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var labelOffset = padding * 0.5f * layoutInfo.OrthogonalDirection;
        var labelPosition = (coordinateUIPositionWorld + parentCoordinateUIPositionWorld) * 0.5f + labelOffset;

        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection);
        _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld);
        _coordinateDimensionLineUI.UpdateUI(coordinateUIPositionWorld, parentCoordinateUIPositionWorld,coordinate.IsPreview );
        _coordinateLabelUI.UpdateUI(labelString,labelPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MueUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI;
    [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI;

    public void UpdateUI(Coordinate coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction,float padding)
    {
        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPosition = direction * coordinate.Value + offset;

        var mue = coordinate as Mue;
        var parentCoordinateUIPosition = direction * mue.ParentValue + offset;
        
        _gridLineUI.UpdateUI(coordinateUIPosition, layoutInfo.OrthogonalDirection);
        _coordinateGizmoUI.UpdateUI(coordinateUIPosition);
        _coordinateDimensionLineUI.UpdateUI(coordinateUIPosition, parentCoordinateUIPosition,coordinate.IsPreview );
    }

}

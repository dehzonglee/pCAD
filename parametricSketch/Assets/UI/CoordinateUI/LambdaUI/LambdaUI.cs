using System;
using Model;
using UnityEngine;

public class LambdaUI : CoordinateUI
{
    public override void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo, Vector3 direction, float padding)
    {
        Coordinate = coordinate;
        
        var lambda = Coordinate as Lambda;
        _label.text = "1/2";
        _label.gameObject.SetActive(false);
        _uiExposedParameter = Coordinate.Parameter;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);

        var coordinateUIPosition = direction * Coordinate.Value + offset;
        transform.position = coordinateUIPosition;

        var labelOffset = layoutInfo.OrthogonalDirection * 0.5f * padding;

        var primaryParentCoordinateUIPosition = direction * lambda.ParentValue + offset;
        var secondaryParentCoordinateUIPosition = direction * lambda.SecondaryParentValue + offset;
        _label.transform.position = coordinateUIPosition + labelOffset;
        _line.SetPosition(0, primaryParentCoordinateUIPosition);
        _line.SetPosition(1, secondaryParentCoordinateUIPosition);
        
        UpdateBase();
    }
}
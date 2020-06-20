using System;
using Model;
using UnityEngine;

public class LambdaUI : CoordinateUI
{
    public override void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo)
    {
        Coordinate = coordinate;
        
        var lambda = Coordinate as Lambda;
        _label.text = "1/2";
        _label.gameObject.SetActive(false);
        _uiExposedParameter = Coordinate.Parameter;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * _padding);

        var coordinateUIPosition = _direction * Coordinate.Value + offset;
        transform.position = coordinateUIPosition;

        var labelOffset = layoutInfo.OrthogonalDirection * 0.5f * _padding;

        var primaryParentCoordinateUIPosition = _direction * lambda.ParentValue + offset;
        var secondaryParentCoordinateUIPosition = _direction * lambda.SecondaryParentValue + offset;
        _label.transform.position = coordinateUIPosition + labelOffset;
        _line.SetPosition(0, primaryParentCoordinateUIPosition);
        _line.SetPosition(1, secondaryParentCoordinateUIPosition);

        _gridLine.positionCount = 2;
        _gridLine.useWorldSpace = true;
        _gridLine.SetPosition(0, coordinateUIPosition + layoutInfo.OrthogonalDirection * 100f);
        _gridLine.SetPosition(1, coordinateUIPosition + layoutInfo.OrthogonalDirection * -100f);
        
        
        UpdateBase();
    }
}
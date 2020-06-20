using System;
using Model;
using UnityEngine;

public class MueUI : CoordinateUI
{
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _defaultColor;
    public override void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo)
    {
        Coordinate = coordinate;
        var label = Coordinate.Parameter.ToString("F");
        _label.text = label;
        gameObject.name = $"Mue:{label}";
        _uiExposedParameter = Coordinate.Parameter;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * _padding);
        var coordinateUIPosition = _direction * Coordinate.Value + offset;
        transform.position = coordinateUIPosition;

        var labelOffset = _padding * 0.5f * layoutInfo.OrthogonalDirection;

        var mue = Coordinate as Mue;
        var parentCoordinateUIPosition = _direction * mue.ParentValue + offset;
        var labelPosition = (coordinateUIPosition + parentCoordinateUIPosition) * 0.5f + labelOffset;
        Debug.Log($"{mue.Value}, {labelPosition}, {Time.frameCount}");
        _label.transform.position = labelPosition;
        _line.SetPosition(0, coordinateUIPosition);
        _line.SetPosition(1, parentCoordinateUIPosition);

        _line.material.SetColor("_Color", coordinate.IsPreview ? _activeColor : _defaultColor);  
        
        _gridLine.positionCount = 2;
        _gridLine.useWorldSpace = true;
        _gridLine.SetPosition(0, coordinateUIPosition + layoutInfo.OrthogonalDirection * 100f);
        _gridLine.SetPosition(1, coordinateUIPosition + layoutInfo.OrthogonalDirection * -100f);

        UpdateBase();
    }
}
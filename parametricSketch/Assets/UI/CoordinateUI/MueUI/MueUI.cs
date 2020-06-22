using System;
using Model;
using UnityEngine;

public class MueUI : CoordinateUI
{
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _defaultColor;
    public override void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo, Vector3 direction, float padding)
    {
        Coordinate = coordinate;
        var label = Coordinate.Parameter.ToString("F");
        _label.text = label;
        gameObject.name = $"Mue:{label}";
        _uiExposedParameter = Coordinate.Parameter;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPosition = direction * Coordinate.Value + offset;
        transform.position = coordinateUIPosition;

        var labelOffset = padding * 0.5f * layoutInfo.OrthogonalDirection;

        var mue = Coordinate as Mue;
        var parentCoordinateUIPosition = direction * mue.ParentValue + offset;
        var labelPosition = (coordinateUIPosition + parentCoordinateUIPosition) * 0.5f + labelOffset;
        _label.transform.position = labelPosition;
        _line.SetPosition(0, coordinateUIPosition);
        _line.SetPosition(1, parentCoordinateUIPosition);

        _line.material.SetColor("_Color", coordinate.IsPreview ? _activeColor : _defaultColor);  
        
        UpdateBase();
    }
}
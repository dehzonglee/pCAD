using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordinateDimensionLineUI : MaskableGraphic
{
    public void UpdateUI(Vector3 startWorld, Vector3 endWorld, bool renderAsPreview,
        CoordinateUIStyle.DimensionLineStyle style)
    {
        _startWorld = startWorld;
        _endWorld = endWorld;
        _style = style;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddLine(vh, _startWorld, _endWorld - _startWorld, _style.Width, _style.Color,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private CoordinateUIStyle.DimensionLineStyle _style;
    private Vector3 _startWorld;
    private Vector3 _endWorld;
}
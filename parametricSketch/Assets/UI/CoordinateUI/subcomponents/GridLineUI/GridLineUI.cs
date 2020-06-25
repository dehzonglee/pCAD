using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineUI : MaskableGraphic
{
    public void UpdateUI(Vector3 originWorld, Vector3 directionWorld,
        CoordinateUIStyle.GridLineStyle lambdaStyleGridLineStyle, SketchStyle.State state)
    {
        _originScreen =WorldScreenTransformationHelper.WorldToScreenPoint(originWorld);
        _directionScreen =WorldScreenTransformationHelper.WorldToScreenPoint( directionWorld);
        _style = lambdaStyleGridLineStyle;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // quick fix: assume that rectangle is projected on the xz plane
        vh.Clear();
        UIMeshGenerationHelper.AddScreenSpanningLine(vh, _originScreen, _directionScreen, _style.Width,_style.Color.GetForState(_state));
    }

    public float GetScreenDistanceToLine(Vector2 screenPosition)
    {
        //https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
        var o = _originScreen;
        var p = screenPosition;
        var v = _directionScreen.normalized;
        var normalToPoint = (o - p) - Vector2.Dot((o - p), v) * v;
        
        
        return normalToPoint.magnitude;
    }

    private CoordinateUIStyle.GridLineStyle _style;
    private SketchStyle.State _state;
    private Vector2 _originScreen;
    private Vector2 _directionScreen;
}
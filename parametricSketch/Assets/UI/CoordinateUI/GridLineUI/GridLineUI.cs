﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineUI : MaskableGraphic
{
    [SerializeField] private float width = 1f;

    public void UpdateUI(Vector3 originWorld, Vector3 directionWorld)
    {
        _originScreen =WorldScreenTransformationHelper.WorldToScreenPoint(originWorld);
        _directionScreen =WorldScreenTransformationHelper.WorldToScreenPoint( directionWorld);
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // quick fix: assume that rectangle is projected on the xz plane
        vh.Clear();
        UIMeshGenerationHelper.AddScreenSpanningLine(vh, _originScreen, _directionScreen, width, base.color);
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
    
    private Vector2 _originScreen;
    private Vector2 _directionScreen;
}
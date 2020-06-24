﻿using System;
using UnityEngine;

[CreateAssetMenu(menuName = "paraSketch/GeometryStyle")]
public class GeometryStyle : ScriptableObject
{
    public RectangleStyleSet Rectangle;

    [Serializable]
    public class RectangleStyleSet : SketchStyle.StyleSet<RectangleStyle>
    {
    }

    [Serializable]
    public class RectangleStyle
    {
        public Color OutlineColor = Color.black;
        public Color FillColor = Color.white;
        public float OutlineWidth = 0.5f;
    }
}
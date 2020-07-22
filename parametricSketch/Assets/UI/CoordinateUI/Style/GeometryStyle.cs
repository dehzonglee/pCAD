using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "paraSketch/GeometryStyle")]
public class GeometryStyle : ScriptableObject
{
    public RectangleStyleSet Rectangle;
    public PointStyleSet Points;
    public LineStyleSet Lines;

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


    [Serializable]
    public class LineStyleSet : SketchStyle.StyleSet<LineStyle>
    {
    }

    [Serializable]
    public class PointStyleSet : SketchStyle.StyleSet<PointStyle>
    {
    }

    [Serializable]
    public class PointStyle
    {
        public Color OutlineColor = Color.black;
        public Color FillColor = Color.white;
        public float OutlineWidth = 0.5f;
    }

    [Serializable]
    public class LineStyle
    {
        public Color OutlineColor = Color.black;
        public Color FillColor = Color.white;
        public float OutlineWidth = 0.5f;
    }
}
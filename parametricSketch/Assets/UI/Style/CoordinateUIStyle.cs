using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "paraSketch/CoordinateUIStyle")]
public class CoordinateUIStyle : ScriptableObject
{
    public LambdaUIStyle Lambda;
    public MueUIStyle Mue;
    public OriginUIStyle Origin;
    public AnchorStyle Anchor;

    [Serializable]
    public class OriginUIStyle
    {
        public GridLineStyle GridLineStyle;
        public CoordinateGizmoStyle CoordinateGizmoStyle;
        public ColorSet Colors ;
    }

    [Serializable]
    public class MueUIStyle
    {
        public ColorSet Colors;
        public GridLineStyle GridLineStyle;
        public CoordinateGizmoStyle CoordinateGizmoStyle;
        public DimensionLineStyle DimensionLineStyle;
        public LabelStyle LabelStyle;
    }

    [Serializable]
    public class LambdaUIStyle
    {
        public ColorSet Colors;
        public GridLineStyle GridLineStyle;
        public CoordinateGizmoStyle CoordinateGizmoStyle;
        public DimensionLineStyle DimensionLineStyle;
        public LabelStyle LabelStyle;
    }

    [Serializable]
    public class AnchorStyle
    {
        public CircleStyle CircleStyle;
    }

    [Serializable]
    public class CoordinateGizmoStyle
    {
        public float ArrowAngle = 30f;
        public Vector2 MarkDimensions = new Vector2(1f, 2f);
        public float Width = 0.5f;
    }

    [Serializable]
    public class GridLineStyle
    {
        public ColorAsset Color;
        public float Width = 0.5f;
    }

    [Serializable]
    public class DimensionLineStyle
    {
        public float Width = 0.5f;
    }

    [Serializable]
    public class LabelStyle
    {
        public float FontSize = 10f;
    }

    [Serializable]
    public class CircleStyle
    {
        public ColorAsset PrimaryColor;
        public ColorAsset SecondaryColor;
        public float Radius = 10f;
        public float InnerRadius = 10f;
        public float Width = 2f;
    }

    [Serializable]
    public class ColorSet : SketchStyle.StyleSet<ColorAsset>
    {
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "paraSketch/CoordinateUIStyle")]
public class CoordinateUIStyle : ScriptableObject
{
    [FormerlySerializedAs("LambdaStyle")] public LambdaUIStyle Lambda;
    [FormerlySerializedAs("MueStyle")] public MueUIStyle Mue;
    [FormerlySerializedAs("OriginStyle")] public OriginUIStyle Origin;
    [FormerlySerializedAs("AnchorStyle")] public AnchorStyle Anchor;

    [Serializable]
    public class OriginUIStyle
    {
        public GridLineStyle GridLineStyle;
        public CoordinateGizmoStyle CoordinateGizmoStyle;
    }

    [Serializable]
    public class MueUIStyle
    {
        public GridLineStyle GridLineStyle;
        public CoordinateGizmoStyle CoordinateGizmoStyle;
        public DimensionLineStyle DimensionLineStyle;
        public LabelStyle LabelStyle;
    }

    [Serializable]
    public class LambdaUIStyle
    {
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
        public Vector2 MarkDimensions = new Vector2(1f,2f);
        public ColorSet Color = ColorSet.DefaultSet;
        public float Width = 0.5f;
    }

    [Serializable]
    public class GridLineStyle
    {
        public ColorSet Color = ColorSet.DefaultSet;
        public float Width = 0.5f;
    }

    [Serializable]
    public class DimensionLineStyle
    {
        public ColorSet Color = ColorSet.DefaultSet;
        public float Width = 0.5f;
    }

    [Serializable]
    public class LabelStyle
    {
        public ColorSet Color = ColorSet.DefaultSet;
        public float FontSize = 10f;
    }
    [Serializable]
    public class CircleStyle
    {
        public ColorSet Color = ColorSet.DefaultSet;
        public float Radius = 10f;
        public float Width = 2f;
    }

    [Serializable]
    public class ColorSet : SketchStyle.StyleSet<Color>
    {
        public static ColorSet DefaultSet => new ColorSet() {DefaultStyle = Color.black, DimmedStyle = Color.grey, FocusStyle = Color.blue};
    }
}
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "paraSketch/CoordinateUIStyle")]
public class CoordinateUIStyle : ScriptableObject
{
    public LambdaUIStyle LambdaStyle;
    public MueUIStyle MueStyle;
    public OriginUIStyle OriginStyle;

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
    public class CoordinateGizmoStyle
    {
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
    public class ColorSet : SketchStyle.StyleSet<Color>
    {
        public static ColorSet DefaultSet => new ColorSet() {DefaultStyle = Color.black, DimmedStyle = Color.grey, FocusStyle = Color.blue};
    }
}
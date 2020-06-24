using System;
using UnityEngine;

[CreateAssetMenu(menuName = "paraSketch/CoordinateUIStyle")]
public class CoordinateUIStyle : ScriptableObject
{
    public LambdaStyleSet LambdaStyle;
    public MueStyleSet MueStyle;
    public OriginStyleSet OriginStyle;

    [Serializable]
    public class LambdaStyleSet : SketchStyle.StyleSet<LambdaUIStyle>
    {
    }

    [Serializable]
    public class MueStyleSet : SketchStyle.StyleSet<MueUIStyle>
    {
    }

    [Serializable]
    public class OriginStyleSet : SketchStyle.StyleSet<OriginUIStyle>
    {
    }

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
        public Color Color = Color.black;
        public float Width = 0.5f;
    }

    [Serializable]
    public class GridLineStyle
    {
        public Color Color = Color.black;
        public float Width = 0.5f;
    }

    [Serializable]
    public class DimensionLineStyle
    {
        public Color Color = Color.black;
        public float Width = 0.5f;
    }

    [Serializable]
    public class LabelStyle
    {
        public Color Color = Color.black;
        public float FontSize = 10f;
    }

}
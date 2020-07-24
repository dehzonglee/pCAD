using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "paraSketch/GeometryStyle")]
public class GeometryStyle : ScriptableObject
{

    [Serializable]
    public class GeomStyleSet : SketchStyle.StyleSet<GeoStyle>
    {
    }

    [Serializable]
    public class GeoStyle
    {
        public ColorAsset OutlineColor ;
        public ColorAsset FillColor ;
        public float OutlineWidth = 0.5f;
    }
}
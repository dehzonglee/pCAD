using System;

[Serializable]
public class SketchStyle
{
    public CoordinateUIStyle CoordinateUIStyle;
    public GeometryStyle GeometryStyle;
    
    public class StyleSet<T>
    {
        public T Default;
        public T Dimmed;
        public T Focus;
    }
}

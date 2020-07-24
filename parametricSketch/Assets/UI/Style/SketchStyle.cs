using System;
using UnityEngine.Serialization;

[Serializable]
public class SketchStyle
{
    public CoordinateUIStyle CoordinateUIStyle;
    [FormerlySerializedAs("GeometryStyle")] public GeometryStyleAsset _geometryStyleAsset;
    
    public class StyleSet<T>
    {
        public T DefaultStyle;
        public T DraggedOrExplicitInput;
        public T Drawing;
        public T OtherIsDragged;
        public T HasParameterReference;

        public T GetForState(State state)
        {
            switch (state)
            {
                case State.Default:
                    return DefaultStyle;
                case State.DraggedOrExplicitInput:
                    return DraggedOrExplicitInput;
                case State.Drawing:
                    return Drawing;
                case State.OtherIsDragged:
                    return OtherIsDragged;
                case State.HasParameterReference:
                    return HasParameterReference;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
    }
    
        public enum State
        {
            Default,
            DraggedOrExplicitInput,
            Drawing,
            OtherIsDragged,
            HasParameterReference
        }
    
}

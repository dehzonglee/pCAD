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
        public T SelectedStyle;
        public T FocusStyle;
        public T DimmedStyle;
        public T ReferencedStyle;

        public T GetForState(State state)
        {
            switch (state)
            {
                case State.Default:
                    return DefaultStyle;
                case State.Selected:
                    return SelectedStyle;
                case State.Focus:
                    return FocusStyle;
                case State.Dimmed:
                    return DimmedStyle;
                case State.Referenced:
                    return ReferencedStyle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
    }
    
        public enum State
        {
            Default,
            Selected,
            Focus,
            Dimmed,
            Referenced
        }
    
}

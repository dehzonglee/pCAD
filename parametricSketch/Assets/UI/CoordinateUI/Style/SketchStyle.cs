using System;

[Serializable]
public class SketchStyle
{
    public CoordinateUIStyle CoordinateUIStyle;
    public GeometryStyle GeometryStyle;
    
    public class StyleSet<T>
    {
        public T DefaultStyle;
        public T DimmedStyle;
        public T FocusStyle;

        public T GetForState(State state)
        {
            switch (state)
            {
                case State.Default:
                    return DefaultStyle;
                    break;
                case State.Dimmed:
                    return DimmedStyle;
                    break;
                case State.Focus:
                    return FocusStyle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
    }
    
        public enum State
        {
            Default,
            Dimmed,
            Focus
        }
    
}

using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "paraSketch/GeometryStyle")]
public class GeometryStyleAsset : ScriptableObject
{
    public GeometryStyleSet Set;

    [Serializable]
    public class GeometryStyleSet : StyleSet<GeometryStyle>
    {
    }

    [Serializable]
    public class GeometryStyle
    {
        public ColorAsset OutlineColor;
        public ColorAsset FillColorBlack;
        public ColorAsset FillColorGrey;
        public ColorAsset FillColorWhite;
        public float OutlineWidth = 0.5f;
    }

    public enum GeometryColor
    {
        Black,Grey,White
    }

    public class StyleSet<T>
    {
        public T DefaultStyle;
        public T DrawingStyle;

        public T GetForState(State state)
        {
            switch (state)
            {
                case State.Default:
                    return DefaultStyle;
                case State.Drawing:
                    return DrawingStyle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }

    public enum State
    {
        Default,
        Drawing,
    }
}
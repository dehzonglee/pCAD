using System;
using UnityEngine;
using UnityEngine.UI;

//todo; split up into circle, arrow, mark
public class CoordinateGizmoUI : MaskableGraphic
{
    public void UpdateUI(Vector3 positionWorld, Vector3 directionWorld, CoordinateUIStyle.CoordinateGizmoStyle style,
        SketchStyle.State state, Type type)
    {
        _type = type;
        _positionWorld = positionWorld;
        _directionWorld = directionWorld;
        _style = style;
        _state = state;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        switch (_type)
        {
            case Type.Circle:
                UIMeshGenerationHelper.AddCircle(vh, _positionWorld, _style.Width, _style.Color.GetForState(_state));
                break;
            case Type.Arrow:
                UIMeshGenerationHelper.AddArrow(vh, _positionWorld, _directionWorld, _style.Width,
                    _style.Color.GetForState(_state), _style.ArrowAngle);
                break;
            case Type.Mark:
                UIMeshGenerationHelper.AddMark(vh, _positionWorld, _directionWorld, 0.1f * _style.Width, _style.Width,
                    _style.Color.GetForState(_state), _style.MarkDimensions);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum Type
    {
        Circle,
        Arrow,
        Mark
    }

    private Vector3 _positionWorld;
    private Vector3 _directionWorld;
    private CoordinateUIStyle.CoordinateGizmoStyle _style;
    private SketchStyle.State _state;
    private Type _type;
}
using UnityEngine;
using UnityEngine.UI;

public class CoordinateGizmoUI : MaskableGraphic
{
    public void UpdateUI(Vector3 positionWorld, CoordinateUIStyle.CoordinateGizmoStyle style, SketchStyle.State state)
    {
        _positionWorld = positionWorld;
        _style = style;
        _state = state;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddCircle(vh, _positionWorld, _style.Width, _style.Color.GetForState(_state));
    }

    private Vector3 _positionWorld;
    private CoordinateUIStyle.CoordinateGizmoStyle _style;
    private SketchStyle.State _state;
}
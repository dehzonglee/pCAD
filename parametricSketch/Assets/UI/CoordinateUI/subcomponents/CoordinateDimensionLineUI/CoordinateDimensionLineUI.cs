using UnityEngine;
using UnityEngine.UI;

public class CoordinateDimensionLineUI : MaskableGraphic
{
    public void UpdateUI(Vector3 startWorld, Vector3 endWorld,
        CoordinateUIStyle.DimensionLineStyle style, SketchStyle.State state)
    {
        _startWorld = startWorld;
        _endWorld = endWorld;
        _style = style;
        _state = state;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddLine(vh, _startWorld, _endWorld - _startWorld, _style.Width, _style.Color.GetForState(_state),
            UIMeshGenerationHelper.CapsType.Round);
    }

    private CoordinateUIStyle.DimensionLineStyle _style;
    private Vector3 _startWorld;
    private Vector3 _endWorld;
    private SketchStyle.State _state;
}
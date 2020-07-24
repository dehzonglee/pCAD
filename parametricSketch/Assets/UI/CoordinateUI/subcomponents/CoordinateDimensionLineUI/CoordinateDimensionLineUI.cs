using UnityEngine;
using UnityEngine.UI;

public class CoordinateDimensionLineUI : MaskableGraphic
{
    public void UpdateUI(Vector3 startWorld, Vector3 endWorld,
        CoordinateUIStyle.DimensionLineStyle style, CoordinateUIStyle.ColorSet colors, SketchStyle.State state)
    {
        _startWorld = startWorld;
        _endWorld = endWorld;
        _style = style;
        _state = state;
        _colors = colors;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddLine(vh, _startWorld, _endWorld - _startWorld, _style.Width,
            _colors.GetForState(_state).Value,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private CoordinateUIStyle.DimensionLineStyle _style;
    private Vector3 _startWorld;
    private Vector3 _endWorld;
    private SketchStyle.State _state;
    private CoordinateUIStyle.ColorSet _colors;
}
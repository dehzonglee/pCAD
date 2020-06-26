using UnityEngine;
using UnityEngine.UI;

//todo; split up into circle, arrow, mark
public class CircleGizmo : MaskableGraphic
{
    public void UpdateUI(Vector3 positionWorld, float radius, float width, Color circleColor)
    {
        _positionWorld = positionWorld;
        _radius = radius;
        _color = circleColor;
        _width = width;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddCircleOutline(vh, _positionWorld, _radius, _width, _color);
    }

    private Vector3 _positionWorld;
    private float _radius;
    private Color _color;
    private float _width;
}
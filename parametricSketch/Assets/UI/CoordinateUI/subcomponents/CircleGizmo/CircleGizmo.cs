using UnityEngine;
using UnityEngine.UI;

//todo; split up into circle, arrow, mark
public class CircleGizmo : MaskableGraphic
{
    public void UpdateUI(Vector3 positionWorld, float outerRadius, float innerRadius, float width, Color circleColor)
    {
        _positionWorld = positionWorld;
        _radius = outerRadius;
        _innerRadius = innerRadius;
        _color = circleColor;
        _width = width;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        var p = WorldScreenTransformationHelper.WorldToScreenPoint(_positionWorld);
        void DrawLine(Vector2 direction)
        {
            var o = p + direction * _innerRadius;
            var v = direction * _radius;
            UIMeshGenerationHelper.AddLine(vh, o, v, _width, _color, UIMeshGenerationHelper.CapsType.None);
        }
        DrawLine(Vector2.up + Vector2.right);
        DrawLine(Vector2.up + Vector2.left);
        DrawLine(Vector2.down + Vector2.right);
        DrawLine(Vector2.down + Vector2.left);
    }

    private Vector3 _positionWorld;
    private float _radius;
    private Color _color;
    private float _width;
    private float _innerRadius;
}
using UnityEngine;
using UnityEngine.UI;

public class RectangleOutlineUI : MaskableGraphic
{
    public void UpdateUI(Vector3 lowerLeftCornerWorld, Vector3 upperRightCornerWorld,
        GeometryStyle.RectangleStyle style)
    {
        _lowerLeftCornerWorld = lowerLeftCornerWorld;
        _upperRightCornerWorld = upperRightCornerWorld;
        _style = style;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // quick fix: assume that rectangle is projected on the xz plane
        vh.Clear();

        var p0World = _lowerLeftCornerWorld;
        var p1World = new Vector3(_upperRightCornerWorld.x, 0f, _lowerLeftCornerWorld.z);
        var p2World = _upperRightCornerWorld;
        var p3World = new Vector3(_lowerLeftCornerWorld.x, 0f, _upperRightCornerWorld.z);

        UIMeshGenerationHelper.AddLine(vh, p0World, p1World - p0World, _style.OutlineWidth, _style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p1World, p2World - p1World, _style.OutlineWidth, _style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p2World, p3World - p2World, _style.OutlineWidth, _style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p3World, p0World - p3World, _style.OutlineWidth, _style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private Vector3 _lowerLeftCornerWorld;
    private Vector3 _upperRightCornerWorld;
    private GeometryStyle.RectangleStyle _style;
}
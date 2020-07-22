using UnityEngine;
using UnityEngine.UI;

public class LineUI : MaskableGraphic
{
    public void UpdateUI(Vector3 p0, Vector3 p1,
        GeometryStyle.LineStyle style)
    {
        _p0 = p0;
        _p1 = p1;
        _style = style;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // quick fix: assume that rectangle is projected on the xz plane
        vh.Clear();

        UIMeshGenerationHelper.AddLine(vh, _p0, _p1-_p0, _style.OutlineWidth, _style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private Vector3 _p0;
    private Vector3 _p1;
    private GeometryStyle.LineStyle _style;
}
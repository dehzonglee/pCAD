using UnityEngine;
using UnityEngine.UI;

public class PointUI : MaskableGraphic
{
    public void UpdateUI(Vector3 positionWorld, GeometryStyle.PointStyle style)
    {
        _positionWorld = positionWorld;
        _style = style;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        
        
        UIMeshGenerationHelper.AddLine(vh, _positionWorld, Vector3.zero, _style.OutlineWidth, _style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        
        
    }

    private Vector3 _positionWorld;
    private GeometryStyle.PointStyle _style;
}
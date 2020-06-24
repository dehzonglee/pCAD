using UnityEngine;
using UnityEngine.UI;

public class CoordinateGizmoUI : MaskableGraphic
{
    public void UpdateUI(Vector3 positionWorld, CoordinateUIStyle.CoordinateGizmoStyle style)
    {
        _positionWorld = positionWorld;
        _style = style;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddCircle(vh, _positionWorld, _style.Width, _style.Color);
    }

    private Vector3 _positionWorld;
    private CoordinateUIStyle.CoordinateGizmoStyle _style;
}
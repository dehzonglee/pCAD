using UnityEngine;
using UnityEngine.UI;

public class CoordinateGizmoUI : MaskableGraphic
{
    [SerializeField] private float width = 1f;

    public void UpdateUI(Vector3 positionWorld)
    {
        _positionWorld = positionWorld;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddCircle(vh, _positionWorld,  width, base.color);
    }

    private Vector3 _positionWorld;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineUI : MaskableGraphic
{
    [SerializeField] private float width = 1f;

    public void UpdateUI(Vector3 originWorld, Vector3 directionWorld)
    {
        _originWorld = originWorld;
        _directionWorld = directionWorld;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // quick fix: assume that rectangle is projected on the xz plane
        vh.Clear();
        UIMeshGenerationHelper.AddScreenSpanningLine(vh, _originWorld, _directionWorld, width, base.color);
    }

    private Vector3 _originWorld;
    private Vector3 _directionWorld;
}
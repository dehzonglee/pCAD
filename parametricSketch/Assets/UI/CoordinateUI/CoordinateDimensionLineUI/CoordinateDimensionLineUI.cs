using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordinateDimensionLineUI : MaskableGraphic
{
    [SerializeField] private float width = 1f;

    public void UpdateUI(Vector3 startWorld, Vector3 endWorld, bool renderAsPreview)
    {
        _startWorld = startWorld;
        _endWorld = endWorld;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIMeshGenerationHelper.AddLine(vh, _startWorld, _endWorld - _startWorld, width, base.color,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private Vector3 _startWorld;
    private Vector3 _endWorld;
}
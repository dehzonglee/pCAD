using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectangleUI2D : MaskableGraphic
{
    private (float max, float min) _xDomainWorld;
    private (float max, float min) _yDomainWorld;

    public void UpdateCoordinates((float max, float min) xDomain, (float max, float min) yDomain)
    {
        _xDomainWorld = xDomain;
        _yDomainWorld = yDomain;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        var p0World = new Vector3(_xDomainWorld.min, 0f, _yDomainWorld.min);
        var p1World = new Vector3(_xDomainWorld.max, 0f, _yDomainWorld.min);
        var p2World = new Vector3(_xDomainWorld.max, 0f, _yDomainWorld.max);
        var p3World = new Vector3(_xDomainWorld.min, 0f, _yDomainWorld.max);
        UIMeshGenerationHelper.AddQuadrilateral(vh, (p0World, p1World, p2World, p3World), color);
    }
}
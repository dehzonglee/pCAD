using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectangleUI2D : MaskableGraphic
{
    public float GridCellSize = 40f;


    private (float max, float min) _xDomain;
    private (float max, float min) _yDomain;

    public void UpdateCoordinates((float max, float min) xDomain, (float max, float min) yDomain)
    {
        _xDomain = xDomain;
        _yDomain = yDomain;
        SetVerticesDirty();
    }

    public float x;
    public float y;

    // helper to easily create quads for our ui mesh. You could make any triangle-based geometry other than quads, too!
    private void AddQuad(VertexHelper vh, (float max, float min) xDomain, (float max, float min) yDomain)
    {
        var i = vh.currentVertCount;
        UIVertex vert = new UIVertex();
        vert.color = color;
        var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        
        vert.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomain.min, 0f, yDomain.min))
                        - screenCenter;
        vert.uv0 = Vector2.zero;
        vh.AddVert(vert);
        
        vert.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomain.max, 0f, yDomain.min)) -
            screenCenter;
        vert.uv0 = Vector2.up;
        vh.AddVert(vert);

        vert.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomain.max, 0f, yDomain.max)) -
            screenCenter;
        vert.uv0 = Vector2.right + Vector2.up;
        vh.AddVert(vert);

        vert.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomain.min, 0f, yDomain.max)) -
            screenCenter;
        vert.uv0 = Vector2.right;
        vh.AddVert(vert);

        vh.AddTriangle(i + 0, i + 2, i + 1);
        vh.AddTriangle(i + 3, i + 2, i + 0);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        AddQuad(vh, _xDomain, _yDomain);
    }
}
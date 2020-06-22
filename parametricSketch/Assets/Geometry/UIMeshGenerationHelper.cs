using UnityEngine;
using UnityEngine.UI;

public static class UIMeshGenerationHelper
{
    // helper to easily create quads for our ui mesh. You could make any triangle-based geometry other than quads, too!
    public static void AddRectangle(VertexHelper vh, (float max, float min) xDomainWorld,
        (float max, float min) yDomainWorld,
        Color color)
    {
        var p0 = new Vector3(xDomainWorld.min, 0f, yDomainWorld.min);

        var i = vh.currentVertCount;
        var vertex = new UIVertex();

        vertex.color = color;
        var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;

        vertex.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomainWorld.min, 0f, yDomainWorld.min))
            - screenCenter;
        vertex.uv0 = Vector2.zero;
        vh.AddVert(vertex);

        vertex.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomainWorld.max, 0f, yDomainWorld.min)) -
            screenCenter;
        vertex.uv0 = Vector2.up;
        vh.AddVert(vertex);

        vertex.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomainWorld.max, 0f, yDomainWorld.max)) -
            screenCenter;
        vertex.uv0 = Vector2.right + Vector2.up;
        vh.AddVert(vertex);

        vertex.position =
            RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(xDomainWorld.min, 0f, yDomainWorld.max)) -
            screenCenter;
        vertex.uv0 = Vector2.right;
        vh.AddVert(vertex);

        vh.AddTriangle(i + 0, i + 2, i + 1);
        vh.AddTriangle(i + 3, i + 2, i + 0);
    }


    public static Vector2 WorldToScreenPoint(Vector3 worldPosition)
    {
        var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        return RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition) - screenCenter;
    }

    public static void AddLine(VertexHelper vh, Vector3 originWorld, Vector3 directionWorld, float width, Color color)
    {
        AddLine(vh, WorldToScreenPoint(originWorld), WorldToScreenPoint(directionWorld), width, color);
    }

    public static void AddLine(VertexHelper vh, Vector2 originScreen, Vector2 directionScreen, float width, Color color)
    {
        var widthVector = Vector2.Perpendicular(directionScreen).normalized * width;
        var p0 = originScreen + widthVector;
        var p1 = originScreen + directionScreen + widthVector;
        var p2 = originScreen + directionScreen - widthVector;
        var p3 = originScreen - widthVector;
        AddQuadrilateral(vh, (p0, p1, p2, p3), color);
    }

    public static void AddQuadrilateral(VertexHelper vh, (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) worldPosition,
        Color color)
    {
        AddQuadrilateral(
            vh,
            (
                WorldToScreenPoint(worldPosition.p0),
                WorldToScreenPoint(worldPosition.p1),
                WorldToScreenPoint(worldPosition.p2),
                WorldToScreenPoint(worldPosition.p3)
            ),
            color);
    }

    public static void AddQuadrilateral(VertexHelper vh,
        (Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) screenPosition, Color color)
    {
        var i = vh.currentVertCount;
        var vertex = new UIVertex();

        vertex.color = color;
        vertex.position = screenPosition.p0;
        vertex.uv0 = Vector2.zero;
        vh.AddVert(vertex);

        vertex.position = screenPosition.p1;
        vertex.uv0 = Vector2.up;
        vh.AddVert(vertex);

        vertex.position = screenPosition.p2;
        vertex.uv0 = Vector2.right + Vector2.up;
        vh.AddVert(vertex);

        vertex.position = screenPosition.p3;
        vertex.uv0 = Vector2.right;
        vh.AddVert(vertex);

        vh.AddTriangle(i + 0, i + 2, i + 1);
        vh.AddTriangle(i + 3, i + 2, i + 0);
    }
}
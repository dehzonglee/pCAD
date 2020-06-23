using System;
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

    public static void AddLine(VertexHelper vh, Vector3 originWorld, Vector3 directionWorld, float width, Color color,
        CapsType capsType)
    {
        AddLine(vh, WorldScreenTransformationHelper.WorldToScreenPoint(originWorld), WorldScreenTransformationHelper.WorldToScreenPoint(directionWorld), width, color, capsType);
    }

    public static void AddLine(VertexHelper vh, Vector2 originScreen, Vector2 directionScreen, float width, Color color,
        CapsType capsType)
    {
        var widthVector = Vector2.Perpendicular(directionScreen).normalized * width;
        var p0 = originScreen + widthVector;
        var p1 = originScreen + directionScreen + widthVector;
        var p2 = originScreen + directionScreen - widthVector;
        var p3 = originScreen - widthVector;
        AddQuadrilateral(vh, (p0, p1, p2, p3), color);

        switch (capsType)
        {
            case CapsType.None:
                break;
            case CapsType.Round:
                AddCircleSegment(vh, originScreen, widthVector, 180f, color);
                AddCircleSegment(vh, originScreen + directionScreen, -widthVector, 180f, color);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(capsType), capsType, null);
        }
    }

    public static void AddScreenSpanningLine(VertexHelper vh, Vector3 originWorld, Vector3 directionWorld,
        float width, Color color)
    {
        AddScreenSpanningLine(vh, WorldScreenTransformationHelper.WorldToScreenPoint(originWorld), WorldScreenTransformationHelper.WorldToScreenPoint(directionWorld), width, color);
    }

    public static void AddScreenSpanningLine(VertexHelper vh, Vector2 originScreen, Vector2 directionScreen,
        float width, Color color)
    {
        Vector2 start;
        Vector2 end;

        if (directionScreen.magnitude == EPSILON)
            return;

        // is vertical
        if (Math.Abs(directionScreen.x) < EPSILON)
        {
            start = new Vector2(originScreen.x, -Screen.height / 2f);
            end = new Vector2(originScreen.x, Screen.height / 2f);
        }
        // is horizontal
        else if (Math.Abs(directionScreen.y) < EPSILON)
        {
            start = new Vector2(-Screen.width / 2f, originScreen.y);
            end = new Vector2(Screen.width / 2f, originScreen.y);
        }
        else
        {
            //todo decide if line should be drawn until horizontal or until vertical border
            var m = directionScreen.y / directionScreen.x;

            var deltaXToLeftBorder = -Screen.width / 2f - originScreen.x;
            var deltaXToRightBorder = Screen.width / 2f - originScreen.x;

            var yOnLeftBorder = originScreen.y + deltaXToLeftBorder * m;
            var yOnRightBorder = originScreen.y + deltaXToRightBorder * m;

            start = new Vector2(-Screen.width / 2f, yOnLeftBorder);
            end = new Vector2(Screen.width / 2f, yOnRightBorder);
        }

        var widthVector = Vector2.Perpendicular(directionScreen).normalized * width;
        var p0 = start + widthVector;
        var p1 = end + 10 * directionScreen + widthVector;
        var p2 = end + 10 * directionScreen - widthVector;
        var p3 = start - widthVector;
        AddQuadrilateral(vh, (p0, p1, p2, p3), color);
    }

    private static void AddCircleSegment(VertexHelper vh, Vector2 circleCenterScreen, Vector2 startVector,
        float angleInDegrees, Color color)
    {
        var segmentResolution = angleInDegrees / 360f * CircleResolution;
        for (var i = 0; i < segmentResolution; i++)
        {
            var angleP0 = i * angleInDegrees / segmentResolution;
            var angleP1 = (i + 1) * angleInDegrees / segmentResolution;
            var p0 = circleCenterScreen + RotateVector(startVector, angleP0);
            var p1 = circleCenterScreen + RotateVector(startVector, angleP1);
            AddTriangle(vh, (circleCenterScreen, p0, p1), color);
        }
    }

    private static Vector2 RotateVector(Vector2 v, float angleInDegrees)
    {
        var angleInRadians = angleInDegrees / 180f * Mathf.PI;
        var cosOfAngle = Mathf.Cos(angleInRadians);
        var sinOfAngle = Mathf.Sin(angleInRadians);
        return new Vector2(
            v.x * cosOfAngle - v.y * sinOfAngle,
            v.x * sinOfAngle - v.y * cosOfAngle);
    }

    public static void AddQuadrilateral(VertexHelper vh, (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) worldPosition,
        Color color)
    {
        AddQuadrilateral(
            vh,
            (
                WorldScreenTransformationHelper.WorldToScreenPoint(worldPosition.p0),
                WorldScreenTransformationHelper.WorldToScreenPoint(worldPosition.p1),
                WorldScreenTransformationHelper.WorldToScreenPoint(worldPosition.p2),
                WorldScreenTransformationHelper.WorldToScreenPoint(worldPosition.p3)
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

    private static void AddTriangle(VertexHelper vh, (Vector2 p0, Vector2 p1, Vector2 p2) screenPosition, Color color)
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

        vh.AddTriangle(i + 0, i + 2, i + 1);
    }

    public enum CapsType
    {
        None,
        Round
    }

    private const int CircleResolution = 20;

    public static void AddCircle(VertexHelper vh, Vector3 positionWorld, float width, Color color)
    {
        var positionScreen = WorldScreenTransformationHelper.WorldToScreenPoint(positionWorld);
        AddCircleSegment(vh, positionScreen, Vector2.right * width, 360f, color);
    }

    private const float EPSILON = 0.01f;
}
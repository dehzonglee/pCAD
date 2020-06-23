using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldScreenTransformationHelper
{
    public static Vector2 WorldToScreenPoint(Vector3 worldPosition)
    {
        var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        return RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition) - screenCenter;
    }

    public static Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        var screenPos = WorldToScreenPoint(worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out var movePos);
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
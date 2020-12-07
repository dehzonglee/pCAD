using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cursorUI", menuName = "parametricSketch/CursorUI", order = 1)]
public class CursorUI : ScriptableObject
{
    public Texture2D VerticalManipulationCursor;
    public Texture2D HorizontalManipulationCursor;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public void UpdateCursor((Coordinate coordinate, Vec.AxisID axis)? hitResult)
    {
        if (hitResult.HasValue)
        {
            var cursor = hitResult.Value.axis == Vec.AxisID.X
                ? HorizontalManipulationCursor
                : VerticalManipulationCursor;
            Cursor.SetCursor(cursor, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(null, hotSpot, cursorMode);
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }
}
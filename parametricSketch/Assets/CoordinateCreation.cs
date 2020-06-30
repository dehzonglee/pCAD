using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public static class CoordinateCreation
{
    public static Vec<Coordinate> UpdateCursorPosition(Vec<Coordinate> oldFocusPosition,
        CoordinateSystem cs, KeyboardInput.Model keyboardInput)
    {
        oldFocusPosition?.ForEach(c =>
        {
            if (c.IsPreview) c.Delete();
        });

        return GetOrCreatePositionAtMousePosition(cs, cs.Anchor, true, keyboardInput);
    }

    public static void DeletePositionAtMousePosition(CoordinateSystem cs)
    {
        var p = GetOrCreatePositionAtMousePosition(cs, cs.Anchor);
        p.ForEach(c => c.Delete());
    }

    private static Vec<Coordinate> GetOrCreatePositionAtMousePosition(CoordinateSystem coordinateSystem,
        Anchor anchor,
        bool asPreview = false,
        KeyboardInput.Model keyboardInput = null)
    {
        var mousePosition = MouseInput.RaycastPosition;
        var distanceToAnchor = new Vec<float>(
            mousePosition.X - anchor.PrimaryPosition.x,
            mousePosition.Y - anchor.PrimaryPosition.y,
            mousePosition.Z - anchor.PrimaryPosition.z
        );

        return
            coordinateSystem.GetParametricPosition(mousePosition, distanceToAnchor, asPreview, keyboardInput);
    }

    public static void BakePosition(Vec<Coordinate> modelFocusPosition)
    {
        modelFocusPosition.ForEach(c => c.Bake());
    }
}
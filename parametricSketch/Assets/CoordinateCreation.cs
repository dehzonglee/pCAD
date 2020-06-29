using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public static class CoordinateCreation
{
    public static Vec<Coordinate> UpdateFocusPosition(Vec<Coordinate> oldFocusPosition,
        CoordinateSystem cs,
        Vec<float?> keyboardInputFloats, Vec<MueParameter> keyboardInputParameters,
        Vec<bool> keyboardInputNegativeDirection)
    {
        oldFocusPosition?.ForEach(c =>
        {
            if (c.IsPreview) c.Delete();
        });

        return GetOrCreatePositionAtMousePosition(cs, cs.Anchor,true, keyboardInputFloats, keyboardInputParameters,
            keyboardInputNegativeDirection);
    }

    public static void DeletePositionAtMousePosition(CoordinateSystem cs)
    {
        var p = GetOrCreatePositionAtMousePosition(cs,cs.Anchor);
        p.ForEach(c => c.Delete());
    }

    private static Vec<Coordinate> GetOrCreatePositionAtMousePosition(
        CoordinateSystem coordinateSystem,
        Anchor anchor,
        bool asPreview = false,
        Vec<float?> keyboardInput = null,
        Vec<MueParameter> keyboardInputParameters = null,
        Vec<bool> keyboardInputNegativeDirection = null)
    {
        var mousePosition = MouseInput.RaycastPosition;
        var distanceToAnchor = new Vec<float>(
            mousePosition.X - anchor.PrimaryPosition.x,
            mousePosition.Y - anchor.PrimaryPosition.y,
            mousePosition.Z - anchor.PrimaryPosition.z
        );

        return
            coordinateSystem.GetParametricPosition(mousePosition, distanceToAnchor, asPreview, keyboardInput,
                keyboardInputParameters, keyboardInputNegativeDirection);
    }

    public static void BakePosition(Vec<Coordinate> modelFocusPosition)
    {
        modelFocusPosition.ForEach(c => c.Bake());
    }
}
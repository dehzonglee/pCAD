using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public static class CoordinateCreation
{
    public static GenericVector<Coordinate> UpdateFocusPosition(GenericVector<Coordinate> oldFocusPosition,
        CoordinateSystem cs,
        GenericVector<float?> keyboardInputFloats, GenericVector<MueParameter> keyboardInputParameters,
        GenericVector<bool> keyboardInputNegativeDirection)
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

    private static GenericVector<Coordinate> GetOrCreatePositionAtMousePosition(
        CoordinateSystem coordinateSystem,
        Anchor anchor,
        bool asPreview = false,
        GenericVector<float?> keyboardInput = null,
        GenericVector<MueParameter> keyboardInputParameters = null,
        GenericVector<bool> keyboardInputNegativeDirection = null)
    {
        var mousePosition = MouseInput.RaycastPosition;
        var distanceToAnchor = new GenericVector<float>(
            mousePosition.X - anchor.PrimaryPosition.x,
            mousePosition.Y - anchor.PrimaryPosition.y,
            mousePosition.Z - anchor.PrimaryPosition.z
        );

        return
            coordinateSystem.GetParametricPosition(mousePosition, distanceToAnchor, asPreview, keyboardInput,
                keyboardInputParameters, keyboardInputNegativeDirection);
    }

    public static void BakePosition(GenericVector<Coordinate> modelFocusPosition)
    {
        modelFocusPosition.ForEach(c => c.Bake());
    }
}
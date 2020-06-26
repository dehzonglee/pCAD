using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public static class CoordinateCreation
{
    public static (Coordinate x, Coordinate y, Coordinate z)? UpdateFocusPosition(
        (Coordinate x, Coordinate y, Coordinate z)? oldFocusPosition, CoordinateSystem cs,
        Axis.GenericVector<float?> keyboardInput )
    {
        if (oldFocusPosition.HasValue)
        {
            var p = oldFocusPosition.Value;
            if (p.x.IsPreview) p.x.Delete();
            if (p.y.IsPreview) p.y.Delete();
            if (p.z.IsPreview) p.z.Delete();
        }

        return GetOrCreatePositionAtMousePosition(cs, true, keyboardInput );
    }

    public static void DeletePositionAtMousePosition(CoordinateSystem cs)
    {
        var p = GetOrCreatePositionAtMousePosition(cs);
        p.x.Delete();
        p.y.Delete();
        p.z.Delete();
    }

    private static (Coordinate x, Coordinate y, Coordinate z) GetOrCreatePositionAtMousePosition(
        CoordinateSystem coordinateSystem, bool asPreview = false, Axis.GenericVector<float?> keyboardInput = null)
    {
        var position =
            coordinateSystem.GetParametricPosition(MouseInput.RaycastPosition, asPreview, keyboardInput);
        return position;
    }

    public static void BakePosition((Coordinate x, Coordinate y, Coordinate z) modelFocusPosition)
    {
        modelFocusPosition.x.Bake();
        modelFocusPosition.y.Bake();
        modelFocusPosition.z.Bake();
    }
}
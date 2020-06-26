using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectangleCreation
{
    public static Sketch.RectangleModel StartNewRectangle(
        (Coordinate x, Coordinate y, Coordinate z) focusPosition)
    {
        var nextRectangle = new Sketch.RectangleModel {P0 = focusPosition};

        focusPosition.x.AddAttachedGeometry(nextRectangle);
        focusPosition.y.AddAttachedGeometry(nextRectangle);
        focusPosition.z.AddAttachedGeometry(nextRectangle);

        return nextRectangle;
    }

    public static void CompleteRectangle(Sketch.RectangleModel nextRectangle,
        (Coordinate x, Coordinate y, Coordinate z) focusPosition)
    {
        nextRectangle.P1 = focusPosition;
        nextRectangle.IsBaked = true;
    }

    public static void UpdateRectangle(Sketch.RectangleModel nextRectangle,
        (Coordinate x, Coordinate y, Coordinate z) focusPosition)
    {
        nextRectangle.P1 = focusPosition;
    }

    public static void AbortRectangle(Sketch.RectangleModel nextRectangle)
    {
        nextRectangle.P0.x.UnregisterGeometryAndTryToDelete(nextRectangle);
        nextRectangle.P0.y.UnregisterGeometryAndTryToDelete(nextRectangle);
        nextRectangle.P0.z.UnregisterGeometryAndTryToDelete(nextRectangle);
    }
}
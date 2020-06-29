using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectangleCreation
{
    public static Sketch.RectangleModel StartNewRectangle(
        Vec<Coordinate> focusPosition)
    {
        var nextRectangle = new Sketch.RectangleModel {P0 = focusPosition};
        focusPosition.ForEach(c => c.AddAttachedGeometry(nextRectangle));
        return nextRectangle;
    }

    public static void CompleteRectangle(Sketch.RectangleModel nextRectangle,
        Vec<Coordinate> focusPosition)
    {
        nextRectangle.P1 = focusPosition;
        nextRectangle.IsBaked = true;
    }

    public static void UpdateRectangle(Sketch.RectangleModel nextRectangle,
        Vec<Coordinate> focusPosition)
    {
        nextRectangle.P1 = focusPosition;
    }

    public static void AbortRectangle(Sketch.RectangleModel nextRectangle)
    {
        nextRectangle.P0.ForEach(c=>c.UnregisterGeometryAndTryToDelete(nextRectangle));
    }
}
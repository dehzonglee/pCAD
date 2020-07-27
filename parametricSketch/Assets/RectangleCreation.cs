public static class RectangleCreation
{
    public static RectangleModel StartNewRectangle(
        Vec<Coordinate> focusPosition)
    {
        var nextRectangle = new RectangleModel {P0 = focusPosition};
        focusPosition.ForEach(c => c.AddAttachedGeometry(nextRectangle));
        return nextRectangle;
    }

    public static void CompleteRectangle(RectangleModel nextRectangle,
        Vec<Coordinate> focusPosition)
    {
        nextRectangle.P1 = focusPosition;
        nextRectangle.IsBaked = true;
    }

    public static void UpdateRectangle(RectangleModel nextRectangle,
        Vec<Coordinate> focusPosition)
    {
        nextRectangle.P1 = focusPosition;
    }

    public static void AbortRectangle(RectangleModel nextRectangle)
    {
        nextRectangle.P0.ForEach(c=>c.UnregisterGeometryAndTryToDelete(nextRectangle));
    }
}
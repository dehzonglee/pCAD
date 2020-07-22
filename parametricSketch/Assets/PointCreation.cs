public static class PointCreation
{
    public static Sketch.PointModel NewPoint(
        Vec<Coordinate> focusPosition)
    {
        var nextPoint = new Sketch.PointModel {P0 = focusPosition, IsBaked = true};
        focusPosition.ForEach(c => c.AddAttachedGeometry(nextPoint));
        return nextPoint;
    }
}
public static class LineCreation
{
    public static Sketch.LineModel StartNewLine(
        Vec<Coordinate> focusPosition)
    {
        var nextLine = new Sketch.LineModel {P0 = focusPosition};
        focusPosition.ForEach(c => c.AddAttachedGeometry(nextLine));
        return nextLine;
    }

    public static void CompleteLine(Sketch.LineModel nextLine, Vec<Coordinate> focusPosition)
    {
        nextLine.P1 = focusPosition;
        nextLine.IsBaked = true;
    }

    public static void UpdateLine(Sketch.LineModel nextLine, Vec<Coordinate> focusPosition)
    {
        nextLine.P1 = focusPosition;
    }

    public static void AbortLine(Sketch.LineModel nextLine)
    {
        nextLine.P0.ForEach(c=>c.UnregisterGeometryAndTryToDelete(nextLine));
    }
}
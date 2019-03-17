public class AnchorCoordinates
{
    public Coordinate PrimaryCoordinate { get; private set; }

    public Coordinate SecondaryCoordinate { get; private set; }

    public bool AnchorsMatch => PrimaryCoordinate == SecondaryCoordinate;

    public AnchorCoordinates(Coordinate primary, Coordinate secondary)
    {
        PrimaryCoordinate = primary;
        SecondaryCoordinate = secondary;
    }

    public void SetPrimaryCoordinate(Coordinate c)
    {
        SecondaryCoordinate = PrimaryCoordinate;
        PrimaryCoordinate = c;
    }

    public void ResetAnchors(Origin origin)
    {
        PrimaryCoordinate = origin;
        SecondaryCoordinate = origin;
    }
}
using System;

public class AnchorCoordinates
{
    public Coordinate PrimaryCoordinate { get { return _primaryCoordinate; } }

    public Coordinate SecondaryCoordinate { get { return _secondaryCoordinate; } }

    public bool AnchorsMatch { get { return _primaryCoordinate == _secondaryCoordinate; } }

    public AnchorCoordinates(Coordinate primary, Coordinate secondary)
    {
        _primaryCoordinate = primary;
        _secondaryCoordinate = secondary;
    }

    public void SetPrimaryCoordinate(Coordinate c)
    {
        _secondaryCoordinate = _primaryCoordinate;
        _primaryCoordinate = c;
    }

    private Coordinate _primaryCoordinate;

    private Coordinate _secondaryCoordinate;
}
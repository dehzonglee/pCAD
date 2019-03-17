using Model;
using UnityEngine;

public class Anchor
{
    public AnchorCoordinates XCoordinates { get { return _coordinates[0]; } }

    public AnchorCoordinates YCoordinates { get { return _coordinates[1]; } }

    public AnchorCoordinates ZCoordinates { get { return _coordinates[2]; } }

    public Anchor(AnchorCoordinates x, AnchorCoordinates y, AnchorCoordinates z)
    {
        _coordinates[0] = x;
        _coordinates[1] = y;
        _coordinates[2] = z;
    }

    public Vector3 PrimaryPosition
    {
        get
        {
            var x = XCoordinates.PrimaryCoordinate.Value;
            var y = YCoordinates.PrimaryCoordinate.Value;
            var z = ZCoordinates.PrimaryCoordinate.Value;
            return new Vector3(x, y, z);
        }
    }

    public Vector3 SecondaryPosition
    {
        get
        {
            var x = XCoordinates.SecondaryCoordinate.Value;
            var y = YCoordinates.SecondaryCoordinate.Value;
            var z = ZCoordinates.SecondaryCoordinate.Value;
            return new Vector3(x, y, z);
        }
    }

    private AnchorCoordinates[] _coordinates = new AnchorCoordinates[3];
}
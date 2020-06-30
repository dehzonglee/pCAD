using Model;
using UnityEngine;

public class Anchor
{
    public Anchor(AnchorCoordinates x, AnchorCoordinates y, AnchorCoordinates z)
    {
        _coordinates = new Vec<AnchorCoordinates>(x, y, z);
    }

    public Vector3 PrimaryPosition
    {
        get
        {
            var x = _coordinates.X.PrimaryCoordinate.Value;
            var y = _coordinates.Y.PrimaryCoordinate.Value;
            var z = _coordinates.Z.PrimaryCoordinate.Value;
            return new Vector3(x, y, z);
        }
    }

    public Vector3 SecondaryPosition
    {
        get
        {
            var x = _coordinates.X.SecondaryCoordinate.Value;
            var y = _coordinates.Y.SecondaryCoordinate.Value;
            var z = _coordinates.Z.SecondaryCoordinate.Value;
            return new Vector3(x, y, z);
        }
    }

    private Vec<AnchorCoordinates> _coordinates;
}
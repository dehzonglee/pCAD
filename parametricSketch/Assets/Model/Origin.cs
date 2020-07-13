using System.Collections.Generic;
using TMPro;
using UnityEditor;

public class Origin : Coordinate
{
    public override string Name => "Origin";
    public override float Value => _originPosition;

    public override (float min, float max) GetBounds()
    {
        return
            (float.NegativeInfinity,
                float.PositiveInfinity); //make origin block the layout row, maybe this should be done in the ui instead
    }

    public Origin(float originPosition) : base(false, null, null, new List<Coordinate>())
    {
        _originPosition = originPosition;
        Parameter = new Parameter( GUID.Generate().ToString(), 0f);
    }

    private float _originPosition;
}
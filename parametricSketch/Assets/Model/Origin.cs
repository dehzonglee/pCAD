using System.Collections.Generic;
using UnityEditor;

public class Origin : Coordinate
{
    public override string Name => "Origin";
    public override float Value => 0f;

    public override (float min, float max) GetBounds()
    {
        return
            (float.NegativeInfinity,
                float.PositiveInfinity); //make origin block the layout row, maybe this should be done in the ui instead
    }

    public Origin() : base(false, null, null, new List<Coordinate>())
    {
        Parameter = new Parameter( GUID.Generate().ToString(), 0f);
    }
}
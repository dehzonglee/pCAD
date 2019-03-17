using System.Collections.Generic;

public class Origin : Coordinate
{
    public override string Name => "Origin";
    public override float Value => 0f;

    public Origin() : base(false, null, null, new List<Coordinate>())
    {
    }
}
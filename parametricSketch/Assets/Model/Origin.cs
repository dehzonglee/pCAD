using System;

public class Origin : Coordinate
{
    public override string Name => "Origin";
    public override float Value => 0f;

    public Origin(Action coordinateChangedCallback)
    {
        CoordinateChangedEvent += coordinateChangedCallback;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[Serializable]
public class Origin : Coordinate
{
    public override string Name => "Origin";
    public override float Value => _originPosition;

    public override (float min, float max) GetBounds()
    {
        return
            (float.NegativeInfinity,
                float.PositiveInfinity); // make origin block the layout row. Todo: move this closer to its usage
    }

    public Origin(float originPosition) : base(false, null, null, new List<Coordinate>())
    {
        _originPosition = originPosition;
        Parameter = new Parameter(GUID.Generate().ToString(), 0f);
    }

    // used during deserialization
    public Origin(string id, float originPosition) : base(id, false, null, null)
    {
        _originPosition = originPosition;
        Parameter = new Parameter(GUID.Generate().ToString(), 0f);
//        Parameter = parameter;
    }

    [Serializable]
    public new class Serialization : Coordinate.Serialization
    {
        public float OriginPosition;
    }

    public Serialization ToSerializableType(int index)
    {
        return new Serialization
        {
            Index = index, ParameterID = Parameter.ID, ID = ID,
            OriginPosition = _originPosition
        };
    }

    private float _originPosition;
}
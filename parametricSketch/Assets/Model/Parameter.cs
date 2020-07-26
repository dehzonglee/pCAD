using System;

[Serializable]
public class Parameter
{
    public  string ID;
    public float Value;

    public Parameter(string id, float value)
    {
        ID = id;
        Value = value;
    }
}
using System;
using Model;

public enum AxisID
{
    X,
    Y,
    Z
};

public class GenericVector<T>
{
    public T X;
    public T Y;
    public T Z;

    public GenericVector()
    {
    }

    public GenericVector(T defaultValue)
    {
        X = defaultValue;
        Y = defaultValue;
        Z = defaultValue;
    }

    public GenericVector(T x, T y, T z)
    {
        X = x;
        Y = y;
        Z = z;
    }


    public void ForEach(Action<T> function)
    {
        function.Invoke(X);
        function.Invoke(Y);
        function.Invoke(Z);
    }

    public T this[AxisID axis]
    {
        get => GetForAxisID(axis);
        set => SetForAxisID(axis, value);
    }

    public T GetForAxisID(AxisID axis)
    {
        switch (axis)
        {
            case AxisID.X:
                return X;
            case AxisID.Y:
                return Y;
            case AxisID.Z:
                return Z;
            default:
                throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
        }
    }

    public void SetForAxisID(AxisID axis, T value)
    {
        switch (axis)
        {
            case AxisID.X:
                X = value;
                break;
            case AxisID.Y:
                Y = value;
                break;
            case AxisID.Z:
                Z = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
        }
    }
}
using System;

public abstract class Coordinate
{
    public bool IsPreview { get; private set; }
    public float ParentValue => _parent.Value;
    protected event Action<Coordinate> CoordinateDeletedEvent;
    protected event Action CoordinateChangedEvent;
    public event Action ValueChangedEvent;
    public abstract string Name { get; }
    public abstract float Value { get; }
    public bool IsUsed => ValueChangedEvent != null;
    protected Action FireValueChangedEvent => () => ValueChangedEvent?.Invoke();


    public Coordinate(bool isPreview, Action<Coordinate> onCoordinateDeleted, Action onCoordinateChanged)
    {
        IsPreview = isPreview;
        CoordinateDeletedEvent += onCoordinateDeleted;
        CoordinateChangedEvent += onCoordinateChanged;
    }

    public float Parameter
    {
        get => _parameter;
        set
        {
            _parameter = value;
            ValueChangedEvent?.Invoke();
        }
    }

    public void Register(Action onValueChanged)
    {
        ValueChangedEvent += onValueChanged;
    }

    public void Unregister(Action onValueChanged)
    {
//        Debug.Log($"unregister {this}");
        ValueChangedEvent -= onValueChanged;
        if (onValueChanged == null)
        {
            CoordinateDeletedEvent?.Invoke(this);
            CoordinateChangedEvent?.Invoke();
        }
    }

    protected Coordinate _parent;
    private float _parameter;

    public void Bake()
    {
        IsPreview = false;
    }
}
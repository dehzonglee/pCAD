using System;
using UnityEngine;

namespace Model
{
    public class ParametricPosition
    {
        public event Action PositionChangedEvent;
        public event Action PositionRemovedEvent;

        public Vector3 Value => new Vector3(_x.Value, _y.Value, _z.Value);

        public Coordinate X
        {
            get => _x;
            set
            {
                _x.Unregister(PositionChangedEvent);
                _x = value;
                _x.Register(PositionChangedEvent);
            }
        }

        public Coordinate Y
        {
            get => _y;
            set
            {
                _y.Unregister(PositionChangedEvent);
                _y = value;
                _y.Register(PositionChangedEvent);
            }
        }

        public Coordinate Z
        {
            get => _z;
            set
            {
                _z.Unregister(PositionChangedEvent);
                _z = value;
                _z.Register(PositionChangedEvent);
            }
        }

        public ParametricPosition(Coordinate x, Coordinate y, Coordinate z)
        {
            _x = x;
            _y = y;
            _z = z;
            x.ValueChangedEvent += delegate { PositionChangedEvent?.Invoke(); };
            y.ValueChangedEvent += delegate { PositionChangedEvent?.Invoke(); };
            z.ValueChangedEvent += delegate { PositionChangedEvent?.Invoke(); };
        }

        public void BakePreview()
        {
            _x.Bake();
            _y.Bake();
            _z.Bake();
        }

        public void RemovePreview()
        {
            if (_x.IsPreview) _x.Unregister(PositionChangedEvent);
            if (_y.IsPreview) _y.Unregister(PositionChangedEvent);
            if (_z.IsPreview) _z.Unregister(PositionChangedEvent);
        }

        private Coordinate _x;
        private Coordinate _y;
        private Coordinate _z;
    }
}
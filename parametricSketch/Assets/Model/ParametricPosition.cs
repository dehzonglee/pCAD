using System;
using UnityEngine;

namespace Model
{
    public class ParametricPosition
    {
        public event Action PositionChangedEvent;

        public Vector3 Value => new Vector3(_x.Value, _y.Value, _z.Value);

        public Coordinate X
        {
            get => _x;
            set
            {
                _x.ValueChangedEvent -= PositionChangedEvent;
                _x = value;
                _x.ValueChangedEvent += PositionChangedEvent;
            }
        }

        public Coordinate Y
        {
            get => _y;
            set
            {
                _y.ValueChangedEvent -= PositionChangedEvent;
                _y = value;
                _y.ValueChangedEvent += PositionChangedEvent;
            }
        }

        public Coordinate Z
        {
            get => _z;
            set
            {
                _z.ValueChangedEvent -= PositionChangedEvent;
                _z = value;
                _z.ValueChangedEvent += PositionChangedEvent;
            }
        }

        public ParametricPosition(Coordinate x, Coordinate y, Coordinate z)
        {
            _x = x;
            _y = y;
            _z = z;
            x.ValueChangedEvent += () => PositionChangedEvent?.Invoke();
            y.ValueChangedEvent += () => PositionChangedEvent?.Invoke();
            z.ValueChangedEvent += () => PositionChangedEvent?.Invoke();
        }


        private Coordinate _x;
        private Coordinate _y;
        private Coordinate _z;
    }
}
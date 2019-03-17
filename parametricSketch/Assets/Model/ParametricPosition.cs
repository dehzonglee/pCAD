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
                _x.UnregisterView(PositionChangedEvent);
                _x = value;
                _x.RegisterView(PositionChangedEvent);
            }
        }

        public Coordinate Y
        {
            get => _y;
            set
            {
                _y.UnregisterView(PositionChangedEvent);
                _y = value;
                _y.RegisterView(PositionChangedEvent);
            }
        }

        public Coordinate Z
        {
            get => _z;
            set
            {
                _z.UnregisterView(PositionChangedEvent);
                _z = value;
                _z.RegisterView(PositionChangedEvent);
            }
        }

        public ParametricPosition(Coordinate x, Coordinate y, Coordinate z)
        {
            _x = x;
            _y = y;
            _z = z;
            x.RegisterView(delegate { PositionChangedEvent?.Invoke(); });
            y.RegisterView(delegate { PositionChangedEvent?.Invoke(); });
            z.RegisterView(delegate { PositionChangedEvent?.Invoke(); });
        }

        public void BakePreview()
        {
            _x.Bake();
            _y.Bake();
            _z.Bake();
        }

        public void RemovePreview()
        {
            if (_x.IsPreview) _x.Remove();
            if (_y.IsPreview) _y.Remove();
            if (_z.IsPreview) _z.Remove();
        }

        public void Remove()
        {
            _x.Remove();
            _y.Remove();
            _z.Remove();
        }

        private Coordinate _x;
        private Coordinate _y;
        private Coordinate _z;
    }
}
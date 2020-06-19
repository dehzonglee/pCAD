using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Model
{
    
    [Serializable]
    public class ParametricPosition
    {
        public event Action<ParametricPosition,Coordinate> PositionChangedEvent;

        public Vector3 Value => new Vector3(_x.Value, _y.Value, _z.Value);

        public Coordinate X
        {
            get => _x;
//            set
//            {
//                _x.UnregisterView(PositionChangedEvent);
//                _x = value;
//                _x.RegisterView(PositionChangedEvent);
//            }
        }

        public Coordinate Y
        {
            get => _y;
//            set
//            {
//                _y.UnregisterView(PositionChangedEvent);
//                _y = value;
//                _y.RegisterView(PositionChangedEvent);
//            }
        }

        public Coordinate Z
        {
            get => _z;
//            set
//            {
//                _z.UnregisterView(PositionChangedEvent);
//                _z = value;
//                _z.RegisterView(PositionChangedEvent);
//            }
        }

        public ParametricPosition(Coordinate x, Coordinate y, Coordinate z)
        {
            _x = x;
            _y = y;
            _z = z;
            x.RegisterView(()=> PositionChangedEvent?.Invoke(this,x) );
            y.RegisterView(()=> PositionChangedEvent?.Invoke(this,y) );
            z.RegisterView(()=> PositionChangedEvent?.Invoke(this,z) );
        }

        public void BakePreview()
        {
            _x.Bake();
            _y.Bake();
            _z.Bake();
        }

        public void DeleteIfPreview()
        {
            if (_x.IsPreview) _x.Delete();
            if (_y.IsPreview) _y.Delete();
            if (_z.IsPreview) _z.Delete();
        }

        public void Remove()
        {
            _x.Delete();
            _y.Delete();
            _z.Delete();
        }
        private Coordinate _x;
        private Coordinate _y;
        private Coordinate _z;
    }
}
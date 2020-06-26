using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class CoordinateSystem
    {
        //todo subscribe and update only on change
        public event Action CoordinateSystemChangedEvent;

        public Axis XAxis { get; }
        public Axis YAxis { get; }
        public Axis ZAxis { get; }
        public Anchor Anchor { get; }

        public CoordinateSystem()
        {
            XAxis = new Axis(OnAxisChanged, Vector3.right);
            YAxis = new Axis(OnAxisChanged, Vector3.up);
            ZAxis = new Axis(OnAxisChanged, Vector3.forward);

            var xAnchorCoordinate = XAxis.Anchor;
            var yAnchorCoordinate = YAxis.Anchor;
            var zAnchorCoordinate = ZAxis.Anchor;
            Anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        public (Coordinate x, Coordinate y, Coordinate z) GetParametricPosition(Vector3 position, bool asPreview,
            Sketch.KeyboardInputModel keyboardInput)
        {
            var x = keyboardInput.XInM.HasValue
                ? XAxis.AddNewMueCoordinateWithParameter(keyboardInput.XInM.Value, asPreview)
                : XAxis.GetCoordinate(position.x, asPreview);
            var y = YAxis.GetCoordinate(position.y, asPreview);
            var z = keyboardInput.ZInM.HasValue
                ? ZAxis.AddNewMueCoordinateWithParameter(keyboardInput.ZInM.Value, asPreview)
                : ZAxis.GetCoordinate(position.z, asPreview);
            return (x, y, z);
        }

        public Axis AxisThatContainsCoordinate(Coordinate c)
        {
            if (XAxis.Coordinates.Contains(c))
                return XAxis;
            if (YAxis.Coordinates.Contains(c))
                return YAxis;
            return ZAxis;
        }

        public void SetAnchorPosition(Vector3 position)
        {
            XAxis.SnapAnchorToClosestCoordinate(position.x);
            YAxis.SnapAnchorToClosestCoordinate(position.y);
            ZAxis.SnapAnchorToClosestCoordinate(position.z);
        }

        private void OnAxisChanged()
        {
            CoordinateSystemChangedEvent?.Invoke();
        }
    }
}
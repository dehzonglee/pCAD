using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class CoordinateSystem
    {
        //todo subscribe and update only on change
        public event Action CoordinateSystemChangedEvent;

        public GenericVector<Axis> Axes { get; }
        public Anchor Anchor { get; }

        public CoordinateSystem()
        {
            Axes = new GenericVector<Axis>()
            {
                X = new Axis(OnAxisChanged, Vector3.right),
                Y = new Axis(OnAxisChanged, Vector3.up),
                Z = new Axis(OnAxisChanged, Vector3.forward)
            };

            var xAnchorCoordinate = Axes[AxisID.X].Anchor;
            var yAnchorCoordinate = Axes[AxisID.Y].Anchor;
            var zAnchorCoordinate = Axes[AxisID.Z].Anchor;

            Anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        public GenericVector<Coordinate> GetParametricPosition(Vector3 position, bool asPreview,
            GenericVector<float?> keyboardInput)
        {
            var x = keyboardInput[AxisID.X].HasValue
                ? Axes[AxisID.X].AddNewMueCoordinateWithParameter(keyboardInput[AxisID.X].Value, asPreview)
                : Axes[AxisID.X].GetCoordinate(position.x, asPreview);
            var y = Axes[AxisID.Y].GetCoordinate(position.y, asPreview);
            var z = keyboardInput[AxisID.Z].HasValue
                ? Axes[AxisID.Z].AddNewMueCoordinateWithParameter(keyboardInput[AxisID.Z].Value, asPreview)
                : Axes[AxisID.Z].GetCoordinate(position.z, asPreview);
            return new GenericVector<Coordinate>() {X = x, Y = y, Z = z};
        }

        public Axis AxisThatContainsCoordinate(Coordinate c)
        {
            if (Axes[AxisID.X].Coordinates.Contains(c))
                return Axes[AxisID.X];
            if (Axes[AxisID.Y].Coordinates.Contains(c))
                return Axes[AxisID.Y];
            return Axes[AxisID.Z];
        }

        public void SetAnchorPosition(Vector3 position)
        {
            Axes[AxisID.X].SnapAnchorToClosestCoordinate(position.x);
            Axes[AxisID.Y].SnapAnchorToClosestCoordinate(position.y);
            Axes[AxisID.Z].SnapAnchorToClosestCoordinate(position.z);
        }

        private void OnAxisChanged()
        {
            CoordinateSystemChangedEvent?.Invoke();
        }
    }
}
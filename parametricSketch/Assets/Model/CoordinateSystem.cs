using UnityEngine;

namespace Model
{
    public class CoordinateSystem
    {
        public Axis XAxis { get; private set; }

        public Axis YAxis { get; private set; }

        public Axis ZAxis { get; private set; }

        public CoordinateSystem()
        {
            XAxis = new Axis();
            YAxis = new Axis();
            ZAxis = new Axis();

            var xAnchorCoordinate = XAxis.Anchor;
            var yAnchorCoordinate = YAxis.Anchor;
            var zAnchorCoordinate = ZAxis.Anchor;
            _anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        public ParametricPosition GetParametricPosition(Vector3 position)
        {
            var x = XAxis.GetCoordinate(position.x);
            var y = YAxis.GetCoordinate(position.y);
            var z = ZAxis.GetCoordinate(position.z);
            return new ParametricPosition(x, y, z);
        }

        public void RemoveParametricPosition(ParametricPosition position)
        {
        }

        public Anchor GetAnchor()
        {
            return _anchor;
        }

        public void SetAnchorPosition(Vector3 position)
        {
            XAxis.SnapAnchorToClosestCoordinate(position.x);
            YAxis.SnapAnchorToClosestCoordinate(position.y);
            ZAxis.SnapAnchorToClosestCoordinate(position.z);
        }

        private readonly Anchor _anchor;
    }
}
﻿using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class CoordinateSystem
    {
        public event Action CoordinateSystemChangedEvent;

        public Axis XAxis { get; private set; }

        public Axis YAxis { get; private set; }

        public Axis ZAxis { get; private set; }

        public CoordinateSystem()
        {
            XAxis = new Axis(OnAxisChanged, Vector3.right);
            YAxis = new Axis(OnAxisChanged, Vector3.up);
            ZAxis = new Axis(OnAxisChanged, Vector3.forward);

            var xAnchorCoordinate = XAxis.Anchor;
            var yAnchorCoordinate = YAxis.Anchor;
            var zAnchorCoordinate = ZAxis.Anchor;
            _anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        public (Coordinate x, Coordinate y, Coordinate z) GetParametricPosition(Vector3 position, bool asPreview)
        {
            var x = XAxis.GetCoordinate(position.x, asPreview);
            var y = YAxis.GetCoordinate(position.y, asPreview);
            var z = ZAxis.GetCoordinate(position.z, asPreview);
            return (x, y, z);
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

        private void OnAxisChanged()
        {
            CoordinateSystemChangedEvent?.Invoke();
        }

        private readonly Anchor _anchor;
    }
}
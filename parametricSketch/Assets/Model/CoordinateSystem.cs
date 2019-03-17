﻿using System;
using UnityEngine;

namespace Model
{
    public class CoordinateSystem
    {
        public event Action CoordinateSystemChangedEvent;

        public Axis XAxis { get; private set; }

        public Axis YAxis { get; private set; }

        public Axis ZAxis { get; private set; }

        public CoordinateSystem()
        {
            XAxis = new Axis(OnAxisChanged);
            YAxis = new Axis(OnAxisChanged);
            ZAxis = new Axis(OnAxisChanged);

            var xAnchorCoordinate = XAxis.Anchor;
            var yAnchorCoordinate = YAxis.Anchor;
            var zAnchorCoordinate = ZAxis.Anchor;
            _anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        public ParametricPosition GetParametricPosition(Vector3 position, bool asPreview)
        {
            var x = XAxis.GetCoordinate(position.x, asPreview);
            var y = YAxis.GetCoordinate(position.y, asPreview);
            var z = ZAxis.GetCoordinate(position.z, asPreview);
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

        private void OnAxisChanged()
        {
            CoordinateSystemChangedEvent?.Invoke();
        }

        private readonly Anchor _anchor;
    }
}
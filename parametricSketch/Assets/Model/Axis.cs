using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public class Axis
    {
        public event Action AnchorChangedEvent;
        public List<Coordinate> Coordinates => _coordinates;

        public Axis()
        {
            _origin = new Origin();
            _coordinates.Add(_origin);
            Anchor = new AnchorCoordinates(_origin, _origin);
        }

        public Coordinate GetCoordinate(float position)
        {
            var closestCoordinate = GetClosestCoordinateInSnapRadius(position);
            if (closestCoordinate != null)
                return closestCoordinate;

            var newCoordinate = AddNewMueCoordinate(position);
            return newCoordinate;
        }

        public AnchorCoordinates Anchor { get; }

        public void RemoveUnusedCoordinate()
        {
            var cleanList = new List<Coordinate>();
            foreach (var c in _coordinates)
            {
                if (c.IsUsed)
                    cleanList.Add(c);
            }

            _coordinates = cleanList;
        }

        public float SmallestValue => _coordinates.Select(c => c.Value).Min();

        public AnchorCoordinates SnapAnchorToClosestCoordinate(float position)
        {
            Anchor.SetPrimaryCoordinate(FindClosestCoordinate(position));
            AnchorChangedEvent?.Invoke();
            return Anchor;
        }


        private Coordinate AddNewMueCoordinate(float position)
        {
            var delta = position - Anchor.PrimaryCoordinate.Value;
            var newCoordinate = new Mue(Anchor.PrimaryCoordinate, delta, OnCoordinateDeprecated);
            _coordinates.Add(newCoordinate);
            return newCoordinate;
        }

        private void OnCoordinateDeprecated(Coordinate deprecatedCoordinate)
        {
            _coordinates.Remove(deprecatedCoordinate);
        }

        private Coordinate GetClosestCoordinateInSnapRadius(float position)
        {
            var closestCoordinate = FindClosestCoordinate(position);
            var distanceToClosestCoordinate = Mathf.Abs(position - closestCoordinate.Value);
            var distanceToLambdaCoordinate = DistanceToLambdaCoordinate(position);

            if (distanceToClosestCoordinate > SNAP_RADIUS && distanceToLambdaCoordinate > SNAP_RADIUS)
                return null;

            if (distanceToClosestCoordinate < distanceToLambdaCoordinate)
                return closestCoordinate;

            return AddLambdaCoordinateBetweenAnchors();
        }

        private Lambda AddLambdaCoordinateBetweenAnchors()
        {
            var newLambda = new Lambda(
                Anchor.PrimaryCoordinate,
                Anchor.SecondaryCoordinate,
                0.5f,
                OnCoordinateDeprecated
            );
            _coordinates.Add(newLambda);
            return newLambda;
        }

        private float DistanceToLambdaCoordinate(float position)
        {
            //todo: quick fix to catch undefined lambda if anchors match
            if (Anchor.AnchorsMatch) return float.PositiveInfinity;

            var lambdaPosition = (Anchor.PrimaryCoordinate.Value + Anchor.SecondaryCoordinate.Value) / 2f;
            return Mathf.Abs(position - lambdaPosition);
        }

        private Coordinate FindClosestCoordinate(float position)
        {
            Coordinate closestCoordinate = _origin;
            var closestDistance = Mathf.Abs(_origin.Value - position);
            foreach (var c in _coordinates)
            {
                var distance = Mathf.Abs(c.Value - position);
                if (distance < closestDistance)
                {
                    closestCoordinate = c;
                    closestDistance = distance;
                }
            }

            return closestCoordinate;
        }

        private const float SNAP_RADIUS = 0.01f;
        private List<Coordinate> _coordinates = new List<Coordinate>();
        private readonly Coordinate _origin;
    }
}
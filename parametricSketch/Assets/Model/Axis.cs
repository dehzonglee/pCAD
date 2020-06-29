using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class Axis
    {
        private event Action _axisChangedEvent;
        public List<Coordinate> Coordinates { get; private set; } = new List<Coordinate>();
        public Vector3 Direction;

        public Axis(Action axisChangedCallback, Vector3 direction)
        {
            _origin = new Origin();
            Direction = direction;
            Coordinates.Add(_origin);
            Anchor = new AnchorCoordinates(_origin);
            _axisChangedEvent += axisChangedCallback;
        }

        public Coordinate GetCoordinate(float position, bool isPreview)
        {
            var closestCoordinate = GetClosestCoordinateInSnapRadius(position, isPreview);
            return closestCoordinate ?? AddNewMueCoordinate(position, isPreview);
        }

        public AnchorCoordinates Anchor { get; }

        public float SmallestValue => Coordinates.Select(c => c.Value).Min();

        public AnchorCoordinates SnapAnchorToClosestCoordinate(float position)
        {
            Anchor.SetPrimaryCoordinate(FindClosestCoordinate(position));
            return Anchor;
        }

        private Coordinate AddNewMueCoordinate(float position, bool asPreview)
        {
            var delta = position - Anchor.PrimaryCoordinate.Value;
            var pointsInNegativeDirection = delta < 0f;
            if (pointsInNegativeDirection)
                delta *= -1f;

            var newCoordinate = new Mue(
                Anchor.PrimaryCoordinate,
                delta,
                pointsInNegativeDirection,
                OnCoordinateDeleted,
                OnCoordinateChanged,
                asPreview
            );
            Coordinates.Add(newCoordinate);
            return newCoordinate;
        }

        public Coordinate AddNewMueCoordinateWithParameterValue(float parameterValue, bool pointsInNegativeDirection,
            bool asPreview)
        {
            Debug.Log($"{parameterValue} , {pointsInNegativeDirection}");
            var newCoordinate = new Mue(
                Anchor.PrimaryCoordinate,
                parameterValue,
                pointsInNegativeDirection, 
                OnCoordinateDeleted,
                OnCoordinateChanged,
                asPreview
            );
            Coordinates.Add(newCoordinate);
            return newCoordinate;
        }


        public Coordinate AddNewMueCoordinateWithParameterReference(MueParameter parameterReference,
            bool pointsInNegativeDirection, bool asPreview)
        {
            var newCoordinate =
                new Mue(Anchor.PrimaryCoordinate, parameterReference, pointsInNegativeDirection, OnCoordinateDeleted,
                    OnCoordinateChanged, asPreview);
            Coordinates.Add(newCoordinate);
            return newCoordinate;
        }

        private void OnCoordinateChanged()
        {
            _axisChangedEvent?.Invoke();
        }

        private void OnCoordinateDeleted(Coordinate deletedCoordinate)
        {
            Coordinates.Remove(deletedCoordinate);
            if (Anchor.PrimaryCoordinate == deletedCoordinate) Anchor.ResetPrimaryCoordinate();
            if (Anchor.SecondaryCoordinate == deletedCoordinate) Anchor.ResetSecondaryCoordinate();
        }

        private Coordinate GetClosestCoordinateInSnapRadius(float position, bool isPreview)
        {
            var closestCoordinate = FindClosestCoordinate(position);
            var distanceToClosestCoordinate = Mathf.Abs(position - closestCoordinate.Value);
            var distanceToLambdaCoordinate = DistanceToLambdaCoordinate(position);

            if (distanceToClosestCoordinate > SNAP_RADIUS && distanceToLambdaCoordinate > SNAP_RADIUS)
                return null;

            if (distanceToClosestCoordinate < distanceToLambdaCoordinate)
                return closestCoordinate;

            return AddLambdaCoordinateBetweenAnchors(isPreview);
        }

        private Lambda AddLambdaCoordinateBetweenAnchors(bool isPreview)
        {
            var newLambda = new Lambda(
                Anchor.PrimaryCoordinate,
                Anchor.SecondaryCoordinate,
                0.5f,
                OnCoordinateDeleted,
                OnCoordinateChanged,
                isPreview
            );
            Coordinates.Add(newLambda);
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
            foreach (var c in Coordinates)
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
        private readonly Origin _origin;
    }
}
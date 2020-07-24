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
        public List<Coordinate> Coordinates  = new List<Coordinate>();
        public Vector3 Direction;
        public AnchorCoordinates Anchor { get; }
        public float SmallestValue => Coordinates.Select(c => c.Value).Min();

        public Axis(Action axisChangedCallback, Vector3 direction, float originPosition)
        {
            _origin = new Origin(originPosition);
            Direction = direction;
            Coordinates.Add(_origin);
            Anchor = new AnchorCoordinates(_origin);
            _axisChangedEvent += axisChangedCallback;
        }

        public void SnapAnchorToClosestCoordinate(float position)
        {
            Anchor.SetPrimaryCoordinate(FindClosestCoordinate(position));
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

        public Coordinate AddNewMueCoordinateWithParameterReference(Parameter parameterReference,
            bool pointsInNegativeDirection, bool asPreview)
        {
            var newCoordinate =
                new Mue(Anchor.PrimaryCoordinate, parameterReference, pointsInNegativeDirection, OnCoordinateDeleted,
                    OnCoordinateChanged, asPreview);
            Coordinates.Add(newCoordinate);
            return newCoordinate;
        }

        public Coordinate AddNewMueCoordinate(float position, bool asPreview)
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

        public Coordinate AddNewMueCoordinate(Parameter parameter, bool pointsInNegativeDirection, bool asPreview)
        {
            var newCoordinate = new Mue(
                Anchor.PrimaryCoordinate,
                parameter,
                pointsInNegativeDirection,
                OnCoordinateDeleted,
                OnCoordinateChanged,
                asPreview
            );
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

        public Coordinate TryToSnapToExistingCoordinate(float position, bool isPreview)
        {
            var closestCoordinate = FindClosestCoordinate(position);
            var distanceToClosestCoordinate = Mathf.Abs(position - closestCoordinate.Value);
            var distanceToPotentialLambdaCoordinate = DistancePotentialToLambdaCoordinate(position);

            if (distanceToClosestCoordinate > SnapRadius && distanceToPotentialLambdaCoordinate > SnapRadius)
                return null;

            if (distanceToClosestCoordinate < distanceToPotentialLambdaCoordinate + Epsilon)
                return closestCoordinate;

            return AddLambdaCoordinateBetweenAnchors(isPreview);
        }


        public (Parameter parameter, bool pointsInNegativeDirection)? TryToSnapToExistingParameter(
            float parameterValue, List<Parameter> allParameters)
        {
            if (Coordinates.Count == 0)
                return null;

            var parametersWithDistance = allParameters.Select(p => GenerateDistanceToValue(p, parameterValue));

            (Parameter p, float distance, bool pointsInNegativeDirection)? candidate = null;
            foreach (var p in parametersWithDistance)
            {
                if (!candidate.HasValue)
                {
                    candidate = p;
                    continue;
                }

                if (p.distance < candidate.Value.distance)
                    candidate = p;
            }

            if (!candidate.HasValue || candidate.Value.distance > SnapRadius)
                return null;

            return (candidate.Value.p, candidate.Value.pointsInNegativeDirection);

            ( Parameter p, float distance, bool pointsInNegativeDirection) GenerateDistanceToValue(Parameter p,
                float inputValue)
            {
                var distanceToParameter = Mathf.Abs(inputValue - p.Value);
                var distanceToNegativeParameter = Mathf.Abs(inputValue + p.Value);

                return distanceToParameter < distanceToNegativeParameter
                    ? (p, distanceToParameter, false)
                    : (p, distanceToNegativeParameter, true);
            }
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

        private float DistancePotentialToLambdaCoordinate(float position)
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

        private const float SnapRadius = 0.01f;
        private readonly Origin _origin;
        private const float Epsilon = 0.0001f;
    }
}

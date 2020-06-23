using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Model;
using UI;
using UnityEngine;

namespace Interaction
{
    public class CoordinateManipulation : MonoBehaviour
    {
        [CanBeNull]
        public static Coordinate TryToHitCoordinate(
            IScreenDistanceCalculatorProvider screenDistanceProviders, Vector2 screenPos)
        {
            var (x,y,z) = screenDistanceProviders.GetProvidersForAxis();
            Coordinate hitCoordinate = null;
            var radius = SnapRadius;
            
            var closestOnX = GetClosestCoordinateOnAxisWithinSnapRadius(x, screenPos,radius);
            if (closestOnX != null)
            {
                hitCoordinate = closestOnX.Value.Coordinate;
                radius = closestOnX.Value.ScreenDistanceToCoordinate;
            }
            
            var closestOnY = GetClosestCoordinateOnAxisWithinSnapRadius(y, screenPos,radius);
            if (closestOnY!= null)
            {
                hitCoordinate = closestOnY.Value.Coordinate;
                radius = closestOnY.Value.ScreenDistanceToCoordinate;
            }
            
            var closestOnZ = GetClosestCoordinateOnAxisWithinSnapRadius(z, screenPos,radius);
            if (closestOnZ != null)
            {
                hitCoordinate = closestOnZ.Value.Coordinate;
            }
            return hitCoordinate;
        }


        [CanBeNull]
        private static ScreenDistance? GetClosestCoordinateOnAxisWithinSnapRadius(
            IScreenDistanceCalculator screenDistanceProviderForAxis, Vector2 screenPosition, float snapRadius)
        {
            var distances = screenDistanceProviderForAxis.GetAllDistancesToCoordinateUIs(screenPosition);
            if (distances.Count == 0)
                return null;

            var closestCoordinate = distances
                .OrderBy(distancesAndCoordinate => distancesAndCoordinate.ScreenDistanceToCoordinate).ToList()[0];

            if (closestCoordinate.ScreenDistanceToCoordinate > snapRadius)
                return null;

            return closestCoordinate;
        }

        public interface IScreenDistanceCalculator
        {
            List<ScreenDistance> GetAllDistancesToCoordinateUIs(Vector2 screenPos);
        }

        public interface IScreenDistanceCalculatorProvider
        {
            (IScreenDistanceCalculator x, IScreenDistanceCalculator y,
                IScreenDistanceCalculator z) GetProvidersForAxis();
        }

        public struct ScreenDistance
        {
            public Coordinate Coordinate;
            public float ScreenDistanceToCoordinate;
        }

        private const float SnapRadius = 10000f;
    }
}
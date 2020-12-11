using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Model;
using UI;
using UnityEngine;

public class CoordinateManipulation : MonoBehaviour
{
    public static (Coordinate coordinate, Vec.AxisID axis)? TryGetCoordinateAtPosition(
        CoordinateSystemUI coordinateSystemUI)
    {
        var dragged = TryToHitCoordinate(
            coordinateSystemUI,
            new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            - 0.5f * new Vector2(Screen.width, Screen.height)
        );

        return dragged;
    }

    public static (float value, bool inOppositeDirection) UpdateDrag(Coordinate draggedCoordinate,
        Axis axisOfDraggedCoordinate)
    {
        // mouse position to parameter
        var pos = MouseInput.RaycastPosition;
        var worldPositionAsUnityVector = new Vector3(pos.X, pos.Y, pos.Z);


        var deltaToOldValue = Vector3.Dot(worldPositionAsUnityVector, axisOfDraggedCoordinate.Direction) -
                              draggedCoordinate.Value;

        //todo take lambdas into consideration!!!

        var multiplier = CalculateMultiplierAlongPathRecursive(draggedCoordinate, draggedCoordinate.Parameter);

        float CalculateMultiplierAlongPathRecursive(Coordinate currentNode, Parameter parameter)
        {
            if (currentNode.GetType() == typeof(Origin))
            {
                return 0f;
            }

            if (currentNode.GetType() == typeof(Lambda))
            {
                var lambdaCoordinate = currentNode as Lambda;
                Debug.Assert(lambdaCoordinate != null, nameof(lambdaCoordinate) + " != null");
                var lambda = lambdaCoordinate.Parameter.Value;
                var p0 = lambdaCoordinate.Parents[0];
                var p1 = lambdaCoordinate.Parents[1];
                return (1f - lambda) * CalculateMultiplierAlongPathRecursive(p0, parameter)
                       + lambda * CalculateMultiplierAlongPathRecursive(p1, parameter);
            }

//            if (currentNode.GetType() == typeof(Mue))
            var mueCoordinate = currentNode as Mue;
            Debug.Assert(mueCoordinate != null, nameof(mueCoordinate) + " != null");

            if (mueCoordinate.Parameter != parameter)
                return CalculateMultiplierAlongPathRecursive(mueCoordinate.Parents[0], parameter);

            var weightForThisCoordinate = mueCoordinate.PointsInNegativeDirection ? -1f : 1f;
            return weightForThisCoordinate +
                   CalculateMultiplierAlongPathRecursive(mueCoordinate.Parents[0], parameter);
        }


        var mue = draggedCoordinate as Mue;
        if (mue.PointsInNegativeDirection)
            multiplier *= -1f;

        if (multiplier == 0)
            return (draggedCoordinate.Parameter.Value, mue.PointsInNegativeDirection);

        var deltaThatTakesOtherCoordinatesIntoConsideration = deltaToOldValue / multiplier;
        var output = mue.PointsInNegativeDirection
            ? (value: draggedCoordinate.Parameter.Value - deltaThatTakesOtherCoordinatesIntoConsideration,
                inoppositeDirection: true)
            : (value: draggedCoordinate.Parameter.Value + deltaThatTakesOtherCoordinatesIntoConsideration,
                inoppositeDirection: false);

        if (output.value < Epsilon)
            output.value = Epsilon;

        return output;
    }

    private const float Epsilon = 0.001f;

    private static float MousePositionToParameter(Vec<float> mouseWorldPosition, Coordinate coordinate,
        Axis axis)
    {
        var worldPositionAsUnityVector = new Vector3(mouseWorldPosition.X, mouseWorldPosition.Y, mouseWorldPosition.Z);
        return Vector3.Dot(worldPositionAsUnityVector, axis.Direction) - coordinate.ParentValue;
    }

    [CanBeNull]
    private static (Coordinate, Vec.AxisID)? TryToHitCoordinate(
        IScreenDistanceCalculatorProvider screenDistanceProviders, Vector2 screenPos)
    {
        var providers = screenDistanceProviders.GetProvidersForAxis();
        (Coordinate hitCoordinate, Vec.AxisID axis)? hitResult = null;
        var radius = SnapRadius;

        foreach (var a in Vec.XYZ)
        {
            var closestOnAxis = GetClosestCoordinateOnAxisWithinSnapRadius(providers[a], screenPos, radius);
            if (closestOnAxis == null)
                continue;
            hitResult = (closestOnAxis.Value.Coordinate, a);
            radius = closestOnAxis.Value.ScreenDistanceToCoordinate;
        }

        return hitResult;
    }


    [CanBeNull]
    private static ScreenDistance? GetClosestCoordinateOnAxisWithinSnapRadius(
        IScreenDistanceCalculator screenDistanceProviderForAxis, Vector2 screenPosition, float snapRadius)
    {
        var distances = screenDistanceProviderForAxis.GetAllDistancesToCoordinateUIs(screenPosition);
        if (distances.Count == 0)
            return null;

        var orderedCoordinates = distances
            .Where(d => !(d.Coordinate is Mue && d.Coordinate.IsCurrentlyDrawn))
            .OrderBy(distancesAndCoordinate => distancesAndCoordinate.ScreenDistanceToCoordinate)
            .ToList();

        if (!orderedCoordinates.Any())
            return null;

        var closestCoordinate = orderedCoordinates[0];
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
        Vec<IScreenDistanceCalculator> GetProvidersForAxis();
    }

    public struct ScreenDistance
    {
        public Coordinate Coordinate;
        public float ScreenDistanceToCoordinate;
    }

    private const float SnapRadius = 10f;
}
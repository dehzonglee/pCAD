using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Model;
using UI;
using UnityEngine;

public class CoordinateManipulation : MonoBehaviour
{
    public static Coordinate TryStartDrag(CoordinateSystemUI coordinateSystemUI)
    {
        var dragged = TryToHitCoordinate(coordinateSystemUI,
            new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            - 0.5f * new Vector2(Screen.width, Screen.height));

        return dragged;
    }

    public static (float value, bool inOppositeDirection) UpdateDrag(Coordinate draggedCoordinate,
        Axis axisOfDraggedCoordinate)
    {
        // mouse position to parameter
        var pos = MouseInput.RaycastPosition;
        var worldPositionAsUnityVector = new Vector3(pos.X, pos.Y, pos.Z);

        var delta = Vector3.Dot(worldPositionAsUnityVector, axisOfDraggedCoordinate.Direction) -
                    draggedCoordinate.ParentValue;

        var isInOppositeDirection = delta < 0f;
        var value = isInOppositeDirection ? -delta : delta;
        return (value, isInOppositeDirection);
    }

    private static float MousePositionToParameter(GenericVector<float> mouseWorldPosition, Coordinate coordinate,
        Axis axis)
    {
        var worldPositionAsUnityVector = new Vector3(mouseWorldPosition.X, mouseWorldPosition.Y, mouseWorldPosition.Z);
        return Vector3.Dot(worldPositionAsUnityVector, axis.Direction) - coordinate.ParentValue;
    }

    [CanBeNull]
    private static Coordinate TryToHitCoordinate(
        IScreenDistanceCalculatorProvider screenDistanceProviders, Vector2 screenPos)
    {
        var providers = screenDistanceProviders.GetProvidersForAxis();
        Coordinate hitCoordinate = null;
        var radius = SnapRadius;

        foreach (var axis in new[] {AxisID.X, AxisID.Y, AxisID.Z})
        {
            var closestOnAxis = GetClosestCoordinateOnAxisWithinSnapRadius(providers[axis], screenPos, radius);
            if (closestOnAxis == null)
                continue;
            hitCoordinate = closestOnAxis.Value.Coordinate;
            radius = closestOnAxis.Value.ScreenDistanceToCoordinate;
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
        GenericVector<IScreenDistanceCalculator> GetProvidersForAxis();
    }

    public struct ScreenDistance
    {
        public Coordinate Coordinate;
        public float ScreenDistanceToCoordinate;
    }

    private const float SnapRadius = 10000f;
}
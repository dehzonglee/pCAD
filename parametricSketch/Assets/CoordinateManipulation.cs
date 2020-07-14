﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using Model;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Debug = System.Diagnostics.Debug;

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

    public static (float value, bool inOppositeDirection) NewUpdateDrag(Coordinate draggedCoordinate,
        Axis axisOfDraggedCoordinate)
    {
        // mouse position to parameter
        var pos = MouseInput.RaycastPosition;
        var worldPositionAsUnityVector = new Vector3(pos.X, pos.Y, pos.Z);


        var deltaToOldValue = Vector3.Dot(worldPositionAsUnityVector, axisOfDraggedCoordinate.Direction) -
                              draggedCoordinate.Value;

        //todo take lambdas into consideration!!!

        var multiplier = CalculateMultiplierAlongPathRecursive(draggedCoordinate,draggedCoordinate.Parameter);
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
            else
            {
                var mueCoordinate = currentNode as Mue;
                Debug.Assert(mueCoordinate != null, nameof(mueCoordinate) + " != null");

                if (mueCoordinate.Parameter != parameter)
                    return CalculateMultiplierAlongPathRecursive(mueCoordinate.Parents[0], parameter);
                
                var weightForThisCoordinate = mueCoordinate.PointsInNegativeDirection ? -1f : 1f;
                return weightForThisCoordinate +
                       CalculateMultiplierAlongPathRecursive(mueCoordinate.Parents[0], parameter);
            }
        }


        var mue = draggedCoordinate as Mue;
        if (mue.PointsInNegativeDirection)
            multiplier *= -1f;

        if (multiplier == 0)
            return (draggedCoordinate.Parameter.Value, mue.PointsInNegativeDirection);

        var deltaThatTakesOtherCoordinatesIntoConsideration = deltaToOldValue / multiplier;
        return mue.PointsInNegativeDirection
            ? (draggedCoordinate.Parameter.Value - deltaThatTakesOtherCoordinatesIntoConsideration, true)
            : (draggedCoordinate.Parameter.Value + deltaThatTakesOtherCoordinatesIntoConsideration, false);
    }

    private static float MousePositionToParameter(Vec<float> mouseWorldPosition, Coordinate coordinate,
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

        foreach (var a in Vec.XYZ)
        {
            var closestOnAxis = GetClosestCoordinateOnAxisWithinSnapRadius(providers[a], screenPos, radius);
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
        Vec<IScreenDistanceCalculator> GetProvidersForAxis();
    }

    public struct ScreenDistance
    {
        public Coordinate Coordinate;
        public float ScreenDistanceToCoordinate;
    }

    private const float SnapRadius = 10000f;
}
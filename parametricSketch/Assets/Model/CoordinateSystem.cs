using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class CoordinateSystem
    {
        //todo subscribe and update only on change
        public event Action CoordinateSystemChangedEvent;

        public Vec<Axis> Axes { get; }
        public Anchor Anchor { get; }

        public CoordinateSystem(Vec<float> mousePositionAsOrigin)
        {
            Axes = new Vec<Axis>
            {
                X = new Axis(OnAxisChanged, Vector3.right, mousePositionAsOrigin.X),
                Y = new Axis(OnAxisChanged, Vector3.up, mousePositionAsOrigin.Y),
                Z = new Axis(OnAxisChanged, Vector3.forward, mousePositionAsOrigin.Z)
            };


            var xAnchorCoordinate = Axes[Vec.AxisID.X].Anchor;
            var yAnchorCoordinate = Axes[Vec.AxisID.Y].Anchor;
            var zAnchorCoordinate = Axes[Vec.AxisID.Z].Anchor;

            Anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        /// <summary>
        /// Get a Vector of Coordinates for a given drawing context
        /// </summary>
        /// <param name="position"></param>
        /// <param name="distancesToAnchor"></param>
        /// <param name="asPreview"></param>
        /// <param name="keyboardInput"></param>
        /// <returns></returns>
        public Vec<Coordinate> GetParametricPosition(Vec<float> position, Vec<float> distancesToAnchor, bool asPreview,
            KeyboardInput.Model keyboardInput)
        {
            SnappedCoordinate = new Vec<Coordinate>();
            SnappedParameter = new Vec<Parameter>();

            var output = new Vec<Coordinate>();
            foreach (var a in Vec.XYZ)
            {
                // a parameter is referenced in the keyboard input
                if (keyboardInput?.ParameterReferences[a] != null)
                {
                    output[a] = Axes[a].AddNewMueCoordinateWithParameterReference(
                        keyboardInput.ParameterReferences[a],
                        keyboardInput.IsDirectionNegative[a],
                        asPreview
                    );
                    continue;
                }

                // a dimension is set in the keyboard input
                if (keyboardInput?.DimensionInput[a] != null)
                {
                    output[a] = Axes[a].AddNewMueCoordinateWithParameterValue(
                        keyboardInput.DimensionInput[a].InM,
                        keyboardInput.IsDirectionNegative[a],
                        asPreview
                    );
                    continue;
                }

                // try to snap to an existing coordinate
                var snappedCoordinate = Axes[a].TryToSnapToExistingCoordinate(position[a], asPreview);
                if (snappedCoordinate != null)
                {
                    output[a] = snappedCoordinate;
                    SnappedCoordinate[a] = snappedCoordinate;
                    continue;
                }

                // try to snap to an existing parameter
                var parameterSnap = Axes[a].TryToSnapToExistingParameter(distancesToAnchor[a], GetAllParameters());
                if (parameterSnap != null)
                {
                    output[a] = Axes[a].AddNewMueCoordinate(
                        parameterSnap.Value.parameter,
                        parameterSnap.Value.pointsInNegativeDirection,
                        asPreview
                    );
                    SnappedParameter[a] = parameterSnap.Value.parameter;
                    continue;
                }

                // create new coordinate
                output[a] = Axes[a].AddNewMueCoordinate(position[a], asPreview);
            }

            return output;
        }

        public Vec<Parameter> SnappedParameter = new Vec<Parameter>();
        public Vec<Coordinate> SnappedCoordinate = new Vec<Coordinate>();

        public Axis AxisThatContainsCoordinate(Coordinate c)
        {
            foreach (Axis a in Axes)
            {
                if (a.Coordinates.Contains(c))
                    return a;
            }

            Debug.LogError($"No Axis contains the coordiante {c}");
            return null;
        }

        public void SetAnchorPosition(Vec<float> position)
        {
            foreach (var a in Vec.XYZ)
            {
                Axes[a].SnapAnchorToClosestCoordinate(position[a]);
            }
        }

        private void OnAxisChanged()
        {
            CoordinateSystemChangedEvent?.Invoke();
        }

        public List<Mue> GetAllMuesThatUseParameter(Parameter parameter)
        {
            var output = new List<Mue>();
            foreach (var a in Vec.XYZ)
            {
                output.AddRange(
                    Axes[a].Coordinates
                        .Where(c => c.GetType() == typeof(Mue))
                        .Select(c => c as Mue)
                        .Where(c => c.Parameter == parameter)
                );
            }

            return output;
        }

        public List<Parameter> GetAllParameters()
        {
            var output = new List<Parameter>();
            foreach (var a in Vec.XYZ)
            {
                output.AddRange(
                    Axes[a].Coordinates
                        .Where(c => c.GetType() != typeof(Origin))
//                        .Where(c => !c.IsCurrentlyDrawn)
                        .Select(c => c.Parameter)
                );
            }

            var distinctList = new List<Parameter>();
            foreach (var parameter in output)
            {
                if (distinctList.Any(p => p.ID == parameter.ID))
                    continue;
                distinctList.Add(parameter);
            }

            return distinctList.OrderBy(p => p.Value).ToList();
        }
    }
}
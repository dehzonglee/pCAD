using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

namespace Model
{
    [Serializable]
    public class CoordinateSystem
    {
        //todo subscribe and update only on change
        public event Action CoordinateSystemChangedEvent;

        public GenericVector<Axis> Axes { get; }
        public Anchor Anchor { get; }

        public CoordinateSystem()
        {
            Axes = new GenericVector<Axis>()
            {
                X = new Axis(OnAxisChanged, Vector3.right),
                Y = new Axis(OnAxisChanged, Vector3.up),
                Z = new Axis(OnAxisChanged, Vector3.forward)
            };

            var xAnchorCoordinate = Axes[AxisID.X].Anchor;
            var yAnchorCoordinate = Axes[AxisID.Y].Anchor;
            var zAnchorCoordinate = Axes[AxisID.Z].Anchor;

            Anchor = new Anchor(xAnchorCoordinate, yAnchorCoordinate, zAnchorCoordinate);
        }

        public GenericVector<Coordinate> GetParametricPosition(GenericVector<float> position, bool asPreview,
            GenericVector<float?> keyboardInputValues, GenericVector<MueParameter> keyboardInputParameters, GenericVector<bool> keyboardInputNegativeDirection)
        {
            var output = new GenericVector<Coordinate>();
            foreach (var a in new[] {AxisID.X, AxisID.Y, AxisID.Z})
            {
                if (keyboardInputParameters[a] != null)
                {
                    output[a] = Axes[a]
                        .AddNewMueCoordinateWithParameterReference(keyboardInputParameters[a],keyboardInputNegativeDirection[a], asPreview);
                }
                else if (keyboardInputValues[a].HasValue)
                    output[a] = Axes[a].AddNewMueCoordinateWithParameterValue(keyboardInputValues[a].Value,keyboardInputNegativeDirection[a], asPreview);
                else
                    output[a] = Axes[a].GetCoordinate(position[a], asPreview);
            }

            return output;
        }

        public Axis AxisThatContainsCoordinate(Coordinate c)
        {
            if (Axes[AxisID.X].Coordinates.Contains(c))
                return Axes[AxisID.X];
            if (Axes[AxisID.Y].Coordinates.Contains(c))
                return Axes[AxisID.Y];
            return Axes[AxisID.Z];
        }

        public void SetAnchorPosition(GenericVector<float> position)
        {
            Axes[AxisID.X].SnapAnchorToClosestCoordinate(position.X);
            Axes[AxisID.Y].SnapAnchorToClosestCoordinate(position.Y);
            Axes[AxisID.Z].SnapAnchorToClosestCoordinate(position.Z);
        }

        private void OnAxisChanged()
        {
            CoordinateSystemChangedEvent?.Invoke();
        }

        public List<MueParameter> GetAllParameters()
        {
            var output = new List<MueParameter>();
            foreach (var axisID in new[] {AxisID.X, AxisID.Y, AxisID.Z})
            {
                var axis = Axes[axisID];
                output.AddRange(
                    axis.Coordinates
                        .Where(c => c.GetType() != typeof(Origin))
                        .Where(c => !c.IsPreview)
                        .Select(c => c.Parameter)
                        );
            }

            var distinctList = new List<MueParameter>();
            foreach (var parameter in output)
            {
                if(distinctList.Any(p=>p.ID == parameter.ID))
                    continue;
                distinctList.Add(parameter);
            }
            
            return distinctList.OrderBy(p => p.Value).ToList();
        }
    }
}
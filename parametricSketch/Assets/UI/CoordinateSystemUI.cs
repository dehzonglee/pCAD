using System.Security.AccessControl;
using Model;
using UnityEngine;

namespace UI
{
    public class CoordinateSystemUI : MonoBehaviour, CoordinateManipulation.IScreenDistanceCalculatorProvider
    {
        [SerializeField] AxisUI _axisUIPrefab;
        [SerializeField] AnchorUI _anchorUIPrefab;

        public void Initialize()
        {
            var xAxisUI = Instantiate(_axisUIPrefab, transform);
            xAxisUI.Initialize(_embedding[AxisID.X], "xAxisUI");

            var yAxisUI = Instantiate(_axisUIPrefab, transform);
            yAxisUI.Initialize(_embedding[AxisID.Y], "yAxisUI");

            var zAxisUI = Instantiate(_axisUIPrefab, transform);
            zAxisUI.Initialize(_embedding[AxisID.Z], "zAxisUI");

            _axisUIs = new GenericVector<AxisUI>()
            {
                X = xAxisUI,
                Y = yAxisUI,
                Z = zAxisUI,
            };

            _anchorUI = Instantiate(_anchorUIPrefab, transform);
        }

        public void UpdateUI(CoordinateSystem cs, CoordinateUIStyle coordinateUIStyle,
            GenericVector<float?> inputVector, AxisID? activeAxisInKeyboardInput)
        {
            foreach (var axis in new[] {AxisID.X, AxisID.Y, AxisID.Z})
            {
                _axisUIs[axis].UpdateCoordinateUIs(
                    cs.Axes[axis],
                    _embedding[GetOrthogonalAxis(axis)],
                    cs.Axes[GetOrthogonalAxis(axis)].SmallestValue,
                    coordinateUIStyle,
                    inputVector[axis],
                    activeAxisInKeyboardInput == axis);
            }

            _anchorUI.UpdateUI(cs.Anchor, coordinateUIStyle.Anchor);
        }

        private static AxisID GetOrthogonalAxis(AxisID axis)
        {
            switch (axis)
            {
                case AxisID.X:
                    return AxisID.Z;
                case AxisID.Y:
                    return AxisID.Y;
                case AxisID.Z:
                default:
                    return AxisID.X;
            }
        }

        private GenericVector<AxisUI> _axisUIs;

        private AnchorUI _anchorUI;

        private readonly GenericVector<Vector3> _embedding = new GenericVector<Vector3>()
        {
            X = Vector3.right,
            Y = Vector3.up,
            Z = Vector3.forward,
        };

        public GenericVector<CoordinateManipulation.IScreenDistanceCalculator> GetProvidersForAxis()
        {
            return new GenericVector<CoordinateManipulation.IScreenDistanceCalculator>()
            {
                X = _axisUIs.X,
                Y = _axisUIs.Y,
                Z = _axisUIs.Z,
            };
        }
    }
}
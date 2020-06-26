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
            _xAxisUI = Instantiate(_axisUIPrefab, transform);
            _xAxisUI.Initialize(Vector3.right, "xAxisUI");

            _yAxisUI = Instantiate(_axisUIPrefab, transform);
            _yAxisUI.Initialize(Vector3.up, "yAxisUI");

            _zAxisUI = Instantiate(_axisUIPrefab, transform);
            _zAxisUI.Initialize(Vector3.forward, "zAxisUI");

            _anchorUI = Instantiate(_anchorUIPrefab, transform);
        }

        public void UpdateUI(CoordinateSystem cs, CoordinateUIStyle coordinateUIStyle,
            Axis.GenericVector<float?> inputVector,Axis.GenericVector<bool> keyboardInoutSelection)
        {
            _xAxisUI.UpdateCoordinateUIs(cs.XAxis, Vector3.forward, GetOrthogonalAxis(cs, Dimensions.X).SmallestValue,
                coordinateUIStyle, inputVector.X,keyboardInoutSelection.X);
            _yAxisUI.UpdateCoordinateUIs(cs.YAxis, Vector3.up, GetOrthogonalAxis(cs, Dimensions.Y).SmallestValue,
                coordinateUIStyle, inputVector.Y,keyboardInoutSelection.Y);
            _zAxisUI.UpdateCoordinateUIs(cs.ZAxis, Vector3.right, GetOrthogonalAxis(cs, Dimensions.Z).SmallestValue,
                coordinateUIStyle, inputVector.Z,keyboardInoutSelection.Z);
            _anchorUI.UpdateUI(cs.Anchor, coordinateUIStyle.Anchor);
        }

        private static Axis GetOrthogonalAxis(CoordinateSystem cs, int dimension)
        {
            switch (dimension)
            {
                case Dimensions.X:
                    return cs.ZAxis;
                case Dimensions.Y:
                    return cs.YAxis;
                case Dimensions.Z:
                default:
                    return cs.XAxis;
            }
        }

        private AxisUI _xAxisUI;
        private AxisUI _yAxisUI;
        private AxisUI _zAxisUI;
        private AnchorUI _anchorUI;

        public (CoordinateManipulation.IScreenDistanceCalculator x, CoordinateManipulation.IScreenDistanceCalculator y,
            CoordinateManipulation.IScreenDistanceCalculator z) GetProvidersForAxis()
        {
            return (_xAxisUI, _yAxisUI, _zAxisUI);
        }
    }
}
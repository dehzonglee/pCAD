using Model;
using UnityEngine;

namespace UI
{
    public class CoordinateSystemUI : MonoBehaviour, CoordinateManipulation.IScreenDistanceCalculatorProvider
    {
        Vec<CoordinateManipulation.IScreenDistanceCalculator> CoordinateManipulation.IScreenDistanceCalculatorProvider.
            GetProvidersForAxis() =>
            new Vec<CoordinateManipulation.IScreenDistanceCalculator>(a => _axisUIs[a]);

        [SerializeField] AxisUI _axisUIPrefab;
        [SerializeField] AnchorUI _anchorUIPrefab;

        public void Initialize()
        {
            _axisUIs = new Vec<AxisUI>(a =>
            {
                var ui = Instantiate(_axisUIPrefab, transform);
                ui.Initialize(_embedding[a], $"{a} - AxisUI");
                return ui;
            });

            _anchorUI = Instantiate(_anchorUIPrefab, transform);
        }

        public void UpdateUI(CoordinateSystem cs, CoordinateUIStyle coordinateUIStyle,
            KeyboardInput.Model keyboardInput)
        {
            foreach (var a in Vec.XYZ)
            {
                var referencedParameter = keyboardInput.ActiveAxis.HasValue
                    ? keyboardInput.ParameterReferences[keyboardInput.ActiveAxis.Value]
                    : null;

                if (referencedParameter == null)
                {
                    
//                    referencedParameter = 
                }
                
                _axisUIs[a].UpdateCoordinateUIs(
                    cs.Axes[a],
                    _embedding[Vec.GetOrthogonalAxis(a)],
                    cs.Axes[Vec.GetOrthogonalAxis(a)].SmallestValue,
                    coordinateUIStyle,
                    referencedParameter,
                    keyboardInput.ActiveAxis == a);
            }

            _anchorUI.UpdateUI(cs.Anchor, coordinateUIStyle.Anchor);
        }

        private Vec<AxisUI> _axisUIs;
        private AnchorUI _anchorUI;
        private readonly Vec<Vector3> _embedding = new Vec<Vector3>(Vector3.right, Vector3.up, Vector3.forward);
    }
}
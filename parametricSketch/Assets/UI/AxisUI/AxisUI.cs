using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

namespace UI
{
    public class AxisUI : MonoBehaviour, CoordinateManipulation.IScreenDistanceCalculator
    {
        [SerializeField] MueUI2D _mueUi2DPrefab = null;
        [SerializeField] LambdaUI2D _lambdaUiPrefab = null;
        [SerializeField] OriginUI _originUiPrefab = null;
        [SerializeField] protected float _padding = default;


        internal void Initialize(Vector3 direction, string label)
        {
            gameObject.name = label;
            _direction = direction;
        }

        public void UpdateCoordinateUIs(
            Axis axis,
            Vector3 orthogonalDirection,
            float orthogonalAnchor,
            CoordinateUIStyle coordinateUIStyle,
            List<Parameter> referencedParameter,
            bool hasKeyboardInputSelection)
        {
            var lambdaCoordinates = axis.Coordinates.Where(coordinate => coordinate is Lambda).ToList();
            var mueCoordinates = axis.Coordinates.Where(coordinate => coordinate is Mue).ToList();
            var nextLambdaUI = 0;
            var nextMueUI = 0;

            UpdatePool(lambdaCoordinates, mueCoordinates);

            var rows = new List<CoordinateRow> {new CoordinateRow(0)};

            //update uis
            foreach (var c in axis.Coordinates)
            {
                if (!rows.Any(r => r.DoesCoordinateFitIntoRow(c)))
                {
                    var newRow = new CoordinateRow(rows.Count);
                    rows.Add(newRow);
                }

                var selectedRow = rows.First(r => r.DoesCoordinateFitIntoRow(c));
                selectedRow.AddCoordinate(c);

                var layoutInfo = new CoordinateUI.LayoutInfo()
                {
                    Index = -selectedRow.Index,
                    OrthogonalAnchor = orthogonalAnchor,
                    OrthogonalDirection = orthogonalDirection,
                };

                switch (c)
                {
                    case Lambda lambda:
                        _uiPoolLambda[nextLambdaUI].UpdateUI(lambda, layoutInfo, _direction, _padding,
                            coordinateUIStyle.Lambda);
                        nextLambdaUI++;
                        break;
                    case Mue mue:
                        _uiPoolMue[nextMueUI].UpdateUI(mue, layoutInfo, _direction, _padding, coordinateUIStyle.Mue,
                            hasKeyboardInputSelection, referencedParameter.Contains(c.Parameter));
                        nextMueUI++;
                        break;
                    case Origin origin:
                        _originUI.UpdateUI(origin, layoutInfo, _direction, _padding, coordinateUIStyle.Origin);
                        break;
                }
            }
        }

        private class CoordinateRow
        {
            public readonly int Index;

            public CoordinateRow(int index)
            {
                Index = index;
            }

            public void AddCoordinate(Coordinate c)
            {
                _coordinates.Add(c);
            }

            public bool DoesCoordinateFitIntoRow(Coordinate c)
            {
                var coordinateBounds = c.GetBounds();

                foreach (var otherC in _coordinates)
                {
                    var doesNotCollide = coordinateBounds.max <= otherC.GetBounds().min ||
                                         coordinateBounds.min >= otherC.GetBounds().max;
                    if (!doesNotCollide)
                        return false;
                }

                return true;
            }

            private List<Coordinate> _coordinates = new List<Coordinate>();
        }

        private void UpdatePool(List<Coordinate> lambdaCoordinates, List<Coordinate> mueCoordinates)
        {
            if (_originUI == null)
            {
                var newUI = Instantiate(_originUiPrefab, transform);
                _originUI = newUI;
            }

            while (_uiPoolLambda.Count > lambdaCoordinates.Count)
            {
                var uiToRemove = _uiPoolLambda[0];
                _uiPoolLambda.Remove(uiToRemove);
                Destroy(uiToRemove.gameObject);
            }

            while (_uiPoolMue.Count > mueCoordinates.Count)
            {
                var uiToRemove = _uiPoolMue[0];
                _uiPoolMue.Remove(uiToRemove);
                Destroy(uiToRemove.gameObject);
            }

            while (_uiPoolLambda.Count < lambdaCoordinates.Count)
            {
                var newUI = Instantiate(_lambdaUiPrefab, transform);
                _uiPoolLambda.Add(newUI);
            }

            while (_uiPoolMue.Count < mueCoordinates.Count)
            {
                var newUI2D = Instantiate(_mueUi2DPrefab, transform);
                _uiPoolMue.Add(newUI2D);
            }
        }

        List<CoordinateManipulation.ScreenDistance> CoordinateManipulation.IScreenDistanceCalculator.
            GetAllDistancesToCoordinateUIs(Vector2 screenPos)
        {
            return _uiPoolMue.Select(ui => ui.GetScreenDistanceToCoordinate(screenPos)).ToList();
        }

        private readonly List<MueUI2D> _uiPoolMue = new List<MueUI2D>();
        private readonly List<LambdaUI2D> _uiPoolLambda = new List<LambdaUI2D>();
        private OriginUI _originUI;
        private Vector3 _direction;
    }
}
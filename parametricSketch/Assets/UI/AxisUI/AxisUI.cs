using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Model;
using UnityEngine;

namespace UI
{
    public class AxisUI : MonoBehaviour
    {
        [SerializeField] MueUI _mueUiPrefab;

        [SerializeField] LambdaUI _lambdaUiPrefab;

        [SerializeField] OriginUI _originUiPrefab;

        private Axis _axis;
        private Vector3 _direction;
        private Action<Axis, Coordinate, float> _modelChangeRequest;

        internal void Initialize(Axis axis, Action<Axis, Coordinate, float> modelChangeRequest, Vector3 direction,
            string label)
        {
            gameObject.name = label;
            _direction = direction;
            _modelChangeRequest = modelChangeRequest;
            _axis = axis;
        }

        public void UpdateCoordinateUIs(Vector3 orthogonalDirection, float orthogonalAnchor)
        {
            var lambdaCoordinates = _axis.Coordinates.Where(coordinate => coordinate is Lambda).ToList();
            var mueCoordinates = _axis.Coordinates.Where(coordinate => coordinate is Mue).ToList();
            var nextLambdaUI = 0;
            var nextMueUI = 0;

            if (_originUI == null)
            {
                var newUI = Instantiate(_originUiPrefab, transform);
                newUI.Initialize(_axis, _direction, (changedCoordinate, parameter) =>
                    _modelChangeRequest(_axis, changedCoordinate, parameter));
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
                newUI.Initialize(_axis, _direction, (changedCoordinate, parameter) =>
                    _modelChangeRequest(_axis, changedCoordinate, parameter));
                _uiPoolLambda.Add(newUI);
            }

            while (_uiPoolMue.Count < mueCoordinates.Count)
            {
                var newUI = Instantiate(_mueUiPrefab, transform);
                newUI.Initialize(_axis, _direction, (changedCoordinate, parameter) =>
                    _modelChangeRequest(_axis, changedCoordinate, parameter));
                _uiPoolMue.Add(newUI);
            }

            //update uis
            for (var i = 0; i < _axis.Coordinates.Count; i++)
            {
                var coordinate = _axis.Coordinates[i];
                var layoutInfo = new CoordinateUI.LayoutInfo()
                {
                    Index = -i,
                    OrthogonalAnchor = orthogonalAnchor,
                    OrthogonalDirection = orthogonalDirection,
                };

                switch (coordinate)
                {
                    case Lambda lambda:
                        _uiPoolLambda[nextLambdaUI].UpdateUI(coordinate, layoutInfo);
                        nextLambdaUI++;
                        break;
                    case Mue mue:
                        _uiPoolMue[nextMueUI].UpdateUI(coordinate, layoutInfo);
                        nextMueUI++;
                        break;
                    case Origin origin:
                        _originUI.UpdateUI(coordinate, layoutInfo);
                        break;
                }
            }
        }

        private readonly List<CoordinateUI> _uiPoolMue = new List<CoordinateUI>();
        private readonly List<CoordinateUI> _uiPoolLambda = new List<CoordinateUI>();
        private CoordinateUI _originUI;
    }
}
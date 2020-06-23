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
        [SerializeField] MueUI2D _mueUi2DPrefab;

        [SerializeField] LambdaUI _lambdaUiPrefab;

        [SerializeField] OriginUI _originUiPrefab;
        [SerializeField] protected float _padding;


        private Action<Axis, Coordinate, float> _modelChangeRequest;

        internal void Initialize(Action<Axis, Coordinate, float> modelChangeRequest, Vector3 direction, string label)
        {
            gameObject.name = label;
            _direction = direction;
            _modelChangeRequest = modelChangeRequest;
        }

        public void UpdateCoordinateUIs(Axis axis, Vector3 orthogonalDirection, float orthogonalAnchor)
        {
            var lambdaCoordinates = axis.Coordinates.Where(coordinate => coordinate is Lambda).ToList();
            var mueCoordinates = axis.Coordinates.Where(coordinate => coordinate is Mue).ToList();
            var nextLambdaUI = 0;
            var nextMueUI = 0;

            if (_originUI == null)
            {
                var newUI = Instantiate(_originUiPrefab, transform);
                newUI.Initialize(axis,
                    (changedCoordinate, parameter) => _modelChangeRequest(axis, changedCoordinate, parameter));
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
                newUI.Initialize(axis,
                    (changedCoordinate, parameter) => _modelChangeRequest(axis, changedCoordinate, parameter));
                _uiPoolLambda.Add(newUI);
            }

            while (_uiPoolMue.Count < mueCoordinates.Count)
            {
                var newUI = Instantiate(_mueUiPrefab, transform);
                newUI.Initialize(axis, (changedCoordinate, parameter) =>
                    _modelChangeRequest(axis, changedCoordinate, parameter));

                var newUI2D = Instantiate(_mueUi2DPrefab, transform);

                _uiPoolMue.Add(newUI2D);
            }

            //update uis
            for (var i = 0; i < axis.Coordinates.Count; i++)
            {
                var coordinate = axis.Coordinates[i];
                var layoutInfo = new CoordinateUI.LayoutInfo()
                {
                    Index = -i,
                    OrthogonalAnchor = orthogonalAnchor,
                    OrthogonalDirection = orthogonalDirection,
                };

                switch (coordinate)
                {
                    case Lambda lambda:
                        _uiPoolLambda[nextLambdaUI].UpdateUI(coordinate, layoutInfo, _direction, _padding);
                        nextLambdaUI++;
                        break;
                    case Mue mue:
                        _uiPoolMue[nextMueUI].UpdateUI(mue, layoutInfo, _direction, _padding);
                        nextMueUI++;
                        break;
                    case Origin origin:
                        _originUI.UpdateUI(coordinate, layoutInfo, _direction, _padding);
                        break;
                }
            }
        }

        private readonly List<MueUI2D> _uiPoolMue = new List<MueUI2D>();
        private readonly List<CoordinateUI> _uiPoolLambda = new List<CoordinateUI>();
        private CoordinateUI _originUI;
        private Vector3 _direction;
    }
}
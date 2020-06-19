using System;
using System.Collections.Generic;
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

        internal void Initialize(Axis axis, Action<Axis, Coordinate, float> modelChangeRequest, Vector3 direction, string label)
        {
            gameObject.name = label;
            _direction = direction;
            _modelChangeRequest = modelChangeRequest;
            _axis = axis;
            
        }

        public void UpdateCoordinateUIs(Vector3 orthogonalDirection, float orthogonalAnchor)
        {
            //remove deprecated uis
            var uisToDelete = new List<Coordinate>();
            foreach (var kvp in _ui)
            {
                if (_axis.Coordinates.Contains(kvp.Key))
                    continue;
                uisToDelete.Add(kvp.Key);
            }

            foreach (var c in uisToDelete)
            {
                var ui = _ui[c];
                Destroy(_ui[c].gameObject);
                _ui.Remove(c);
            }

            for (var i = 0;
                i < _axis.Coordinates.Count;
                i++)
            {
                var c = _axis.Coordinates[i];

                var layoutInfo = new CoordinateUI.LayoutInfo()
                {
                    Index = -i,
                    OrthogonalAnchor = orthogonalAnchor,
                    OrthogonalDirection = orthogonalDirection,
                };

                if (!_ui.ContainsKey(c))
                {
                    CoordinateUI ui;
                    switch (c)
                    {
                        case Mue _:
                            ui = Instantiate(_mueUiPrefab, transform);
                            break;
                        case Lambda _:
                            ui = Instantiate(_lambdaUiPrefab, transform);
                            break;
                        default:
                            ui = Instantiate(_originUiPrefab, transform);
                            break;
                    }


                    ui.Initialize(_axis,c, _direction,
                        (coordinate, parameter) => _modelChangeRequest(_axis, coordinate, parameter));
                    _ui.Add(c, ui);
                }

                _ui[c].UpdateUI(layoutInfo);
            }
  
        }

        private readonly Dictionary<Coordinate, CoordinateUI> _ui = new Dictionary<Coordinate, CoordinateUI>();
    }
}
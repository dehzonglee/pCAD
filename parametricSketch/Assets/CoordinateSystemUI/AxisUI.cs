using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisUI : MonoBehaviour
{

    [SerializeField]
    MueUI _mueUIPrefab;

    [SerializeField]
    LambdaUI _lambdaUIPrefab;

    [SerializeField]
    OriginUI _originUIPrefab;

    private Axis _axis;
    private Vector3 _direction;
    private Action<Axis, Coordinate, float> _modelChangeRequest;
    internal void Initialize(Axis axis, Action<Axis, Coordinate, float> modelChangeRequest, Vector3 direction)
    {
        _direction = direction;
        _modelChangeRequest = modelChangeRequest;
        _axis = axis;
    }

    public void UpdateCoordinateUIs(Vector3 orthogonalDirection, float orthogonalAnchor)
    {
        for (int i = 0; i < _axis.Coordinates.Count; i++)
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
                if (c as Mue != null)
                    ui = Instantiate(_mueUIPrefab, transform);
                else if (c as Lambda != null)
                    ui = Instantiate(_lambdaUIPrefab, transform);
                else
                    ui = Instantiate(_originUIPrefab, transform);


                ui.Initalize(c, _direction, (coordinate, parameter) => _modelChangeRequest(_axis, coordinate, parameter));
                _ui.Add(c, ui);
            }

            _ui[c].UpdateUI(layoutInfo);
        }
    }

    private Dictionary<Coordinate, CoordinateUI> _ui = new Dictionary<Coordinate, CoordinateUI>();

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisUI : MonoBehaviour
{

    [SerializeField]
    CoordinateUI _coordinateUIPrefab;

    private Axis _axis;
    private Action<Axis, Coordinate, float> _modelChangeRequest;
    internal void Initialize(Axis axis, Action<Axis, Coordinate, float> modelChangeRequest)
    {
        _modelChangeRequest = modelChangeRequest;
        _axis = axis;
    }

    public void UpdateCoordinateUIs(Vector3 direction, Vector3 orthogonalDirection)
    {
        var orthogonalAnchor = _axis.SmallestValue;
        for (int i = 0; i < _axis.Coordinates.Count; i++)
        {
            var c = _axis.Coordinates[i];

            var layoutInfo = new CoordinateUI.LayoutInfo()
            {
                Direction = direction,
                Index = -i,
                OrthogonalAnchor = orthogonalAnchor,
                OrthogonalDirection = orthogonalDirection,
            };

            if (!_ui.ContainsKey(c))
            {
                var ui = Instantiate(_coordinateUIPrefab, transform);
                ui.Initalize(c, (coordinate, parameter) => _modelChangeRequest(_axis, coordinate, parameter));
                _ui.Add(c, ui);
            }

            _ui[c].UpdateUI(c, layoutInfo);
        }
    }
    private Dictionary<Coordinate, CoordinateUI> _ui = new Dictionary<Coordinate, CoordinateUI>();

}

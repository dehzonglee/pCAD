using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystemUI : MonoBehaviour
{
    [SerializeField]
    CoordinateUI _coordinateUIPrefab;

    public void Initialize(CoordinateSystem cs)
    {
        _coordinateSystem = cs;
    }

    void Update()
    {
        if (_coordinateSystem == null)
            return;

        RenderAxis(_coordinateSystem.Axes[Dimensions.X], Vector3.right);
        RenderAxis(_coordinateSystem.Axes[Dimensions.Y], Vector3.up);
        RenderAxis(_coordinateSystem.Axes[Dimensions.Z], Vector3.forward);

    }

    private void RenderAxis(Axis axis, Vector3 direction)
    {
        foreach (var c in axis.Coordinates)
        {
            if (!_ui.ContainsKey(c))
            {
                var ui = Instantiate(_coordinateUIPrefab, transform);
                ui.Initalize(c, direction);
                _ui.Add(c, ui);
            }
            // _ui[c].Initalize()
        }
    }


    private CoordinateSystem _coordinateSystem;
    private Dictionary<Coordinate, CoordinateUI> _ui = new Dictionary<Coordinate, CoordinateUI>();
}

using Interaction;
using UnityEngine;

public class MueUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI = null;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI = null;
    [SerializeField] protected CoordinateLabelUI _coordinateLabelUI = null;

    public void UpdateUI(Mue coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding,
        CoordinateUIStyle.MueUIStyle style)
    {
        var state = coordinate.IsPreview ? SketchStyle.State.Focus : SketchStyle.State.Default;
        _coordinate = coordinate;
        var labelString = coordinate.Parameter.ToString("F");
        gameObject.name = $"Mue2D:{labelString}";

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;

        var parentCoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var labelPosition = (coordinateUIPositionWorld + parentCoordinateUIPositionWorld) * 0.5f;

        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection, style.GridLineStyle,state);
        _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld, style.CoordinateGizmoStyle,state);
        _coordinateDimensionLineUI.UpdateUI(coordinateUIPositionWorld, parentCoordinateUIPositionWorld,
            style.DimensionLineStyle,state);
        _coordinateLabelUI.UpdateUI(labelString, labelPosition, style.LabelStyle,state);
    }

    public CoordinateManipulation.ScreenDistance GetScreenDistanceToCoordinate(Vector2 screenPos)
    {
        var distance = _gridLineUI.GetScreenDistanceToLine(screenPos);
        Debug.Log($"distance to {_coordinate.Parameter} is {distance}");
        return new CoordinateManipulation.ScreenDistance()
            {Coordinate = _coordinate, ScreenDistanceToCoordinate = distance};
    }

    private Mue _coordinate;
}
using UnityEngine;

public class MueUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _targetGizmo = null;
    [SerializeField] protected CoordinateGizmoUI _parentGizmo = null;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI = null;
    [SerializeField] protected CoordinateLabelUI _coordinateLabelUI = null;

    public void UpdateUI(Mue coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding,
        CoordinateUIStyle.MueUIStyle style, bool hasKeyboardInputSelection, bool isReferencesByOtherParameter)
    {
        SketchStyle.State state;

        if (isReferencesByOtherParameter)
            state = SketchStyle.State.Referenced;
        else if (coordinate.IsPreview)
            state = hasKeyboardInputSelection ? SketchStyle.State.Selected : SketchStyle.State.Focus;
        else
            state = SketchStyle.State.Default;

        _coordinate = coordinate;
        var labelString = coordinate.Parameter.Value.ToString("F");
        gameObject.name = $"Mue2D:{labelString}";

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;
        var parentCoordinateUIPositionWorld = direction * coordinate.ParentValue + offset;
        var directionWorld = coordinateUIPositionWorld - parentCoordinateUIPositionWorld;
        var labelPosition = (coordinateUIPositionWorld + parentCoordinateUIPositionWorld) * 0.5f;

        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection, style.GridLineStyle, state);
        _targetGizmo.UpdateUI(coordinateUIPositionWorld, directionWorld, style.CoordinateGizmoStyle, state,
            CoordinateGizmoUI.Type.Arrow);
        _parentGizmo.UpdateUI(parentCoordinateUIPositionWorld, directionWorld, style.CoordinateGizmoStyle, state,
            CoordinateGizmoUI.Type.Mark);
        _coordinateDimensionLineUI.UpdateUI(coordinateUIPositionWorld, parentCoordinateUIPositionWorld,
            style.DimensionLineStyle, state);
        _coordinateLabelUI.UpdateUI(labelString, labelPosition, style.LabelStyle, state);
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
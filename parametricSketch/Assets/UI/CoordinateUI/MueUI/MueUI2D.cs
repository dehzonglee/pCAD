using UnityEngine;

public class MueUI2D : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _targetGizmo = null;
    [SerializeField] protected CoordinateGizmoUI _parentGizmo = null;
    [SerializeField] protected CoordinateDimensionLineUI _coordinateDimensionLineUI = null;
    [SerializeField] protected CoordinateLabelUI _coordinateLabelUI = null;

    public void UpdateUI(
        Mue coordinate,
        CoordinateUI.LayoutInfo layoutInfo,
        Vector3 direction,
        float padding,
        CoordinateUIStyle.MueUIStyle style,
        bool hasKeyboardInputSelection,
        bool isReferencesByOtherParameter,
        Coordinate draggedCoordinate
    )
    {
        SketchStyle.State state;

        if (coordinate.IsCurrentlyDrawn && hasKeyboardInputSelection || draggedCoordinate == coordinate)
            state = SketchStyle.State.DraggedOrExplicitInput;
        else if (isReferencesByOtherParameter || draggedCoordinate?.Parameter == coordinate.Parameter)
            state = SketchStyle.State.HasParameterReference;
        else if (coordinate.IsCurrentlyDrawn && !hasKeyboardInputSelection)
            state = SketchStyle.State.Drawing;
        else if (draggedCoordinate != null)
            state = SketchStyle.State.OtherIsDragged;
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

        _targetGizmo.UpdateUI(coordinateUIPositionWorld, directionWorld, style.CoordinateGizmoStyle, style.Colors,
            state,
            CoordinateGizmoUI.Type.Arrow);

        _parentGizmo.UpdateUI(parentCoordinateUIPositionWorld, directionWorld, style.CoordinateGizmoStyle, style.Colors,
            state,
            CoordinateGizmoUI.Type.Mark);

        _coordinateDimensionLineUI.UpdateUI(coordinateUIPositionWorld, parentCoordinateUIPositionWorld,
            style.DimensionLineStyle, style.Colors, state);

        _coordinateLabelUI.UpdateUI(labelString, labelPosition, style.LabelStyle,style.Colors, state);
    }

    public CoordinateManipulation.ScreenDistance GetScreenDistanceToCoordinate(Vector2 screenPos)
    {
        var distance = _gridLineUI.GetScreenDistanceToLine(screenPos);
        Debug.Log($"frame:{Time.frameCount}, distance to {_coordinate.Parameter} is {distance}");
        return new CoordinateManipulation.ScreenDistance()
            {Coordinate = _coordinate, ScreenDistanceToCoordinate = distance};
    }

    private Mue _coordinate;
}
using UnityEngine;

public class OriginUI : MonoBehaviour
{
    [SerializeField] protected GridLineUI _gridLineUI = null;
    [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI = null;

    public void UpdateUI(Origin coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding,
        CoordinateUIStyle.OriginUIStyle style)
    {
        var state = SketchStyle.State.Default;
        _coordinate = coordinate;
        var labelString = coordinate.Parameter.Value.ToString("F");
        gameObject.name = $"Origin:{labelString}";

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * padding);
        var coordinateUIPositionWorld = direction * coordinate.Value + offset;
        
        _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection,style.GridLineStyle,state);
        _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld, direction,style.CoordinateGizmoStyle,state,CoordinateGizmoUI.Type.Mark);
    }

    public CoordinateManipulation.ScreenDistance GetScreenDistanceToCoordinate(Vector2 screenPos)
    {
        var distance = _gridLineUI.GetScreenDistanceToLine(screenPos);
        Debug.Log($"distance to {_coordinate.Parameter} is {distance}");
        return new CoordinateManipulation.ScreenDistance()
            {Coordinate = _coordinate, ScreenDistanceToCoordinate = distance};
    }

    private Origin _coordinate;
}
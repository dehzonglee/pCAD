using UnityEngine;

public class OriginUI : CoordinateUI
{
    public override void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo, Vector3 direction, float padding)
    {
        Coordinate = coordinate;
        _label.gameObject.SetActive(false);

        var coordinateUIPosition = direction * Coordinate.Value;
        transform.position = coordinateUIPosition;
        
        UpdateBase();
    }
}
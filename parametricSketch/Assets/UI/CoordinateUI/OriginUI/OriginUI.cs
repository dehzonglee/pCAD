public class OriginUI : CoordinateUI
{
    public override void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo)
    {
        Coordinate = coordinate;
        _label.gameObject.SetActive(false);

        var coordinateUIPosition = _direction * Coordinate.Value;
        transform.position = coordinateUIPosition;

        _gridLine.positionCount = 2;
        _gridLine.useWorldSpace = true;
        _gridLine.SetPosition(0, coordinateUIPosition + layoutInfo.OrthogonalDirection * 100f);
        _gridLine.SetPosition(1, coordinateUIPosition + layoutInfo.OrthogonalDirection * -100f);
        
        UpdateBase();
    }
}
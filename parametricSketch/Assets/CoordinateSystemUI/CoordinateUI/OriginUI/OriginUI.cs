public class OriginUI : CoordinateUI
{
    public override void UpdateUI(LayoutInfo layoutInfo)
    {
        _label.gameObject.SetActive(false);

        var coordinateUIPosition = _direction * _coordinate.Value;
        transform.position = coordinateUIPosition;

        _gridLine.positionCount = 2;
        _gridLine.useWorldSpace = true;
        _gridLine.SetPosition(0, coordinateUIPosition + layoutInfo.OrthogonalDirection * 100f);
        _gridLine.SetPosition(1, coordinateUIPosition + layoutInfo.OrthogonalDirection * -100f);
    }
}
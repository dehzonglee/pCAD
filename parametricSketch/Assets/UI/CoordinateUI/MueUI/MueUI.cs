public class MueUI : CoordinateUI
{
    public override void UpdateUI(LayoutInfo layoutInfo)
    {
        _label.text = Coordinate.Parameter.ToString("F");
        _uiExposedParameter = Coordinate.Parameter;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * _padding);

        var coordinateUIPosition = _direction * Coordinate.Value + offset;
        transform.position = coordinateUIPosition;

        var mue = Coordinate as Mue;
        if (mue == null)
        {
            _line.enabled = false;
            return;
        }

        var labelOffset = layoutInfo.OrthogonalDirection * 0.5f * _padding;
//        Debug.LogFormat("labelOffset {0}, padding {1}, layoutInfo.OrthogonalDirection  {2}", labelOffset, _padding, layoutInfo.OrthogonalDirection);

        var parentCoordinateUIPosition = _direction * mue.ParentValue + offset;
        _label.transform.position = (coordinateUIPosition + parentCoordinateUIPosition) * 0.5f + labelOffset;
        _line.SetPosition(0, coordinateUIPosition);
        _line.SetPosition(1, parentCoordinateUIPosition);

        _gridLine.positionCount = 2;
        _gridLine.useWorldSpace = true;
        _gridLine.SetPosition(0, coordinateUIPosition + layoutInfo.OrthogonalDirection * 100f);
        _gridLine.SetPosition(1, coordinateUIPosition + layoutInfo.OrthogonalDirection * -100f);
    }
}
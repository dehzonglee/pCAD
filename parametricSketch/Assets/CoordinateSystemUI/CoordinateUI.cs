using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateUI : MonoBehaviour
{
    [SerializeField] float _uiExposedParameter;

    [SerializeField] TMPro.TMP_Text _label;

    [SerializeField] LineRenderer _line;

    [SerializeField] LineRenderer _gridLine;

    [SerializeField] float _padding;

    private Coordinate _coordinate;

    private Vector3 _direction;

    private Action<Coordinate, float> _modelChangeRequest;

    private const float EPSILON = 0.001f;

    void Update()
    {
        MakeBillboard();
        CheckForParameterManipultation();
    }

    public void Initalize(Coordinate c, Vector3 direction, Action<Coordinate, float> modelChangeRequest)
    {
        _modelChangeRequest = modelChangeRequest;
        _direction = direction;
        _coordinate = c;
    }

    public struct LayoutInfo
    {
        public int Index;
        public float OrthogonalAnchor;
        public Vector3 OrthogonalDirection;
    }

    public void UpdateUI(LayoutInfo layoutInfo)
    {
        _label.text = _coordinate.Parameter.ToString("F");
        _uiExposedParameter = _coordinate.Parameter;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * _padding);

        var coordinateUIPosition = _direction * _coordinate.Value + offset;
        transform.position = coordinateUIPosition;

        var mue = _coordinate as Mue;
        if (mue == null)
        {
            _line.enabled = false;
            return;
        }

        var labelOffset = layoutInfo.OrthogonalDirection * 0.5f * _padding;

        var parentCoordinateUIPosition = _direction * mue.ParentValue + offset;
        _label.transform.position = (coordinateUIPosition + parentCoordinateUIPosition) * 0.5f + labelOffset;
        _line.SetPosition(0, coordinateUIPosition);
        _line.SetPosition(1, parentCoordinateUIPosition);

        _gridLine.positionCount = 2;
        _gridLine.useWorldSpace = true;
        _gridLine.SetPosition(0, coordinateUIPosition + layoutInfo.OrthogonalDirection * 100f);
        _gridLine.SetPosition(1, coordinateUIPosition + layoutInfo.OrthogonalDirection * -100f);
    }

    private void MakeBillboard()
    {
        var target = 2 * transform.position - _camera.transform.position;
        var camUp = _camera.transform.TransformVector(Vector3.up);
        transform.LookAt(target, camUp);
    }

    private void CheckForParameterManipultation()
    {
        if (Mathf.Abs(_uiExposedParameter - _coordinate.Parameter) > EPSILON)
        {
            _modelChangeRequest.Invoke(_coordinate, _uiExposedParameter);
        }
    }

    public void ManipulateCoordinate(Vector3 raycastPosition)
    {
        _modelChangeRequest(_coordinate, MousePositionToParameter(raycastPosition));
    }

    private float MousePositionToParameter(Vector3 mouseWorldPosition)
    {
        return Vector3.Dot(mouseWorldPosition, _direction) - _coordinate.ParentValue;
    }

    private Camera _cameraCache;
    private Camera _camera
    {
        get
        {
            if (_cameraCache == null)
                _cameraCache = Camera.main;
            return _cameraCache;
        }
    }
}

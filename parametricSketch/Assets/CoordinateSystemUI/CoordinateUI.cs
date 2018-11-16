using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateUI : MonoBehaviour
{
    [SerializeField] float _parameter;

    [SerializeField] TMPro.TMP_Text _label;
    [SerializeField] LineRenderer _line;
    [SerializeField] float _padding;

    private Coordinate _coordinate;

    private Action<Coordinate, float> _modelChangeRequest;

    private const float EPSILON = 0.001f;

    void Update()
    {
        var target = 2 * transform.position - _camera.transform.position;
        var camUp = _camera.transform.TransformVector(Vector3.up);
        transform.LookAt(target, camUp);

        // Debug.LogFormat("{0} != {1}", _parameter, _coordinate.Parameter);
        if (Mathf.Abs(_parameter - _coordinate.Parameter) > EPSILON)
        {
            _modelChangeRequest.Invoke(_coordinate, _parameter);
        }
    }

    public void Initalize(Coordinate c, Action<Coordinate, float> modelChangeRequest)
    {
        _modelChangeRequest = modelChangeRequest;
        _coordinate = c;
    }

    public struct LayoutInfo
    {
        public Vector3 Direction;
        public int Index;
        public float OrthogonalAnchor;
        public Vector3 OrthogonalDirection;
    }

    public void UpdateUI(Coordinate c, LayoutInfo layoutInfo)
    {
        _label.text = c.Parameter.ToString("F");
        _parameter = c.Parameter;

        // var offsetDirection = layoutInfo.OrthogonalDirection * layoutInfo.Direction;

        var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * _padding);

        var coordinateUIPosition = layoutInfo.Direction * c.Value + offset;
        transform.position = coordinateUIPosition;

        var mue = _coordinate as Mue;
        if (mue == null)
        {
            _line.enabled = false;
            return;
        }

        var parentCoordinateUIPosition = layoutInfo.Direction * mue.ParentValue + offset;
        _label.transform.position = (coordinateUIPosition + parentCoordinateUIPosition) * 0.5f;
        _line.SetPosition(0, coordinateUIPosition);
        _line.SetPosition(1, parentCoordinateUIPosition);
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

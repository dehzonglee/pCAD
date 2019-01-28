using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoordinateUI : MonoBehaviour
{
    [SerializeField] protected float _uiExposedParameter;

    [SerializeField] protected TMPro.TMP_Text _label;

    [SerializeField] protected LineRenderer _line;

    [SerializeField] protected LineRenderer _gridLine;

    [SerializeField] protected float _padding;

    void Update()
    {
        MakeBillboard();
        CheckForParameterManipultation();
    }

    public void Initalize(Coordinate c, Vector3 direction, Action<Coordinate, float> modelChangeRequest)
    {
        gameObject.name = c.Name;
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

    public abstract void UpdateUI(LayoutInfo layoutInfo);

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

    protected Coordinate _coordinate;

    protected Vector3 _direction;

    protected Action<Coordinate, float> _modelChangeRequest;

    protected const float EPSILON = 0.001f;

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

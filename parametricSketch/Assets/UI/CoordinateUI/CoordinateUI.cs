using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CoordinateUI : MonoBehaviour
{
    [SerializeField] protected float _uiExposedParameter;
    [SerializeField] protected TMPro.TMP_Text _label;
    [SerializeField] protected LineRenderer _line;

    public struct LayoutInfo
    {
        public int Index;
        public float OrthogonalAnchor;
        public Vector3 OrthogonalDirection;
    }

    public void Initialize(Action<Coordinate, float> modelChangeRequest)
    {
        _modelChangeRequest = modelChangeRequest;
    }

    public abstract void UpdateUI(Coordinate coordinate, LayoutInfo layoutInfo, Vector3 direction, float padding);

    protected void UpdateBase()
    {
        CheckForParameterManipulation();
        MakeBillboard();
    }

    private void MakeBillboard()
    {
        var target = 2 * transform.position - _camera.transform.position;
        var camUp = _camera.transform.TransformVector(Vector3.up);
        transform.LookAt(target, camUp);
    }

    private void CheckForParameterManipulation()
    {
        if (Mathf.Abs(_uiExposedParameter - Coordinate.Parameter) > EPSILON)
        {
            _modelChangeRequest.Invoke(Coordinate, _uiExposedParameter);
        }
    }

    public Coordinate Coordinate;

    private Action<Coordinate, float> _modelChangeRequest;
    private const float EPSILON = 0.001f;

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
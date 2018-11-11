using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateUI : MonoBehaviour
{
    [SerializeField] float _parameter;

    [SerializeField] TMPro.TMP_Text _label;

    private Coordinate _coordinate;

    private Action<Coordinate, float> _modelChangeRequest;

    void Update()
    {
        var target = 2 * transform.position - _camera.transform.position;
        var camUp = _camera.transform.TransformVector(Vector3.up);
        transform.LookAt(target, camUp);

        if (_parameter != _coordinate.Value)
        {
            _modelChangeRequest.Invoke(_coordinate, _parameter);
        }
    }

    public void Initalize(Coordinate c, Vector3 direction, Action<Coordinate, float> modelChangeRequest)
    {
        _modelChangeRequest = modelChangeRequest;
        _coordinate = c;
        _coordinate.ValueChangedEvent += () => UpdateUI(c, direction);
        UpdateUI(c, direction);
    }

    private void UpdateUI(Coordinate c, Vector3 direction)
    {
        transform.position = direction * c.Parameter;
        _label.text = c.Parameter.ToString("F");
        _parameter = c.Parameter;
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

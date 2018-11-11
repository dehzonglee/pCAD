using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateUI : MonoBehaviour
{
    [SerializeField] float _value;

    [SerializeField] TMPro.TMP_Text _label;

    private Coordinate _coordinate;

    void Update()
    {
        var target = 2 * transform.position - _camera.transform.position;
        var camUp = _camera.transform.TransformVector(Vector3.up);
        transform.LookAt(target, camUp);
    }

    public void Initalize(Coordinate c, Vector3 direction)
    {
        _coordinate = c;
        UpdateUI(c, direction);
    }

    private void UpdateUI(Coordinate c, Vector3 direction)
    {
        transform.position = direction * c.Value;
        _label.text = c.Value.ToString("F");
        _value = c.Value;
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

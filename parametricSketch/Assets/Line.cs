﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public void SetPositions(ParametricPosition p0, ParametricPosition p1)
    {
        _p0 = p0;
        _p1 = p1;
    }

    public void SetFirstPosition(ParametricPosition p0)
    {
        _p0 = p0;
    }

    public void SetSecondPosition(ParametricPosition p1)
    {
        _p1 = p1;
    }

    void OnDrawGizmos()
    {
        if (!_isInititalized)
            return;
        Gizmos.DrawLine(_p0.Value, _p1.Value);
    }


    private bool _isInititalized { get { return _p0 != null && _p1 != null; } }

    private ParametricPosition _p0;
    private ParametricPosition _p1;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public void Initialize(Position p0, Position p1)
    {
        _p0 = p0;
        _p1 = p1;
    }

    void OnDrawGizmos()
    {
        if (!_isInititalized)
            return;
        Gizmos.DrawLine(_p0.Value, _p1.Value);
    }

    private bool _isInititalized => _p0 != null && _p1 != null;

    private Position _p0;
    private Position _p1;
}

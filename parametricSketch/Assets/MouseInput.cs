using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseInput
{
    public static Vector3 WorldSpacePosition
    {
        get
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
    }
}

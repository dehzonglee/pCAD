using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseInput
{
    public static Vector2 WorldSpaceXZPosition
    {
        get
        {
            var pos = Vector2.zero;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                pos.x = hit.point.x;
                pos.y = hit.point.z;
            }
            return pos;
        }
    }

}

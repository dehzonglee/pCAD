using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public static class MouseInput
{
    public static Vector3 RaycastPosition
    {
        get
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hit) ? hit.point : Vector3.zero;
        }
    }

    public static (Axis, Coordinate)? RaycastCoordinateUI
    {
        get
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) 
                return null;
            
            var ui = hit.transform.GetComponentInParent<CoordinateUI>();
            if (ui == null)
                return null;
            
            return (ui.Axis, ui.Coordinate);

        }
    }
}
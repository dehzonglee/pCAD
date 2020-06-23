using System.Collections;
using System.Collections.Generic;
using Interaction;
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
}
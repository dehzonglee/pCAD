using UnityEngine;

public static class MouseInput
{
    public static Vec<float> RaycastPosition
    {
        get
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out var hit)
                ? new Vec<float>(hit.point.x, hit.point.y, hit.point.z)
                : new Vec<float>(0f);
        }
    }
}
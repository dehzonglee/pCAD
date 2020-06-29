using UnityEngine;

public static class MouseInput
{
    public static GenericVector<float> RaycastPosition
    {
        get
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out var hit)
                ? new GenericVector<float>(hit.point.x, hit.point.y, hit.point.z)
                : new GenericVector<float>(0f);
        }
    }
}
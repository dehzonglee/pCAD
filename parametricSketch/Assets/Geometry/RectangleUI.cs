using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RectangleUI : MonoBehaviour
{
    [SerializeField] private MeshFilter _fillingMesh = null;

    public void Initialize()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateUI(float x0, float x1, float y0, float y1, float z0, float z1)
    {
        // generate the four corners of the rectangle from p0 and p1
        var positionValues = new[]
        {
            new Vector3(x0, y0, z0),
            new Vector3(x1, y0, z0),
            new Vector3(x1, y1, z1),
            new Vector3(x0, y1, z1),
        };
        _lineRenderer.SetPositions(positionValues);

        _fillingMesh.mesh = new Mesh()
        {
            vertices = positionValues,
            triangles = new int[6]
            {
                0, 2, 1,
                2, 0, 3
            },
        };
        _fillingMesh.mesh.RecalculateBounds();
        _fillingMesh.mesh.RecalculateNormals();
    }

    private LineRenderer _lineRenderer;
}
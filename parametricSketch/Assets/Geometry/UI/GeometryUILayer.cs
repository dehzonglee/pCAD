using System;
using UnityEngine.UI;

public class GeometryUILayer : MaskableGraphic
{
    public void Draw(Action<VertexHelper> drawFunction)
    {
        _drawFunction = drawFunction;
        SetVerticesDirty();
    }
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        _drawFunction?.Invoke(vh);
    }

    private Action<VertexHelper> _drawFunction;
}
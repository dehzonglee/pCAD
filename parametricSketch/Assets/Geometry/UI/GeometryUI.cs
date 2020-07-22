using System;
using UnityEngine;
using UnityEngine.UI;

public class GeometryUI : MonoBehaviour
{
    [SerializeField] private GeometryUILayer _fillingLayer;
    [SerializeField] private GeometryUILayer _outlineLayer;

    public void UpdateUI(Sketch.GeometryModel geometryModel, GeometryStyle styles)
    {
        switch (geometryModel)
        {
            case Sketch.PointModel pointModel:
                _fillingLayer.gameObject.SetActive(false);
                _outlineLayer.gameObject.SetActive(true);
                _outlineLayer.Draw(vh => DrawPoint(vh, pointModel, styles.Points));
                break;
            case Sketch.LineModel lineModel:
                _fillingLayer.gameObject.SetActive(false);
                _outlineLayer.gameObject.SetActive(true);
                _outlineLayer.Draw(vh => DrawLine(vh, lineModel, styles.Lines));
                break;
            case Sketch.RectangleModel rectangleModel:
            {
                var style = styles.Rectangle;
                _fillingLayer.gameObject.SetActive(true);
                _outlineLayer.gameObject.SetActive(true);
                _fillingLayer.Draw(vh => DrawRectangleFilling(vh, rectangleModel, style));
                _outlineLayer.Draw(vh => DrawRectangleOutline(vh, rectangleModel, style));
                break;
            }
        }
    }

    private static void DrawRectangleFilling(VertexHelper vh, Sketch.RectangleModel model,
        GeometryStyle.RectangleStyleSet styleSet)
    {
        var p0 = CoordinateTupleToVector3(model.P0);
        var p1 = CoordinateTupleToVector3(model.P1);
        var color = model.IsBaked ? styleSet.DefaultStyle.FillColor : styleSet.FocusStyle.FillColor;

        var p0World = p0;
        var p1World = new Vector3(p1.x, 0f, p0.z);
        var p2World = p1;
        var p3World = new Vector3(p0.x, 0f, p1.z);
        UIMeshGenerationHelper.AddQuadrilateral(vh, (p0World, p1World, p2World, p3World), color);
    }

    private static void DrawRectangleOutline(VertexHelper vh, Sketch.RectangleModel model,
        GeometryStyle.RectangleStyleSet styleSet)
    {
        var p0 = CoordinateTupleToVector3(model.P0);
        var p1 = CoordinateTupleToVector3(model.P1);
        var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.FocusStyle;

        var p0World = p0;
        var p1World = new Vector3(p1.x, 0f, p0.z);
        var p2World = p1;
        var p3World = new Vector3(p0.x, 0f, p1.z);

        UIMeshGenerationHelper.AddLine(vh, p0World, p1World - p0World, style.OutlineWidth, style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p1World, p2World - p1World, style.OutlineWidth, style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p2World, p3World - p2World, style.OutlineWidth, style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p3World, p0World - p3World, style.OutlineWidth, style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private static void DrawLine(VertexHelper vh, Sketch.LineModel model, GeometryStyle.LineStyleSet styleSet)
    {
        var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.FocusStyle;
        var p0 = CoordinateTupleToVector3(model.P0);
        var p1 = CoordinateTupleToVector3(model.P1);
        UIMeshGenerationHelper.AddLine(vh, p0, p1 - p0, style.OutlineWidth, style.OutlineColor,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private static void DrawPoint(VertexHelper vh, Sketch.PointModel model, GeometryStyle.PointStyleSet styleSet)
    {
        var p0 = CoordinateTupleToVector3(model.P0);
        var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.FocusStyle;
        UIMeshGenerationHelper.AddLine(vh, p0, Vector3.zero, style.OutlineWidth,
            style.OutlineColor, UIMeshGenerationHelper.CapsType.Round);
    }

    private static Vector3 CoordinateTupleToVector3(Vec<Coordinate> tuple)
    {
        return new Vector3(tuple.X.Value, tuple.Y.Value, tuple.Z.Value);
    }
    
    private Sketch.GeometryModel _geometryModel;
    private GeometryStyle _styles;
}
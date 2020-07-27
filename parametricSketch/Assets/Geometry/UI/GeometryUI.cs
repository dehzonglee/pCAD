using System;
using UnityEngine;
using UnityEngine.UI;

public class GeometryUI : MonoBehaviour
{
    [SerializeField] private GeometryUILayer _fillingLayer;
    [SerializeField] private GeometryUILayer _outlineLayer;

    public void UpdateUI(GeometryModel geometryModel, GeometryStyleAsset.GeometryStyleSet styleSet)
    {
        switch (geometryModel)
        {
            case PointModel pointModel:
                _fillingLayer.gameObject.SetActive(false);
                _outlineLayer.gameObject.SetActive(true);
                _outlineLayer.Draw(vh => DrawPoint(vh, pointModel, styleSet));
                break;
            case LineModel lineModel:
                _fillingLayer.gameObject.SetActive(false);
                _outlineLayer.gameObject.SetActive(true);
                _outlineLayer.Draw(vh => DrawLine(vh, lineModel, styleSet));
                break;
            case RectangleModel rectangleModel:
            {
                var style = styleSet;
                _fillingLayer.gameObject.SetActive(true);
                _outlineLayer.gameObject.SetActive(true);
                _fillingLayer.Draw(vh => DrawRectangleFilling(vh, rectangleModel, style));
                _outlineLayer.Draw(vh => DrawRectangleOutline(vh, rectangleModel, style));
                break;
            }
        }
    }

    private static void DrawRectangleFilling(VertexHelper vh, RectangleModel model,
        GeometryStyleAsset.GeometryStyleSet styleSet)
    {
        var p0 = CoordinateTupleToVector3(model.P0);
        var p1 = CoordinateTupleToVector3(model.P1);
        var color = model.IsBaked ? styleSet.DefaultStyle.FillColor : styleSet.DrawingStyle.FillColor;

        var p0World = p0;
        var p1World = new Vector3(p1.x, 0f, p0.z);
        var p2World = p1;
        var p3World = new Vector3(p0.x, 0f, p1.z);
        UIMeshGenerationHelper.AddQuadrilateral(vh, (p0World, p1World, p2World, p3World), color.Value);
    }

    private static void DrawRectangleOutline(VertexHelper vh, RectangleModel model,
        GeometryStyleAsset.GeometryStyleSet styleSet)
    {
        var p0 = CoordinateTupleToVector3(model.P0);
        var p1 = CoordinateTupleToVector3(model.P1);
        var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.DrawingStyle;

        var p0World = p0;
        var p1World = new Vector3(p1.x, 0f, p0.z);
        var p2World = p1;
        var p3World = new Vector3(p0.x, 0f, p1.z);

        UIMeshGenerationHelper.AddLine(vh, p0World, p1World - p0World, style.OutlineWidth, style.OutlineColor.Value,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p1World, p2World - p1World, style.OutlineWidth, style.OutlineColor.Value,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p2World, p3World - p2World, style.OutlineWidth, style.OutlineColor.Value,
            UIMeshGenerationHelper.CapsType.Round);
        UIMeshGenerationHelper.AddLine(vh, p3World, p0World - p3World, style.OutlineWidth, style.OutlineColor.Value,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private static void DrawLine(VertexHelper vh, LineModel model, GeometryStyleAsset.GeometryStyleSet styleSet)
    {
        var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.DrawingStyle;
        var p0 = CoordinateTupleToVector3(model.P0);
        var p1 = CoordinateTupleToVector3(model.P1);
        UIMeshGenerationHelper.AddLine(vh, p0, p1 - p0, style.OutlineWidth, style.OutlineColor.Value,
            UIMeshGenerationHelper.CapsType.Round);
    }

    private static void DrawPoint(VertexHelper vh, PointModel model, GeometryStyleAsset.GeometryStyleSet styleSet)
    {
        var p0 = CoordinateTupleToVector3(model.P0);
        var style = model.IsBaked ? styleSet.DefaultStyle : styleSet.DrawingStyle;
        UIMeshGenerationHelper.AddLine(vh, p0, Vector3.zero, style.OutlineWidth,
            style.OutlineColor.Value, UIMeshGenerationHelper.CapsType.Round);
    }

    private static Vector3 CoordinateTupleToVector3(Vec<Coordinate> tuple)
    {
        return new Vector3(tuple.X.Value, tuple.Y.Value, tuple.Z.Value);
    }

    private GeometryModel _geometryModel;
    private GeometryStyleAsset _stylesAsset;
}
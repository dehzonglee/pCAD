using System;
using System.Collections.Generic;
using System.Linq;
using Model;

[Serializable]
public struct SketchModel
{
    public CoordinateSystem coordinateSystem;
    public List<GeometryModel> geometries;

    public Serialization Serialize()
    {
        return new Serialization()
        {
            cs = coordinateSystem.GetSerializableType(),
            points = geometries
                .Where(g => g is PointModel)
                .Select(p => (p as PointModel).ToSerialization())
                .ToList(),
            lines = geometries
                .Where(g => g is LineModel)
                .Select(p => (p as LineModel).ToSerialization())
                .ToList(),

            rectangles = geometries
                .Where(g => g is RectangleModel)
                .Select(p => (p as RectangleModel).ToSerialization())
                .ToList(),
        };
    }

    public void SetSerialization(Serialization serialization)
    {
        coordinateSystem.SetSerialization(serialization.cs);

        var axes = coordinateSystem.Axes;
        var coordinates = new Vec<List<Coordinate>>(axis => axes[axis].Coordinates);

        geometries = new List<GeometryModel>();
        geometries.AddRange(serialization.points.Select(p => PointModel.FromSerialization(p, coordinates)));
        geometries.AddRange(serialization.lines.Select(l => LineModel.FromSerialization(l, coordinates)));
        geometries.AddRange(serialization.rectangles.Select(r => RectangleModel.FromSerialization(r, coordinates)));
    }

    [Serializable]
    public class Serialization
    {
        public CoordinateSystem.SerializableCoordinateSystem cs;
        public List<PointModel.Serialization> points;
        public List<LineModel.Serialization> lines;
        public List<RectangleModel.Serialization> rectangles;
    }
}
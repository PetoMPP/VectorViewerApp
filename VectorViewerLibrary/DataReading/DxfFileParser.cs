using IxMilia.Dxf;
using IxMilia.Dxf.Entities;
using IxMilia.Dxf.Objects;
using System.Drawing;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.DataReading
{
    public class DxfFileParser : IFileParser
    {
        public async Task<IEnumerable<ShapeModel>> GetModelsFromFile(
            string path, CancellationToken cancellationToken)
        {
            return await Task.Run(() => ParseDxf(path));
        }

        private static List<ShapeModel> ParseDxf(string path)
        {
            var dxf = DxfFile.Load(path);

            var model = new ShapeModel { Type = ShapeType.Multi };
            var shapes = new List<ShapeModel>();

            shapes.AddRange(GetLines(dxf));
            //shapes.AddRange(GetDimensions(dxf));
            shapes.AddRange(GetArcs(dxf));
            shapes.AddRange(GetCircles(dxf));

            model.Shapes = shapes.ToArray();

            return new List<ShapeModel> { model };
        }

        private static IEnumerable<ShapeModel> GetCircles(DxfFile dxf)
        {
            var modelShapes = new List<ShapeModel>();

            foreach (var circles in dxf.Entities
                .Where(e => e.EntityType == DxfEntityType.Circle)
                .GroupBy(l => l.Layer))
            {
                var layer = dxf.Layers.First(l => l.Name == circles.Key);
                foreach (DxfCircle circle in circles)
                {
                    var color = GetDxfEntityColor(circle, layer);
                    modelShapes.Add(new ShapeModel
                    {
                        Type = ShapeType.Circle,
                        Center = new PointF((float)circle.Center.X, -(float)circle.Center.Y),
                        Radius = (float)circle.Radius,
                        Color = color
                    });
                }
            }

            return modelShapes;
        }

        private static IEnumerable<ShapeModel> GetArcs(DxfFile dxf)
        {
            var modelShapes = new List<ShapeModel>();

            foreach (var arcs in dxf.Entities
                .Where(e => e.EntityType == DxfEntityType.Arc)
                .GroupBy(l => l.Layer))
            {
                var layer = dxf.Layers.First(l => l.Name == arcs.Key);
                foreach (DxfArc arc in arcs)
                {
                    var color = GetDxfEntityColor(arc, layer);

                    modelShapes.Add(new ShapeModel
                    {
                        Type = ShapeType.Curve,
                        ArcStart = (float?)arc.StartAngle,
                        ArcEnd = (float?)arc.EndAngle,
                        Center = new PointF((float)arc.Center.X, -(float)arc.Center.Y),
                        Radius = (float)arc.Radius,
                        Color = color
                    });
                }
            }

            return modelShapes;
        }

        //private static IEnumerable<ShapeModel> GetDimensions(DxfDocument dxf)
        //{
        //    throw new NotImplementedException();
        //}

        private static IEnumerable<ShapeModel> GetLines(DxfFile dxf)
        {
            var modelShapes = new List<ShapeModel>();

            foreach (var lines in dxf.Entities
                .Where(e => e.EntityType == DxfEntityType.Line)
                .GroupBy(l => l.Layer))
            {
                var layer = dxf.Layers.First(l => l.Name == lines.Key);

                foreach (DxfLine line in lines)
                {
                    var color = GetDxfEntityColor(line, layer);

                    var lineTypeRaw = line.LineTypeName == "BYLAYER"
                        ? layer.LineTypeName
                        : line.LineTypeName;

                    var lineType = lineTypeRaw.ToLower() switch
                    {
                        "continuous" => LineType.Continuous,
                        "center" => LineType.Dashed,
                        "hidden" => LineType.Hidden,
                        _ => throw new Exception()
                    };

                    modelShapes.Add(new ShapeModel
                    {
                        Type = ShapeType.Line,
                        LineType = lineType,
                        Color = color,
                        Points = new PointF[]
                        {
                            new PointF((float)line.P1.X, -(float)line.P1.Y),
                            new PointF((float)line.P2.X, -(float)line.P2.Y)
                        }
                    });
                }
            }

            return modelShapes;
        }

        private static Color GetDxfEntityColor(DxfEntity entity, DxfLayer layer)
        {
            var colour = entity.Color.IsByLayer
                ? layer.Color
                : entity.Color;

            var channels = BitConverter.GetBytes(colour.ToRGB());
            var color = Color.FromArgb(channels[2], channels[1], channels[0]);
            //var color = $"{byte.MaxValue - entity.Transparency}; {rgbColor.R}; {rgbColor.G}; {rgbColor.B}";
            return color;
        }
    }
}

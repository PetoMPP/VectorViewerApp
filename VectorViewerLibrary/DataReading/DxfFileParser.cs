using IxMilia.Dxf;
using IxMilia.Dxf.Entities;
using System.Drawing;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.DataReading
{
    public class DxfFileParser : IFileParser
    {
#if DEBUG
        private readonly List<Type> _reportedTypes;

        public DxfFileParser()
        {
            _reportedTypes = new List<Type>();
        }
#endif

        public async Task<IEnumerable<ShapeModel>> GetModelsFromFile(
            string path, CancellationToken cancellationToken)
        {
            return await Task.Run(() => ParseDxf(path));
        }

        private List<ShapeModel> ParseDxf(string path)
        {
            var dxf = DxfFile.Load(path);

            return new List<ShapeModel>
            {
                new ShapeModel
                {
                    Type = ShapeType.Multi,
                    Shapes = GetShapes(dxf).ToArray()
                }
            };
        }

        private IEnumerable<ShapeModel> GetShapes(DxfFile dxf)
        {
            foreach (IGrouping<string, DxfEntity>? entities in dxf.Entities.GroupBy(e => e.Layer))
            {
                var layer = dxf.Layers.First(l => l.Name == entities.Key);
                foreach (var entity in entities)
                {
                    var color = GetDxfEntityColor(entity, layer);
                    var shape = entity switch
                    {
                        DxfLine line => GetShapeFromLine(layer, line, color),
                        DxfArc arc => GetShapeFromArc(arc, color),
                        DxfCircle circle => GetShapeFromCircle(circle, color),
                        _ => null
                    };
                    if (shape is not null)
                        yield return shape;

#if DEBUG
                    if (!_reportedTypes.Contains(entity.GetType()))
                        _reportedTypes.Add(entity.GetType());
#endif
                }
            }
#if DEBUG
            Console.WriteLine($"Not parsed types:{Environment.NewLine}" +
        string.Join(Environment.NewLine, _reportedTypes.Select(t => t.Name)));
#endif
        }

        private static ShapeModel GetShapeFromCircle(DxfCircle circle, Color color)
        {
            return new ShapeModel
            {
                Type = ShapeType.Circle,
                Center = new PointF((float)circle.Center.X, -(float)circle.Center.Y),
                Radius = (float)circle.Radius,
                Color = color
            };
        }

        private static ShapeModel GetShapeFromArc(DxfArc arc, Color color)
        {
            return new ShapeModel
            {
                Type = ShapeType.Curve,
                ArcStart = (float?)arc.StartAngle,
                ArcEnd = (float?)arc.EndAngle,
                Center = new PointF((float)arc.Center.X, -(float)arc.Center.Y),
                Radius = (float)arc.Radius,
                Color = color
            };
        }

        private static ShapeModel GetShapeFromLine(DxfLayer layer, DxfLine line, Color color)
        {
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

            return new ShapeModel
            {
                Type = ShapeType.Line,
                LineType = lineType,
                Color = color,
                Points = new PointF[]
                {
                            new PointF((float)line.P1.X, -(float)line.P1.Y),
                            new PointF((float)line.P2.X, -(float)line.P2.Y)
                }
            };
        }

        private static Color GetDxfEntityColor(DxfEntity entity, DxfLayer layer)
        {
            var color = entity.Color.IsByLayer
                ? layer.Color
                : entity.Color;

            var channels = BitConverter.GetBytes(color.ToRGB());
            return Color.FromArgb(channels[2], channels[1], channels[0]);
        }
    }
}

using IxMilia.Dxf;
using IxMilia.Dxf.Blocks;
using IxMilia.Dxf.Entities;
using System.Drawing;
using System.Numerics;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.DataReading
{
    public class DxfFileParser : IFileParser
    {
        public async Task<IEnumerable<ShapeModel>> GetModelsFromFile(
            string path, CancellationToken cancellationToken)
        {
            return await ParseDxf(path, cancellationToken);
        }

        private static async Task<List<ShapeModel>> ParseDxf(string path, CancellationToken cancellationToken)
        {
            var dxf = await Task.Run(() => DxfFile.Load(path), cancellationToken);

            return new List<ShapeModel>
            {
                new ShapeModel
                {
                    Type = ShapeType.Multi,
                    Shapes = GetShapes(dxf).ToArray()
                }
            };
        }

        private static IEnumerable<ShapeModel> GetShapes(DxfFile dxf)
        {
            var layeredBlocks = dxf.Blocks.GroupBy(b => b.Layer);
            var layeredEntities = dxf.Entities
                .GroupBy(e => e.Layer)
                .ExceptBy(layeredBlocks.Select(b => b.Key), e => e.Key);

            foreach (IGrouping<string, DxfBlock> blocks in layeredBlocks)
            {
                var layer = dxf.Layers.First(l => l.Name == blocks.Key);
                foreach (DxfBlock block in blocks)
                {
                    var offset = new Vector2((float)block.BasePoint.X, (float)-block.BasePoint.Y);
                    foreach (var entity in block.Entities)
                    {
                        var color = GetDxfEntityColor(entity, layer);
                        var shape = GetShapeFromEntity(layer, entity, color);
                        if (shape is null)
                            continue;

                        if (offset != Vector2.Zero)
                            shape.Offset(offset);

                        yield return shape;
                    }
                }
            }

            foreach (IGrouping<string, DxfEntity> entities in layeredEntities)
            {
                var layer = dxf.Layers.First(l => l.Name == entities.Key);
                foreach (var entity in entities)
                {
                    var color = GetDxfEntityColor(entity, layer);
                    var shape = GetShapeFromEntity(layer, entity, color);
                    if (shape is null)
                        continue;

                    yield return shape;
                }
            }
        }

        private static ShapeModel? GetShapeFromEntity(DxfLayer layer, DxfEntity entity, Color color)
        {
            var shape = entity switch
            {
                DxfLine line => GetShapeFromLine(layer, line, color),
                DxfLwPolyline lwPolyline => GetShapeFromLwPolyline(layer, lwPolyline, color),
                DxfPolyline polyline => GetShapeFromPolyline(layer, polyline, color),
                DxfArc arc => GetShapeFromArc(arc, color),
                DxfCircle circle => GetShapeFromCircle(circle, color),
                DxfInsert insert => GetShapeFromInsert(layer, color, insert),
                _ => null
            };
            if (shape is null)
                return shape;

            shape.Visible = entity.IsVisible;
            return shape;
        }

        private static ShapeModel GetShapeFromInsert(DxfLayer layer, Color color, DxfInsert insert)
        {
            var shape = new ShapeModel
            {
                Type = ShapeType.Multi,
                Shapes = insert.Entities
                    .Select(e => GetShapeFromEntity(layer, e, color))
                    .Where(e => e is not null)
                    .Cast<ShapeModel>()
                    .ToArray()
            };
            if (insert.Location != DxfPoint.Origin)
                shape.Offset(new Vector2((float)insert.Location.X, (float)-insert.Location.Y));

            return shape;
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

        private static ShapeModel GetShapeFromLwPolyline(DxfLayer layer, DxfLwPolyline polyline, Color color)
        {
            var lineType = GetLineType(layer, polyline);
            int i;
            for (i = polyline.Vertices.Count - 1; i > 1; i--)
            {
                var v = polyline.Vertices[i];
                if (v.X != 0 || v.Y != 0)
                    break;
            }
            var points = polyline.Vertices
                .Take(i + 1)
                .Select(v => new PointF((float)v.X, (float)-v.Y));

            return new ShapeModel
            {
                Type = ShapeType.Line,
                LineType = lineType,
                Color = color,
                Points = points.ToArray()
            };
        }

        private static ShapeModel GetShapeFromPolyline(DxfLayer layer, DxfPolyline polyline, Color color)
        {
            var lineType = GetLineType(layer, polyline);
            int i;
            for (i = polyline.Vertices.Count - 1; i > 1; i--)
            {
                var v = polyline.Vertices[i];
                if (v.Location != DxfPoint.Origin)
                    break;
            }

            var points = polyline.Vertices
                .Take(i + 1)
                .Select(v => new PointF((float)v.Location.X, (float)-v.Location.Y));
            return new ShapeModel
            {
                Type = ShapeType.Line,
                LineType = lineType,
                Color = color,
                Points = points.ToArray()
            };
        }

        private static ShapeModel GetShapeFromLine(DxfLayer layer, DxfLine line, Color color)
        {
            var lineType = GetLineType(layer, line);

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

        private static LineType GetLineType(DxfLayer layer, DxfEntity entity)
        {
            var lineTypeRaw = entity.LineTypeName == "BYLAYER"
                ? layer.LineTypeName
                : entity.LineTypeName;

            return lineTypeRaw.ToLower() switch
            {
                "continuous" => LineType.Continuous,
                "center" => LineType.Dashed,
                "hidden" => LineType.Hidden,
                _ => throw new Exception()
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

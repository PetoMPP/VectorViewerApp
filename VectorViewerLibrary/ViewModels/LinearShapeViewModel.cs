using System.Drawing;
using VectorViewerLibrary.Extensions;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.ViewModels
{
    public interface ILinearShapeViewModel : IShapeViewModel
    {
    }

    public class LinearShapeViewModel : ILinearShapeViewModel
    {
        public PointF[] Points { get; private set; }
        public LineType LineType { get; }
        public bool Filled { get; }
        public Color Color { get; }
        public string DisplayName { get; }

        public LinearShapeViewModel(ShapeModel model)
        {
            if (model.Points is null)
                throw new InvalidOperationException("Missing Points in ShapeModel");

            if (model.Color is null)
                throw new InvalidOperationException("Missing Color in ShapeModel");

            Points = model.Points;
            LineType = model.LineType;
            Filled = model.Filled ?? false;
            Color = model.Color ?? Color.Black;
            DisplayName = model.Type.ToString();
        }

        public void Scale(float factor)
        {
            Points = Points.Select(p => new PointF(p.X * factor, p.Y * factor)).ToArray();
        }

        public bool IsPointOnShape(PointF point, float tolerance = 0.25F)
        {
            if (Filled)
            {
                // This is ray casted line through polygon
                var outStart = point;
                var outEnd = new PointF(Points.Max(p => p.X) + 1, Points.Max(p => p.Y) + 1);
                var intersectionCount = 0;
                for (int i = 0; i < Points.Length; i++)
                {
                    var segEnd = Points[i];
                    var segStart = i == 0 ? Points[^1] : Points[i - 1];

                    if (PointFExtensions.AreSegmentsIntersecting(segStart, segEnd, outStart, outEnd))
                        intersectionCount++;
                }
                return intersectionCount % 2 == 1;
            }

            for (int i = 1; i < Points.Length; i++)
            {
                var segStart = Points[i - 1];
                var segEnd = Points[i];
                if (point.GetDistanceToLine(segStart, segEnd) <= tolerance)
                {
                    var bounds = new PointF[2] { segStart, segEnd }.GetBoundsRectangle();
                    bounds.Inflate(tolerance, tolerance);
                    if (bounds.Contains(point))
                        return true;
                }
            }
            return false;
        }
    }
}

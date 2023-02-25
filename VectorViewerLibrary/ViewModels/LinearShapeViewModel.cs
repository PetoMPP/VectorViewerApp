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
        public bool Visible { get; }

        public LinearShapeViewModel(ShapeModel model)
        {
            if (model.Points is null)
                throw new InvalidOperationException("Missing Points in ShapeModel");

            if (model.Color is null)
                throw new InvalidOperationException("Missing Color in ShapeModel");

            Points = model.Points;
            LineType = model.LineType;
            Visible = model.Visible;
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
                if (point.GetDistanceToLineSegment(segStart, segEnd) <= tolerance)
                    return true;
            }
            return false;
        }

        public float GetDistanceToShape(PointF point)
        {
            if (Filled && IsPointOnShape(point, 0))
                return 0;

            var values = new float[Points.Length - 1];
            for (int i = 1; i < Points.Length; i++)
            {
                var segStart = Points[i - 1];
                var segEnd = Points[i];
                values[i - 1] = point.GetDistanceToLineSegment(segStart, segEnd);
            }

            return values.Min();
        }

        public bool IsRectangleIntersecting(RectangleF rectangle)
        {
            if (IsContainedInRectangle(rectangle))
                return true;

            foreach (var (a, b) in rectangle.GetSides())
            {
                for (int i = 1; i < Points.Length; i++)
                {
                    var segStart = Points[i - 1];
                    var segEnd = Points[i];
                    if (PointFExtensions.AreSegmentsIntersecting(segStart, segEnd, a, b))
                        return true;
                }
            }
            return false;
        }

        public bool IsContainedInRectangle(RectangleF rectangle)
        {
            foreach (var point in Points)
            {
                if (!rectangle.Contains(point))
                    return false;
            }
            return true;
        }
    }
}

using System.Drawing;
using System.Globalization;

namespace VectorViewerLibrary.Extensions
{
    public static class PointFExtensions
    {
        public static float ConvertToRadians(this float angle)
        {
            return (float)(Math.PI / 180 * angle);
        }

        public static PointF[] TransformPoints(
            this PointF[] points,
            PointF origin,
            float zoom)
        {
            return points
                .Select(p => new PointF(p.X * zoom + origin.X, p.Y * zoom + origin.Y))
                .ToArray();
        }

        public static RectangleF GetBoundsRectangle(this PointF[] points)
        {
            var xMin = points.Select(p => p.X).Min();
            var yMin = points.Select(p => p.Y).Min();
            var xMax = points.Select(p => p.X).Max();
            var yMax = points.Select(p => p.Y).Max();

            return new RectangleF(
                new PointF(xMin, yMin),
                new SizeF(xMax - xMin, yMax - yMin));
        }
    }
}

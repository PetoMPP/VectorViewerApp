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
                .Select(p => new PointF((p.X * zoom) + origin.X, (p.Y * zoom) + origin.Y))
                .ToArray();
        }

        public static RectangleF GetBoundsRectangle(this PointF[] points)
        {
            var xMin = points.Min(p => p.X);
            var yMin = points.Min(p => p.Y);
            var xMax = points.Max(p => p.X);
            var yMax = points.Max(p => p.Y);

            return new RectangleF(
                new PointF(xMin, yMin),
                new SizeF(xMax - xMin, yMax - yMin));
        }
    }
}

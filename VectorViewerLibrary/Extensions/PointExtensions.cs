using System.Drawing;

namespace VectorViewerLibrary.Extensions
{
    public static class PointExtensions
    {
        public static Rectangle GetBoundsRectangle(this IEnumerable<Point> points)
        {
            var xMin = points.Min(p => p.X);
            var yMin = points.Min(p => p.Y);
            var xMax = points.Max(p => p.X);
            var yMax = points.Max(p => p.Y);

            return new Rectangle(
                new Point(xMin, yMin),
                new Size(xMax - xMin, yMax - yMin));
        }
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;

namespace VectorViewerLibrary.Extensions
{
    public static class PointFExtensions
    {
        public static float ConvertToRadians(this float angle)
        {
            return MathF.PI / 180 * angle;
        }

        public static float ConvertToDegrees(this float angle)
        {
            return 180 * angle / MathF.PI;
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

        public static float GetDistanceToPoint(this PointF start, PointF next)
        {
            return MathF.Sqrt(GetSquaredDistanceToPoint(start, next));
        }

        public static float GetSquaredDistanceToPoint(this PointF start, PointF next)
        {
            return MathF.Pow(next.X - start.X, 2) + MathF.Pow(next.Y - start.Y, 2);
        }

        public static float GetDistanceToLineSegment(this PointF point, PointF lineStart, PointF lineEnd)
        {
            var lineLenSq = lineStart.GetSquaredDistanceToPoint(lineEnd);
            if (lineLenSq == 0)
                return point.GetDistanceToPoint(lineStart);

            var p = point.ToVector2();
            var v = lineStart.ToVector2();
            var w = lineEnd.ToVector2();

            var t = MathF.Max(0, MathF.Min(1, Vector2.Dot(p - v, w - v) / lineLenSq));
            var proj = v + (t * (w - v));
            return point.GetDistanceToPoint((PointF)proj);
        }

        [SuppressMessage("Roslynator", "RCS1224:Make method an extension method.", Justification = "Huh?")]
        public static PointF GetPointOnCircle(PointF center, float radius, float angle)
        {
            var angleR = angle.ConvertToRadians() + (MathF.PI / 2);
            return new PointF(
                x: center.X + (radius * MathF.Sin(angleR)),
                y: center.Y + (radius * MathF.Cos(angleR)));
        }

        [SuppressMessage("Roslynator", "RCS1224:Make method an extension method.", Justification = "Huh?")]
        public static float GetAngleFromPointOnCircle(PointF center, float radius, PointF point)
        {
            var angle = (MathF.Asin((point.X - center.X) / radius) - (MathF.PI / 2))
                .ConvertToDegrees();

            if (center.Y > point.Y)
                return -angle;

            return angle;
        }

        [SuppressMessage("Roslynator", "RCS1224:Make method an extension method.", Justification = "Huh?")]
        public static bool AreSegmentsIntersecting(PointF p1, PointF q1, PointF p2, PointF q2)
        {
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            int Orientation(PointF p, PointF q, PointF r)
            {
                float val = ((q.Y - p.Y) * (r.X - q.X)) -
                    ((q.X - p.X) * (r.Y - q.Y));

                if (val == 0) return 0; // collinear

                return (val > 0) ? 1 : 2; // clock or counterclock wise
            }

            if (o1 != o2 && o3 != o4)
                return true;

            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && IsOnSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && IsOnSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && IsOnSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && IsOnSegment(p2, q1, q2)) return true;

            return false;

            bool IsOnSegment(PointF p, PointF q, PointF r)
            {
                return q.X <= MathF.Max(p.X, r.X) && q.X >= MathF.Min(p.X, r.X) &&
                    q.Y <= MathF.Max(p.Y, r.Y) && q.Y >= MathF.Min(p.Y, r.Y);
            }
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

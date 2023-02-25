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
        public static bool AreSegmentsIntersecting(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            float p0_x = p0.X;
            float p0_y = p0.Y;
            float p1_x = p1.X;
            float p1_y = p1.Y;
            float p2_x = p2.X;
            float p2_y = p2.Y;
            float p3_x = p3.X;
            float p3_y = p3.Y;
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float s, t;
            s = ((-s1_y * (p0_x - p2_x)) + (s1_x * (p0_y - p2_y))) / ((-s2_x * s1_y) + (s1_x * s2_y));
            t = ((s2_x * (p0_y - p2_y)) - (s2_y * (p0_x - p2_x))) / ((-s2_x * s1_y) + (s1_x * s2_y));

            return s >= 0 && s <= 1 && t >= 0 && t <= 1;
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

        [SuppressMessage("Roslynator", "RCS1224:Make method an extension method.", Justification = "<Pending>")]
        internal static PointF GetPointOnLine(PointF a, PointF b, float distFromA)
        {
            var dist = a.GetDistanceToPoint(b);
            return new PointF(
                x: a.X - (distFromA * (a.X - b.X) / dist),
                y: a.Y - (distFromA * (a.Y - b.Y) / dist));
        }

        [SuppressMessage("Roslynator", "RCS1224:Make method an extension method.", Justification = "<Pending>")]
        public static PointF[] GetLineCircleIntersectionPoints(PointF a, PointF b, PointF center, float radius)
        {
            a = a.Sub(center);
            b = b.Sub(center);

            var dx = b.X - a.X;
            var dy = b.Y - a.Y;

            var dr = MathF.Sqrt(dx * dx + dy * dy);
            var drSq = dr * dr;
            var d = a.X * b.Y - a.Y * b.X;
            var disc = radius * radius * dr * dr - d * d;
            var discSqrt = MathF.Sqrt(disc);
            return disc switch
            {
                > 0 => new[]
                {
                    new PointF(
                        x: (d * dy + (dy < 0 ? -1 : 1) * dx * discSqrt) / drSq ,
                        y: (-d * dx + MathF.Abs(dy) * discSqrt) / drSq)
                        .Add(center),
                    new PointF(
                        x: (d * dy - (dy < 0 ? -1 : 1) * dx * discSqrt) / drSq ,
                        y: (-d * dx - MathF.Abs(dy) * discSqrt) / drSq)
                        .Add(center)
                },
                0 => new[] { new PointF(x: d * dy / drSq, y: -d * dx / drSq).Add(center) },
                _ => Array.Empty<PointF>()
            };
        }

        public static bool IsPointOnLine(this PointF point, PointF a, PointF b)
        {
            var dxc = point.X - a.X;
            var dyc = point.Y - a.Y;

            var dxl = b.X - a.X;
            var dyl = b.Y - a.Y;

            var cross = dxc * dyl - dyc * dxl;

            if (cross != 0)
                return false;

            return MathF.Abs(dxl) >= MathF.Abs(dyl)
                ? dxl > 0
                    ? a.X <= point.X && point.X <= b.X
                    : b.X <= point.X && point.X <= a.X
                : dyl > 0
                    ? a.Y <= point.Y && point.Y <= b.Y
                    : b.Y <= point.Y && point.Y <= a.Y;
        }

        public static PointF Add(this PointF point, PointF offset)
        {
            return new PointF(
                x: point.X + offset.X,
                y: point.Y + offset.Y);
        }

        public static PointF Sub(this PointF point, PointF offset)
        {
            return new PointF(
                x: point.X - offset.X,
                y: point.Y - offset.Y);
        }
    }
}

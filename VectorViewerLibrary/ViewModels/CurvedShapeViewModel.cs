using System.Drawing;
using VectorViewerLibrary.Models;
using VectorViewerLibrary.Extensions;

namespace VectorViewerLibrary.ViewModels
{
    public interface ICurvedShapeViewModel : IShapeViewModel
    {
        float? ArcStart { get; }
        float? ArcEnd { get; }
    }

    public class CurvedShapeViewModel : ICurvedShapeViewModel
    {
        private PointF _center;
        private float _radius;

        public LineType LineType { get; }
        public PointF[] Points { get; private set; }
        public float? ArcStart { get; private set; }
        public float? ArcEnd { get; private set; }
        public bool Filled { get; }
        public Color Color { get; }
        public string DisplayName { get; }
        public bool Visible { get; }

        public CurvedShapeViewModel(ShapeModel model)
        {
            if (model.Color is null)
                throw new InvalidOperationException("Missing Color in ShapeModel");

            if (model.Center is null)
                throw new InvalidOperationException("Missing Center in ShapeModel");

            if (model.Radius is null)
                throw new InvalidOperationException("Missing Radius in ShapeModel");

            var center = (PointF)model.Center;

            Points = new PointF[]
            {
                new PointF(center.X + (float)model.Radius, center.Y + (float)model.Radius),
                new PointF(center.X - (float)model.Radius, center.Y + (float)model.Radius),
                new PointF(center.X + (float)model.Radius, center.Y - (float)model.Radius),
                new PointF(center.X - (float)model.Radius, center.Y - (float)model.Radius)
            };

            ArcStart = model.ArcStart;
            ArcEnd = model.ArcEnd;
            LineType = model.LineType;
            Visible = model.Visible;
            Filled = model.Filled ?? false;
            Color = model.Color ?? Color.Black;
            DisplayName = model.Type.ToString();
            RefreshCalculationFields();
        }

        private void RefreshCalculationFields()
        {
            var rect = Points.GetBoundsRectangle();
            _center = rect.GetCenter();
            _radius = rect.Height / 2;
        }

        public void Scale(float factor)
        {
            Points = Points.Select(p => new PointF(p.X * factor, p.Y * factor)).ToArray();
            if (ArcStart is not float start || ArcEnd is not float end)
                return;

            var diff = (end - start) * factor;
            ArcStart = end - diff;
            ArcEnd = start + diff;
            RefreshCalculationFields();
        }

        public bool IsPointOnShape(PointF point, float tolerance = 0.25F)
        {
            var dist = point.GetDistanceToPoint(_center);
            if (Filled)
                return dist - _radius <= tolerance;

            if (MathF.Abs(_radius - dist) > tolerance)
                return false;

            if (ArcStart is null || ArcEnd is null)
                return true;

            var startAngle = GetAngle((float)ArcStart);
            var endAngle = GetAngle((float)ArcEnd);
            var actualAngle = PointFExtensions.GetAngleFromPointOnCircle(_center, dist, point);

            return actualAngle >= startAngle && actualAngle <= endAngle;
        }

        /// <summary>
        /// Normalizes angle to value between 0 and 360
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>angle value between 0 and 360</returns>
        private static float GetAngle(float angle)
        {
            angle = ((int)angle % 360) + (angle - MathF.Floor(angle));
            return angle > 0
                ? angle
                : angle + 360;
        }

        public float GetDistanceToShape(PointF point)
        {
            var dist = point.GetDistanceToPoint(_center);
            if (Filled)
                return MathF.Max(dist - _radius, 0);

            if (ArcStart is null || ArcEnd is null)
                return MathF.Abs(dist - _radius);

            var start = GetAngle((float)ArcStart);
            var end = GetAngle((float)ArcEnd);
            var angle = GetAngle(PointFExtensions.GetAngleFromPointOnCircle(_center, dist, point));

            var isAngleWithinArc = (start < end)
                ? angle >= start && angle <= end
                : angle >= start || angle <= end;

            if (isAngleWithinArc)
                return MathF.Abs(dist - _radius);

            return MathF.Sqrt(MathF.Min(
                point.GetSquaredDistanceToPoint(PointFExtensions.GetPointOnCircle(_center, _radius, start)),
                point.GetSquaredDistanceToPoint(PointFExtensions.GetPointOnCircle(_center, _radius, end))));
        }

        public bool IsRectangleIntersecting(RectangleF rectangle)
        {
            var corners = rectangle.GetCorners().ToArray();

            for (int i = 0; i < corners.Length; i++)
            {
                var idx = i == 0 ? corners.Length - 1 : i - 1;
                var a = corners[idx];
                var b = corners[i];
                var rect = new[] { a, b }.GetBoundsRectangle();
                rect.Inflate(0.01F, 0.01F);
                var intersectionPoints = PointFExtensions.GetLineCircleIntersectionPoints(a, b, _center, _radius);

                if (ArcStart is null || ArcEnd is null)
                {
                    if (intersectionPoints.Any())
                        return true;

                    continue;
                }

                if (intersectionPoints.Any(p => rect.Contains(p) &&
                    IsAngleWithinArc(GetAngle(PointFExtensions.GetAngleFromPointOnCircle(_center, _radius, p)))))
                {
                    return true;
                }
            }
            return IsContainedInRectangle(rectangle);
        }

        public bool IsContainedInRectangle(RectangleF rectangle)
        {
            if (ArcStart is not float start || ArcEnd is not float end)
            {
                foreach (var point in Points)
                {
                    if (!rectangle.Contains(point))
                        return false;
                }
                return true;
            }

            // start end and every covered mult 90deg have to be contained
            start = GetAngle(start);
            end = GetAngle(end);
            var startPoint = PointFExtensions.GetPointOnCircle(_center, _radius, start);
            if (!rectangle.Contains(startPoint))
                return false;

            var endPoint = PointFExtensions.GetPointOnCircle(_center, _radius, end);
            if (!rectangle.Contains(endPoint))
                return false;

            var angles = new float[] { 0, 90, 180, 270 };

            foreach (var angle in angles)
            {
                if (IsAngleWithinArc(angle) &&
                    !rectangle.Contains(PointFExtensions.GetPointOnCircle(_center, _radius, angle)))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsAngleWithinArc(float angle)
        {
            if (ArcStart is not float start || ArcEnd is not float end)
                return true;

            start = GetAngle(start);
            end = GetAngle(end);
            return start < end
                ? angle >= start && angle <= end
                : angle >= start || angle <= end;
        }
    }
}

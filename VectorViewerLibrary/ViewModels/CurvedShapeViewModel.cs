﻿using System.Drawing;
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
        public LineType LineType { get; }
        public PointF[] Points { get; private set; }
        public float? ArcStart { get; private set; }
        public float? ArcEnd { get; private set; }
        public bool Filled { get; }
        public Color Color { get; }
        public string DisplayName { get; }

        public CurvedShapeViewModel(ShapeModel model)
        {
            if (model.Color is null)
                throw new InvalidOperationException("Missing Color in ShapeModel");

            if (model.Center is null)
                throw new InvalidOperationException("Missing Center in ShapeModel");

            if (model.Radius is null)
                throw new InvalidOperationException("Missing Radius in ShapeModel");

            var center = (PointF)model.Center;

            Points = new PointF[4]
            {
                new PointF(center.X + (float)model.Radius, center.Y + (float)model.Radius),
                new PointF(center.X - (float)model.Radius, center.Y + (float)model.Radius),
                new PointF(center.X + (float)model.Radius, center.Y - (float)model.Radius),
                new PointF(center.X - (float)model.Radius, center.Y - (float)model.Radius)
            };

            ArcStart = model.ArcStart;
            ArcEnd = model.ArcEnd;
            LineType = model.LineType;
            Filled = model.Filled ?? false;
            Color = model.Color ?? Color.Black;
            DisplayName = model.Type.ToString();
        }

        public void Scale(float factor)
        {
            Points = Points.Select(p => new PointF(p.X * factor, p.Y * factor)).ToArray();
            if (ArcStart is not float start || ArcEnd is not float end)
                return;

            var diff = (end - start) * factor;
            ArcStart = end - diff;
            ArcEnd = start + diff;
        }

        public bool IsPointOnShape(PointF point, float tolerance = 0.25F)
        {
            var rect = Points.GetBoundsRectangle();
            var center = rect.GetCenter();
            var radius = rect.Height / 2;
            var dist = point.GetDistanceToPoint(center);
            if (Filled)
                return dist - radius <= tolerance;

            if (MathF.Abs(radius - point.GetDistanceToPoint(center)) > tolerance)
                return false;

            if (ArcStart is null || ArcEnd is null)
                return true;

            var startAngle = (float)ArcStart;
            var endAngle = (float)ArcEnd;
            var actualAngle = PointFExtensions.GetAngleFromPointOnCircle(center, dist, point);

            return actualAngle >= startAngle && actualAngle <= endAngle;
        }
    }
}

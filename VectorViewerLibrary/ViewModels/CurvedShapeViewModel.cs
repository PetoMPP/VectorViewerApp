using System.Drawing;
using VectorViewerLibrary.Models;

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
        public PointF[] Points { get; }
        public float? ArcStart { get; }
        public float? ArcEnd { get; }
        public bool? Filled { get; }
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

            if (model.ArcStart is not null && model.ArcEnd is not null)
            {
                ArcStart = model.ArcStart;
                ArcEnd = model.ArcStart < model.ArcEnd 
                    ? model.ArcEnd - model.ArcStart
                    : 360 - (model.ArcStart - model.ArcEnd);
            }

            LineType = model.LineType;
            Filled = model.Filled;
            Color = model.Color ?? Color.Black;
            DisplayName = model.Type.ToString();
        }
    }
}

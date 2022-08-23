using System.Drawing;
using VectorViewerLibrary.Extensions;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.ViewModels
{
    public interface ICurvedShapeViewModel : IShapeViewModel
    {
    }

    public class CurvedShapeViewModel : ICurvedShapeViewModel
    {
        public PointF[] Points { get; }
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

            var center = model.Center.ConvertToPointF();

            Points = new PointF[4]
            {
                new PointF(center.X + (float)model.Radius, center.Y + (float)model.Radius),
                new PointF(center.X - (float)model.Radius, center.Y + (float)model.Radius),
                new PointF(center.X + (float)model.Radius, center.Y - (float)model.Radius),
                new PointF(center.X - (float)model.Radius, center.Y - (float)model.Radius)
            };

            Filled = model.Filled;
            Color = model.Color.ConvertToColor();
            DisplayName = model.Type.ToString();
        }
    }
}

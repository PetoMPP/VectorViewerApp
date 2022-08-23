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
        public PointF[] Points { get; }
        public bool? Filled { get; }
        public Color Color { get; }
        public string DisplayName { get; }

        public LinearShapeViewModel(ShapeModel model)
        {
            if (model.Points is null)
                throw new InvalidOperationException("Missing Points in ShapeModel");

            if (model.Color is null)
                throw new InvalidOperationException("Missing Color in ShapeModel");

            var pointCount = model.Points.Length;
            Points = new PointF[pointCount];

            for (int i = 0; i < pointCount; i++)
                Points[i] = model.Points[i].ConvertToPointF();

            Filled = model.Filled;
            Color = model.Color.ConvertToColor();
            DisplayName = model.Type.ToString();
        }
    }
}

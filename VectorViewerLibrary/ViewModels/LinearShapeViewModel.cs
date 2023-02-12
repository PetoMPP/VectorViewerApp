using System.Drawing;
using VectorViewerLibrary.Models;

namespace VectorViewerLibrary.ViewModels
{
    public interface ILinearShapeViewModel : IShapeViewModel
    {
    }

    public class LinearShapeViewModel : ILinearShapeViewModel
    {
        public PointF[] Points { get; private set; }
        public LineType LineType { get; }
        public bool? Filled { get; private set; }
        public Color Color { get; }
        public string DisplayName { get; }

        public LinearShapeViewModel(ShapeModel model)
        {
            if (model.Points is null)
                throw new InvalidOperationException("Missing Points in ShapeModel");

            if (model.Color is null)
                throw new InvalidOperationException("Missing Color in ShapeModel");

            Points = model.Points;
            LineType = model.LineType;
            Filled = model.Filled;
            Color = model.Color ?? Color.Black;
            DisplayName = model.Type.ToString();
        }

        public void Scale(float factor)
        {
            Points = Points.Select(p => new PointF(p.X * factor, p.Y * factor)).ToArray();
        }
    }
}

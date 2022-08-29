using System.Drawing;

namespace VectorViewerLibrary.Models
{
    public class ShapeModel
    {
        private float? _arcStart;
        private float? _arcEnd;

        public ShapeType Type { get; set; }
        public LineType LineType { get; set; }
        public PointF[]? Points { get; set; }
        public PointF? Center { get; set; }
        public float? Radius { get; set; }

        public float? ArcStart
        {
            get => _arcStart;
            set => _arcStart = value >= 90 ? value - 90 : value + 270;
        }

        public float? ArcEnd
        {
            get => _arcEnd;
            set => _arcEnd = value >= 90 ? value - 90 : value + 270;
        }

        public bool? Filled { get; set; }
        public Color? Color { get; set; }
        public ShapeModel[]? Shapes { get; set; }
    }
}

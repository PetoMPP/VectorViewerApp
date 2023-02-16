using System.Drawing;

namespace VectorViewerLibrary.Extensions
{
    public static class RectangleFExtensions
    {
        public static PointF GetCenter(this RectangleF rectangle)
        {
            return new PointF(rectangle.X + (rectangle.Width / 2),
                      rectangle.Y + (rectangle.Height / 2));
        }
    }
}

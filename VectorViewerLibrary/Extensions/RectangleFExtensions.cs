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

        public static IEnumerable<(PointF A, PointF B)> GetSides(this RectangleF rectangle)
        {
            yield return (new PointF(rectangle.Left, rectangle.Top), new PointF(rectangle.Right, rectangle.Top));
            yield return (new PointF(rectangle.Right, rectangle.Top), new PointF(rectangle.Right, rectangle.Bottom));
            yield return (new PointF(rectangle.Right, rectangle.Bottom), new PointF(rectangle.Left, rectangle.Bottom));
            yield return (new PointF(rectangle.Left, rectangle.Bottom), new PointF(rectangle.Left, rectangle.Top));
        }

        public static IEnumerable<PointF> GetCorners(this RectangleF rectangle)
        {
            yield return new PointF(rectangle.Left, rectangle.Top);
            yield return new PointF(rectangle.Right, rectangle.Top);
            yield return new PointF(rectangle.Right, rectangle.Bottom);
            yield return new PointF(rectangle.Left, rectangle.Bottom);
        }
    }
}

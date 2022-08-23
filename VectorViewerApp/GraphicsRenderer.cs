using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Numerics;
using VectorViewerLibrary.Extensions;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerUI
{
    public class GraphicsRenderer : INotifyPropertyChanged
    {
        private Graphics _graphics;
        private PointF _origin;
        private float _zoom = 1;
        private IShapeViewModel? _shape;
        private Image _canvas;
        private Vector2 _originOffset;

        public delegate void RenderingCompleteEventHandler(object? sender, EventArgs eventArgs);

        public event RenderingCompleteEventHandler? RenderingComplete;
        public event PropertyChangedEventHandler? PropertyChanged;

        public float Zoom
        {
            get => _zoom;
            private set
            {
                _zoom = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(Zoom)));
            }
        }

        public IShapeViewModel? Shape
        {
            get => _shape;
            set
            {
                _shape = value;
                _originOffset = Vector2.Zero;
                _origin = GetOrigin();
                CalculateZoom();
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(Shape)));
            }
        }

        public Image Canvas
        {
            get => _canvas;
            set
            {
                _canvas = value;
                _graphics = Graphics.FromImage(_canvas);
                _origin = GetOrigin();
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(Canvas)));
            }
        }

        public Vector2 OriginOffset
        {
            get => _originOffset;
            set
            {
                _originOffset = value;
                _origin = GetOrigin();
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(OriginOffset)));
            }
        }

        public GraphicsRendererSettings Settings { get; }
        public RectangleF Viewport => new(new PointF(0, 0), Canvas.Size);

        public GraphicsRenderer(
            Image image,
            GraphicsRendererSettings? graphicsRendererSettings = null)
        {
            graphicsRendererSettings ??= new GraphicsRendererSettings();
            Settings = graphicsRendererSettings;
            _graphics = Graphics.FromImage(image);
            _canvas = image;
            _origin = GetOrigin();

            Settings.PropertyChanged += (_, _) => Render();
            PropertyChanged += (_, e) => Render();

            RenderBackground();
            RenderAxes();
        }

        public PointF GetCoordinatesAtPoint(Point location)
        {
            var x = (location.X - _origin.X) / Zoom;
            var y = -((location.Y - _origin.Y) / Zoom);
            return new PointF(x, y);
        }

        private void Render()
        {
            _graphics.SmoothingMode = Settings.SmoothingMode;
            RenderBackground();
            RenderAxes();

            if (Shape is null)
            {
                RenderingComplete?.Invoke(this, new EventArgs());
                return;
            }


            if (Shape is ILinearShapeViewModel linearShape)
                RenderLinearShape(linearShape);

            else if (Shape is ICurvedShapeViewModel curvedShape)
                RenderCurvedShape(curvedShape);

            else
                throw new InvalidOperationException();

            RenderingComplete?.Invoke(this, new EventArgs());
        }

        private void CalculateZoom()
        {
            if (Shape is null)
                return;

            var boundsRectangle = Shape.Points
                .TransformPoints(_origin, 1)
                .GetBoundsRectangle();

            if (Shape.Filled != true)
                boundsRectangle.Inflate(
                    Settings.LineThickness / 2,
                    Settings.LineThickness / 2);

            var zoom = MathF.Min(
                Viewport.Width / 2 / MathF.Max(
                    _origin.X - boundsRectangle.Left, _origin.X - boundsRectangle.Right),
                Viewport.Height / 2 / MathF.Max(
                    _origin.Y - boundsRectangle.Top, _origin.Y - boundsRectangle.Bottom));

            zoom *= 1f - Settings.AutoZoomPadding;
            _zoom = zoom;
        }

        private void RenderCurvedShape(ICurvedShapeViewModel shape)
        {
            if (shape.Points.Length != 4)
                throw new InvalidOperationException("Invalid count of Points");

            var color = Settings.IgnoreTransparency
                ? Color.FromArgb(byte.MaxValue, shape.Color)
                : shape.Color;

            var boundsRectangle = shape.Points
                .TransformPoints(_origin, Zoom)
                .GetBoundsRectangle();

            if (shape.Filled == true)
            {
                var brush = new SolidBrush(color);
                _graphics.FillEllipse(brush, boundsRectangle);
                return;
            }

            var pen = new Pen(color, Settings.LineThickness);
            _graphics.DrawEllipse(pen, boundsRectangle);
        }

        private void RenderLinearShape(ILinearShapeViewModel shape)
        {
            var color = Settings.IgnoreTransparency
                ? Color.FromArgb(byte.MaxValue, shape.Color)
                : shape.Color;

            var pen = new Pen(color, Settings.LineThickness);
            var points = shape.Points.TransformPoints(_origin, Zoom);

            switch (points.Length)
            {
                case 2:
                    _graphics.DrawLine(pen, points[0], points[1]);
                    break;
                case > 2:
                    if (shape.Filled == true)
                    {
                        var brush = new SolidBrush(color);
                        _graphics.FillPolygon(brush, points);
                        break;
                    }

                    points = points.Concat(points[..2]).ToArray();
                    _graphics.DrawLines(pen, points);
                    break;
                default:
                    throw new InvalidOperationException("Invalid count of Points");
            }
        }

        private PointF GetOrigin()
        {
            return new PointF(
                (Viewport.Width / 2) + OriginOffset.X,
                (Viewport.Height / 2) + OriginOffset.Y);
        }

        private void RenderAxes()
        {
            var yAxisPoints = new PointF[]
            {
                new PointF(0, _origin.Y),
                new PointF(Viewport.Width, _origin.Y),
            };

            var xAxisPoints = new PointF[]
            {
                new PointF(_origin.X, 0),
                new PointF(_origin.X, Viewport.Height)
            };

            var state = _graphics.Save();
            _graphics.SmoothingMode = SmoothingMode.None;

            var pen = new Pen(Color.DimGray, 1);
            _graphics.DrawLines(pen, yAxisPoints);
            _graphics.DrawLines(pen, xAxisPoints);

            if (Settings.DisplayScale)
                RenderScale();

            _graphics.Restore(state);
        }

        private void RenderScale()
        {
            var brush = new SolidBrush(Color.DimGray);

            var localViewPort = new RectangleF(Viewport.ToVector4() / Zoom);

            // Origin
            _graphics.DrawString(
                "0", Settings.Font, brush, new PointF(_origin.X, _origin.Y));

            var segment = MathF.Min(localViewPort.Width / 5, localViewPort.Height / 5);


            // X axis
            var unit = 1;
            while (unit * 5 < segment)
                unit *= 5;

            var segmentLenght = unit * Zoom;

            var segmentNumber = 1;
            var point = new PointF(_origin.X + segmentLenght, _origin.Y);
            while (point.X < Viewport.Width)
            {
                DrawXAxisSegment(brush, unit, segmentNumber, point);
                point = new PointF(point.X + segmentLenght, point.Y);
                segmentNumber++;
            }

            segmentNumber = -1;
            point = new PointF(_origin.X - segmentLenght, _origin.Y);
            while (point.X > Viewport.X)
            {
                DrawXAxisSegment(brush, unit, segmentNumber, point);
                point = new PointF(point.X - segmentLenght, point.Y);
                segmentNumber--;
            }

            // Y axis
            segmentNumber = 1;
            point = new PointF(_origin.X, _origin.Y + segmentLenght);
            while (point.Y < Viewport.Height)
            {
                DrawYAxisSegment(brush, unit, segmentNumber, point);
                point = new PointF(point.X, point.Y + segmentLenght);
                segmentNumber++;
            }

            segmentNumber = -1;
            point = new PointF(_origin.X, _origin.Y - segmentLenght);
            while (point.Y > Viewport.Y)
            {
                DrawYAxisSegment(brush, unit, segmentNumber, point);
                point = new PointF(point.X, point.Y - segmentLenght);
                segmentNumber--;
            }
        }

        private void DrawYAxisSegment(
            SolidBrush brush, int unit, int segmentNumber, PointF point)
        {
            var pen = new Pen(brush);

            _graphics.DrawLine(
                pen,
                new PointF(point.X + 5, point.Y),
                new PointF(point.X - 5, point.Y));

            var text = MathF.Round(unit * -segmentNumber, 3).ToString();
            var textSize = _graphics.MeasureString(text, Settings.Font);

            _graphics.DrawString(
                text,
                Settings.Font,
                brush,
                new PointF(
                    point.X + 5,
                    point.Y - (textSize.Height / 2)));
        }

        private void DrawXAxisSegment(
            Brush brush, int unit, int segmentNumber, PointF point)
        {
            var pen = new Pen(brush);

            _graphics.DrawLine(
                pen,
                new PointF(point.X, point.Y + 5),
                new PointF(point.X, point.Y - 5));

            var text = MathF.Round(unit * segmentNumber, 3).ToString();
            var textSize = _graphics.MeasureString(text, Settings.Font);

            _graphics.DrawString(
                text, Settings.Font, brush, new PointF(
                    point.X - (textSize.Width / 2),
                    point.Y + 5));
        }

        private void RenderBackground() => _graphics.Clear(Settings.BackgroundColor);
    }
}

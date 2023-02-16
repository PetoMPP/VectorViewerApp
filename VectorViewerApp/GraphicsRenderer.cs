using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Numerics;
using VectorViewerLibrary;
using VectorViewerLibrary.Extensions;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerUI
{
    public class GraphicsRenderer : INotifyPropertyChanged
    {
        private static readonly float[] ContinuousLinePattern = { 1 };
        private static readonly float[] DashedLinePattern = { 10, 10 };
        private static readonly float[] DotDashedLinePattern = { 10, 9, 1, 9 };
        private static readonly float[] HiddenLinePattern = { 20, 10 };

        private readonly IList<IShapeViewModel> _highlightedShapes;

        private Graphics _graphics;
        private PointF _origin;
        private float _zoom = 1;
        private IEnumerable<IShapeViewModel> _shapes;
        private Image _canvas;
        private Vector2 _originOffset;
        private IList<IShapeViewModel> _selectedShapes;

        public delegate void RenderingCompleteEventHandler(object? sender, EventArgs eventArgs);

        public event EventHandler<EventArgs>? RenderingComplete;
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

        public IEnumerable<IShapeViewModel> Shapes
        {
            get => _shapes;
            set
            {
                _shapes = value;
                _highlightedShapes.Clear();
                SelectedShapes.Clear();
                _originOffset = Vector2.Zero;
                _origin = GetOrigin();
                CalculateZoom();
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(Shapes)));
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

        public IList<IShapeViewModel> SelectedShapes
        {
            get => _selectedShapes;
            private set
            {
                _selectedShapes = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(SelectedShapes)));
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
            _shapes = Array.Empty<IShapeViewModel>();
            _graphics = Graphics.FromImage(image);
            _canvas = image;
            _origin = GetOrigin();
            _highlightedShapes = new List<IShapeViewModel>();
            _selectedShapes = new List<IShapeViewModel>();

            Settings.PropertyChanged += (_, _) => Render();
            PropertyChanged += (_, _) => Render();

            Render();
        }

        public void HighlightShapesAtPoint(Point location)
        {
            var hadHighlightedShapes = _highlightedShapes.Any();
            var point = GetCoordinatesAtPoint(location, false);
            foreach (var shape in Shapes)
            {
                if (shape.IsPointOnShape(point, 5 / Zoom))
                {
                    if (!_highlightedShapes.Contains(shape))
                        _highlightedShapes.Add(shape);
                }
                else
                {
                    if (_highlightedShapes.Contains(shape))
                        _highlightedShapes.Remove(shape);
                }
            }

            if (_highlightedShapes.Any() || hadHighlightedShapes)
                Render();
        }

        public PointF GetCoordinatesAtPoint(Point location, bool invertY = true)
        {
            var x = (location.X - _origin.X) / Zoom;
            var y = (location.Y - _origin.Y) / Zoom;
            if (invertY)
                y *= -1;

            return new PointF(x, y);
        }

        public void ZoomIn(Point location)
        {
            var startLocation = GetCoordinatesAtPoint(location);
            Zoom *= 1.1f;
            var newPoint = GetPointAtCoordinates(startLocation);
            OriginOffset += new Vector2(
                location.X - newPoint.X,
                location.Y - newPoint.Y);
        }

        public void ZoomOut(Point location)
        {
            if (Zoom < 0.0001f)
                return;

            var startLocation = GetCoordinatesAtPoint(location);
            Zoom *= 0.9f;
            var newPoint = GetPointAtCoordinates(startLocation);
            OriginOffset += new Vector2(
                location.X - newPoint.X,
                location.Y - newPoint.Y);
        }

        private Point GetPointAtCoordinates(PointF location)
        {
            var x = _origin.X + (location.X * Zoom);
            var y = _origin.Y + (-location.Y * Zoom);
            return new Point((int)x, (int)y);
        }

        private void Render()
        {
            try
            {
                _graphics.SmoothingMode = Settings.SmoothingMode;
                RenderBackground();

                if (Settings.DisplayScale)
                    RenderAxes();

                RenderShapes();
            }
            finally
            {
                RenderingComplete?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SelectHighlightedShapes(bool addToCurrent)
        {
            if (addToCurrent)
            {
                foreach (var shape in _highlightedShapes)
                    SelectedShapes.Add(shape);
            }
            else
            {
                SelectedShapes = _highlightedShapes.ToList();
            }
        }

        private void RenderShapes()
        {
            if (!Shapes.Any())
                return;

            foreach (var shape in Shapes)
            {
                try
                {
                    RenderShape(shape);
                }
                catch (OutOfMemoryException)
                {
                    shape.Scale(0.99F);
                    try
                    {
                        RenderShape(shape);
                    }
                    catch (OutOfMemoryException)
                    {
                    }
                }
            }

            void RenderShape(IShapeViewModel shape)
            {
                if (shape is ILinearShapeViewModel linearShape)
                    RenderLinearShape(linearShape);
                else if (shape is ICurvedShapeViewModel curvedShape)
                    RenderCurvedShape(curvedShape);
                else
                    throw new InvalidOperationException();
            }
        }

        private void CalculateZoom()
        {
            if (!Shapes.Any())
                return;

            var zooms = new List<float>();

            foreach (var shape in Shapes)
            {
                var boundsRectangle = shape.Points
                    .TransformPoints(_origin, 1)
                    .GetBoundsRectangle();

                if (!shape.Filled)
                {
                    boundsRectangle.Inflate(
                        Settings.LineThickness / 2,
                        Settings.LineThickness / 2);
                }

                var zoom = MathF.Min(
                    Viewport.Width / 2 / MathF.Max(
                        MathF.Abs(_origin.X - boundsRectangle.Left), MathF.Abs(_origin.X - boundsRectangle.Right)),
                    Viewport.Height / 2 / MathF.Max(
                        MathF.Abs(_origin.Y - boundsRectangle.Top), MathF.Abs(_origin.Y - boundsRectangle.Bottom)));

                zoom *= 1f - Settings.AutoZoomPadding;
                zooms.Add(zoom);
            }

            _zoom = zooms.Min();
        }

        private void RenderCurvedShape(ICurvedShapeViewModel shape)
        {
            if (shape.Points.Length != 4)
                throw new InvalidOperationException("Invalid count of Points");

            var color = GetShapeColor(shape);
            using var pen = GetPen(shape, color);

            var boundsRectangle = shape.Points
                .TransformPoints(_origin, Zoom)
                .GetBoundsRectangle();

            if (shape.ArcStart is float start && shape.ArcEnd is float end)
            {
                var sweep = start > end
                    ? 360 - start + end
                    : end - start;

                _graphics.DrawArc(pen, boundsRectangle, -end, sweep);
                return;
            }

            if (shape.Filled)
            {
                using var brush = new SolidBrush(color);
                _graphics.FillEllipse(brush, boundsRectangle);
                return;
            }
            _graphics.DrawEllipse(pen, boundsRectangle);
        }

        private Color GetShapeColor(IShapeViewModel shape)
        {
            var color = shape.Color;
            if (_selectedShapes.Contains(shape))
                color = Color.FromArgb(Math.Max(shape.Color.A, (byte)128), Settings.SelectionColor);
            else if (_highlightedShapes.Contains(shape))
                color = Color.FromArgb(Math.Max(shape.Color.A, (byte)128), Settings.HighlightColor);

            if (Settings.IgnoreTransparency)
                color = Color.FromArgb(byte.MaxValue, color);

            return color;
        }

        private Pen GetPen(IShapeViewModel shape, Color color)
        {
            return new Pen(color, Settings.LineThickness)
            {
                DashPattern = shape.LineType switch
                {
                    LineType.Dashed => DashedLinePattern,
                    LineType.DotDashed => DotDashedLinePattern,
                    LineType.Hidden => HiddenLinePattern,
                    _ => ContinuousLinePattern
                }
            };
        }

        private void RenderLinearShape(ILinearShapeViewModel shape)
        {
            var color = GetShapeColor(shape);
            using var pen = GetPen(shape, color);

            var points = shape.Points.TransformPoints(_origin, Zoom);

            switch (points.Length)
            {
                case 2:
                    _graphics.DrawLine(pen, points[0], points[1]);
                    break;
                case > 2:
                    if (shape.Filled)
                    {
                        using var brush = new SolidBrush(color);
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

            var color = Color.FromArgb(
                byte.MaxValue,
                (byte)(Settings.BackgroundColor.R + 127),
                (byte)(Settings.BackgroundColor.G + 127),
                (byte)(Settings.BackgroundColor.B + 127));

            using var pen = new Pen(color, 1);
            _graphics.DrawLines(pen, yAxisPoints);
            _graphics.DrawLines(pen, xAxisPoints);

            RenderScale(color);

            _graphics.Restore(state);
        }

        private void RenderScale(Color color)
        {
            using var brush = new SolidBrush(color);

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
            using var pen = new Pen(brush);

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
            using var pen = new Pen(brush);

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

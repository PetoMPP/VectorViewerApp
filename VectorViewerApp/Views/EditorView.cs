using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Numerics;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerUI.Views
{
    public partial class EditorView : Form
    {
        private const string ZoomLabelTextBase = "Zoom: {0} %";
        private const string ViewportLabelTextBase = "Viewport: {0} x {1}";
        private const string CursorLocationLabelTextBase = "X: {0}  Y: {1}";

        private static readonly ColorDialog ColorDialog = new()
        {
            AllowFullOpen = true,
            SolidColorOnly = true,
            FullOpen = true
        };

        private IList<IViewModel> _shapes;
        private readonly Func<Image, GraphicsRenderer> _graphicsRendererFactory;
        private readonly GraphicsRenderer _renderer;

        private Point _lastMouseLocation;
        private bool _moving;

        public PictureBox DisplayPictureBox => displayPictureBox;

        public EditorView(
            IEnumerable<IViewModel> shapes,
            Func<Image, GraphicsRenderer> graphicsRendererFactory)
        {
            _shapes = shapes.Reverse().ToList();
            _graphicsRendererFactory = graphicsRendererFactory;

            InitializeComponent();
            displayPictureBox.MouseWheel += DisplayPictureBox_MouseWheel;
            displayPictureBox.Image = new Bitmap(
                displayPictureBox.ClientSize.Width, displayPictureBox.ClientSize.Height);

            _renderer = _graphicsRendererFactory(displayPictureBox.Image);
            _renderer.Settings.Font = new Font(Font.FontFamily, 7);
            _renderer.PropertyChanged += Renderer_PropertyChanged;
            _renderer.RenderingComplete += Renderer_RenderingComplete;

            LoadDataToUI();
        }

        public void UpdateViewport()
        {
            displayPictureBox.Image = new Bitmap(
                displayPictureBox.ClientSize.Width, displayPictureBox.ClientSize.Height);

            _renderer.Canvas = displayPictureBox.Image;

            viewportLabel.Text = string.Format(
                ViewportLabelTextBase, displayPictureBox.Width, displayPictureBox.Height);
        }

        public void ChangeShapesContext(IEnumerable<IViewModel> shapes)
        {
            _shapes = shapes.ToList();
            shapesListBox.DataSource = null;
            InitializeShapesList();
        }

        private void InitializeShapesList()
        {
            shapesListBox.DataSource = _shapes;
            shapesListBox.DisplayMember = nameof(IViewModel.DisplayName);
            shapesListBox.SelectionMode = SelectionMode.One;
        }

        private void Renderer_RenderingComplete(object? sender, EventArgs eventArgs)
        {
            displayPictureBox.Invalidate();
        }

        private void Renderer_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            zoomLabel.Text = string.Format(ZoomLabelTextBase, _renderer.Zoom * 100);
        }

        private void LoadDataToUI()
        {
            viewportLabel.Text = string.Format(
                viewportLabel.Text,
                _renderer.Viewport.Width,
                _renderer.Viewport.Height);

            zoomLabel.Text = string.Format(ZoomLabelTextBase, _renderer.Zoom * 100);
            InitializeShapesList();
        }

        private void ShapesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var shapes = new List<IShapeViewModel>();
            var item = shapesListBox.SelectedItem;

            if (item is IShapeViewModel shapeViewModel)
                shapes.Add(shapeViewModel);
            if (item is IMultiShapeViewModel multiViewModel)
                shapes.AddRange(multiViewModel.InnerShapes);

            _renderer.Shapes = shapes;
        }

        private void AliasingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.Settings.SmoothingMode = aliasingCheckBox.Checked
                ? SmoothingMode.AntiAlias
                : SmoothingMode.None;
        }

        private void ScaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.Settings.DisplayScale = scaleCheckBox.Checked;
        }

        private void IgnoreTransparencyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _renderer.Settings.IgnoreTransparency = ignoreTransparencyCheckBox.Checked;
        }

        private void PaddingTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(
                paddingTextBox.Text.Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var padding))
            {
                return;
            }

            _renderer.Settings.AutoZoomPadding = padding / 100f;
        }

        private void LineThicknessTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!float.TryParse(
                lineThicknessTextBox.Text.Trim().Replace(',', '.'),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var lineThickness))
            {
                return;
            }

            _renderer.Settings.LineThickness = lineThickness;
        }

        private void ColorChangeLabel_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = colorChangeLabel.BackColor;
            if (ColorDialog.ShowDialog() != DialogResult.OK)
                return;

            _renderer.Settings.BackgroundColor = ColorDialog.Color;
            colorChangeLabel.BackColor = ColorDialog.Color;
            displayPictureBox.BackColor = ColorDialog.Color;
        }

        private void DisplayPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _moving = true;
            Cursor = Cursors.SizeAll;
            _lastMouseLocation = e.Location;
        }

        private void DisplayPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            var location = _renderer.GetCoordinatesAtPoint(e.Location);
            cursorLocationLabel.Text = string.Format(
                CursorLocationLabelTextBase, location.X, location.Y);

            if (!_moving)
                return;

            _renderer.OriginOffset += new Vector2(
                e.X - _lastMouseLocation.X, e.Y - _lastMouseLocation.Y);
            _lastMouseLocation = e.Location;
        }

        private void DisplayPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _moving = false;
            Cursor = Cursors.Default;
        }

        private void DisplayPictureBox_MouseLeave(object sender, EventArgs e)
        {
            cursorLocationLabel.Text = string.Format(
                CursorLocationLabelTextBase, "-", "-");

            _moving = false;
            Cursor = Cursors.Default;
        }

        private void DisplayPictureBox_MouseEnter(object sender, EventArgs e)
        {
            displayPictureBox.Focus();
        }

        private void DisplayPictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                _renderer.ZoomIn(e.Location);
            else
                _renderer.ZoomOut(e.Location);
        }

        private void EditorView_Shown(object sender, EventArgs e) => UpdateViewport();
    }
}

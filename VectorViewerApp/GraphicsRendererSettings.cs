using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace VectorViewerUI
{
    public class GraphicsRendererSettings : INotifyPropertyChanged
    {
        private SmoothingMode _smoothingMode = SmoothingMode.None;
        private bool _displayScale = false;
        private bool _ignoreTransparency = false;
        private float _autoZoomPadding = 0.1f;
        private float _lineThickness = 0.25f;
        private Color _backgroundColor = Color.DarkGray;
        private Font _font = new(FontFamily.GenericMonospace, 7);

        public SmoothingMode SmoothingMode
        {
            get => _smoothingMode;
            set
            {
                _smoothingMode = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(SmoothingMode)));
            }
        }

        public bool DisplayScale
        {
            get => _displayScale;
            set
            {
                _displayScale = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(DisplayScale)));
            }
        }

        public bool IgnoreTransparency
        {
            get => _ignoreTransparency;
            set
            {
                _ignoreTransparency = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(IgnoreTransparency)));
            }
        }

        public float AutoZoomPadding
        {
            get => _autoZoomPadding;
            set
            {
                _autoZoomPadding = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(AutoZoomPadding)));
            }
        }

        public float LineThickness
        {
            get => _lineThickness;
            set
            {
                _lineThickness = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(LineThickness)));
            }
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(BackgroundColor)));
            }
        }

        public Font Font
        {
            get => _font;
            set
            {
                _font = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(Font)));
            }
        }

        public Color HighlightColor => Color.Blue;
        public Color SelectionColor => Color.Lime;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

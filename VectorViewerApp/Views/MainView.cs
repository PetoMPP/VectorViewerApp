using VectorViewerLibrary.DataReading;
using VectorViewerLibrary.ViewModels;

namespace VectorViewerUI.Views
{
    public partial class MainView : Form
    {
        private readonly IDataProvider _dataProvider;
        private readonly Func<IEnumerable<IViewModel>, EditorView> _editorViewFactory;
        private EditorView? _editorView;

        public MainView(
            IDataProvider dataProvider,
            Func<IEnumerable<IViewModel>, EditorView> editorViewFactory)
        {
            _dataProvider = dataProvider;
            _editorViewFactory = editorViewFactory;
            InitializeComponent();
        }

        private async void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON config files (*.json)|*.json|DXF files (*.dxf)|*.dxf",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var models = await _dataProvider
                .GetShapesFromFile(dialog.FileName, CancellationToken.None);

            SetEditorContext(models);
        }

        private void SetEditorContext(IEnumerable<IViewModel> models)
        {
            if (_editorView is null)
            {
                _editorView = _editorViewFactory(models);
                LoadFormToMainPanel(_editorView);
                return;
            }

            _editorView.ChangeShapesContext(models);
        }

        private void LoadFormToMainPanel(Form form)
        {
            form.Dock = DockStyle.Fill;
            form.TopLevel = false;
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainView_ResizeBegin(object sender, EventArgs e)
        {
            _editorView?.DisplayPictureBox.SuspendLayout();
        }

        private void MainView_ResizeEnd(object sender, EventArgs e)
        {
            _editorView?.DisplayPictureBox.ResumeLayout();
            _editorView?.UpdateViewport();
        }

        private void MainView_Shown(object sender, EventArgs e)
        {
            _editorView = _editorViewFactory(Array.Empty<IShapeViewModel>());
            LoadFormToMainPanel(_editorView);
        }

        private async void MainView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is null)
                return;

            try
            {
                var path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
                var models = await _dataProvider.GetShapesFromFile(path, CancellationToken.None);
                SetEditorContext(models);
            }
            catch
            {
                MessageBox.Show("Invalid file type!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;
        }
    }
}

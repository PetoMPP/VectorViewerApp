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

            _editorView = _editorViewFactory(models);
            LoadFormToMainPanel(_editorView);
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
    }
}

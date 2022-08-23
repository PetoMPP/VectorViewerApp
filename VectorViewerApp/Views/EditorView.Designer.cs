namespace VectorViewerUI.Views
{
    partial class EditorView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.viewportLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.zoomLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataPanel = new System.Windows.Forms.Panel();
            this.shapesListBox = new System.Windows.Forms.ListBox();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.backgorundColorPanel = new System.Windows.Forms.Panel();
            this.colorChangeLabel = new System.Windows.Forms.Label();
            this.backgroundColorLabel = new System.Windows.Forms.Label();
            this.lineThickenessPanel = new System.Windows.Forms.Panel();
            this.lineThicknessTextBox = new System.Windows.Forms.TextBox();
            this.lineThicknessLabel = new System.Windows.Forms.Label();
            this.zoomPaddingPanel = new System.Windows.Forms.Panel();
            this.paddingTextBox = new System.Windows.Forms.TextBox();
            this.paddingLabel = new System.Windows.Forms.Label();
            this.ignoreTransparencyCheckBox = new System.Windows.Forms.CheckBox();
            this.scaleCheckBox = new System.Windows.Forms.CheckBox();
            this.aliasingCheckBox = new System.Windows.Forms.CheckBox();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.shapesLabel = new System.Windows.Forms.Label();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.displayPictureBox = new System.Windows.Forms.PictureBox();
            this.statusStrip.SuspendLayout();
            this.dataPanel.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.backgorundColorPanel.SuspendLayout();
            this.lineThickenessPanel.SuspendLayout();
            this.zoomPaddingPanel.SuspendLayout();
            this.imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.displayPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.BurlyWood;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewportLabel,
            this.zoomLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 378);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(799, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // viewportLabel
            // 
            this.viewportLabel.Name = "viewportLabel";
            this.viewportLabel.Size = new System.Drawing.Size(100, 17);
            this.viewportLabel.Text = "Viewport: {0} x {1}";
            // 
            // zoomLabel
            // 
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(72, 17);
            this.zoomLabel.Text = "Zoom: {0} %";
            // 
            // dataPanel
            // 
            this.dataPanel.BackColor = System.Drawing.Color.Bisque;
            this.dataPanel.Controls.Add(this.shapesListBox);
            this.dataPanel.Controls.Add(this.settingsPanel);
            this.dataPanel.Controls.Add(this.shapesLabel);
            this.dataPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataPanel.Location = new System.Drawing.Point(599, 0);
            this.dataPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.dataPanel.Size = new System.Drawing.Size(200, 378);
            this.dataPanel.TabIndex = 4;
            // 
            // shapesListBox
            // 
            this.shapesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shapesListBox.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.shapesListBox.FormattingEnabled = true;
            this.shapesListBox.ItemHeight = 20;
            this.shapesListBox.Location = new System.Drawing.Point(6, 35);
            this.shapesListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.shapesListBox.Name = "shapesListBox";
            this.shapesListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.shapesListBox.Size = new System.Drawing.Size(188, 138);
            this.shapesListBox.TabIndex = 3;
            this.shapesListBox.SelectedIndexChanged += new System.EventHandler(this.ShapesListBox_SelectedIndexChanged);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.backgorundColorPanel);
            this.settingsPanel.Controls.Add(this.lineThickenessPanel);
            this.settingsPanel.Controls.Add(this.zoomPaddingPanel);
            this.settingsPanel.Controls.Add(this.ignoreTransparencyCheckBox);
            this.settingsPanel.Controls.Add(this.scaleCheckBox);
            this.settingsPanel.Controls.Add(this.aliasingCheckBox);
            this.settingsPanel.Controls.Add(this.settingsLabel);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.settingsPanel.Location = new System.Drawing.Point(6, 173);
            this.settingsPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(188, 200);
            this.settingsPanel.TabIndex = 2;
            // 
            // backgorundColorPanel
            // 
            this.backgorundColorPanel.Controls.Add(this.colorChangeLabel);
            this.backgorundColorPanel.Controls.Add(this.backgroundColorLabel);
            this.backgorundColorPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.backgorundColorPanel.Location = new System.Drawing.Point(0, 159);
            this.backgorundColorPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.backgorundColorPanel.Name = "backgorundColorPanel";
            this.backgorundColorPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.backgorundColorPanel.Size = new System.Drawing.Size(188, 30);
            this.backgorundColorPanel.TabIndex = 8;
            // 
            // colorChangeLabel
            // 
            this.colorChangeLabel.BackColor = System.Drawing.Color.DarkGray;
            this.colorChangeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorChangeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorChangeLabel.Location = new System.Drawing.Point(143, 2);
            this.colorChangeLabel.Name = "colorChangeLabel";
            this.colorChangeLabel.Size = new System.Drawing.Size(42, 26);
            this.colorChangeLabel.TabIndex = 9;
            this.colorChangeLabel.Click += new System.EventHandler(this.ColorChangeLabel_Click);
            // 
            // backgroundColorLabel
            // 
            this.backgroundColorLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.backgroundColorLabel.Location = new System.Drawing.Point(3, 2);
            this.backgroundColorLabel.Name = "backgroundColorLabel";
            this.backgroundColorLabel.Size = new System.Drawing.Size(140, 26);
            this.backgroundColorLabel.TabIndex = 8;
            this.backgroundColorLabel.Text = "Background color:";
            this.backgroundColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lineThickenessPanel
            // 
            this.lineThickenessPanel.Controls.Add(this.lineThicknessTextBox);
            this.lineThickenessPanel.Controls.Add(this.lineThicknessLabel);
            this.lineThickenessPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineThickenessPanel.Location = new System.Drawing.Point(0, 129);
            this.lineThickenessPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lineThickenessPanel.Name = "lineThickenessPanel";
            this.lineThickenessPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lineThickenessPanel.Size = new System.Drawing.Size(188, 30);
            this.lineThickenessPanel.TabIndex = 7;
            // 
            // lineThicknessTextBox
            // 
            this.lineThicknessTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineThicknessTextBox.Location = new System.Drawing.Point(143, 2);
            this.lineThicknessTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lineThicknessTextBox.MaxLength = 5;
            this.lineThicknessTextBox.Name = "lineThicknessTextBox";
            this.lineThicknessTextBox.Size = new System.Drawing.Size(42, 23);
            this.lineThicknessTextBox.TabIndex = 9;
            this.lineThicknessTextBox.Text = "0.25";
            this.lineThicknessTextBox.TextChanged += new System.EventHandler(this.LineThicknessTextBox_TextChanged);
            // 
            // lineThicknessLabel
            // 
            this.lineThicknessLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lineThicknessLabel.Location = new System.Drawing.Point(3, 2);
            this.lineThicknessLabel.Name = "lineThicknessLabel";
            this.lineThicknessLabel.Size = new System.Drawing.Size(140, 26);
            this.lineThicknessLabel.TabIndex = 8;
            this.lineThicknessLabel.Text = "Line thickness:";
            this.lineThicknessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // zoomPaddingPanel
            // 
            this.zoomPaddingPanel.Controls.Add(this.paddingTextBox);
            this.zoomPaddingPanel.Controls.Add(this.paddingLabel);
            this.zoomPaddingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.zoomPaddingPanel.Location = new System.Drawing.Point(0, 99);
            this.zoomPaddingPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.zoomPaddingPanel.Name = "zoomPaddingPanel";
            this.zoomPaddingPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.zoomPaddingPanel.Size = new System.Drawing.Size(188, 30);
            this.zoomPaddingPanel.TabIndex = 6;
            // 
            // paddingTextBox
            // 
            this.paddingTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddingTextBox.Location = new System.Drawing.Point(143, 2);
            this.paddingTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paddingTextBox.MaxLength = 2;
            this.paddingTextBox.Name = "paddingTextBox";
            this.paddingTextBox.Size = new System.Drawing.Size(42, 23);
            this.paddingTextBox.TabIndex = 9;
            this.paddingTextBox.Text = "10";
            this.paddingTextBox.TextChanged += new System.EventHandler(this.PaddingTextBox_TextChanged);
            // 
            // paddingLabel
            // 
            this.paddingLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.paddingLabel.Location = new System.Drawing.Point(3, 2);
            this.paddingLabel.Name = "paddingLabel";
            this.paddingLabel.Size = new System.Drawing.Size(140, 26);
            this.paddingLabel.TabIndex = 8;
            this.paddingLabel.Text = "Auto zoom padding (%):";
            this.paddingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ignoreTransparencyCheckBox
            // 
            this.ignoreTransparencyCheckBox.AutoSize = true;
            this.ignoreTransparencyCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ignoreTransparencyCheckBox.Location = new System.Drawing.Point(0, 76);
            this.ignoreTransparencyCheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ignoreTransparencyCheckBox.Name = "ignoreTransparencyCheckBox";
            this.ignoreTransparencyCheckBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ignoreTransparencyCheckBox.Size = new System.Drawing.Size(188, 23);
            this.ignoreTransparencyCheckBox.TabIndex = 4;
            this.ignoreTransparencyCheckBox.Text = "Ignore shape transparency";
            this.ignoreTransparencyCheckBox.UseVisualStyleBackColor = true;
            this.ignoreTransparencyCheckBox.CheckedChanged += new System.EventHandler(this.IgnoreTransparencyCheckBox_CheckedChanged);
            // 
            // scaleCheckBox
            // 
            this.scaleCheckBox.AutoSize = true;
            this.scaleCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.scaleCheckBox.Location = new System.Drawing.Point(0, 53);
            this.scaleCheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scaleCheckBox.Name = "scaleCheckBox";
            this.scaleCheckBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scaleCheckBox.Size = new System.Drawing.Size(188, 23);
            this.scaleCheckBox.TabIndex = 3;
            this.scaleCheckBox.Text = "Display scale";
            this.scaleCheckBox.UseVisualStyleBackColor = true;
            this.scaleCheckBox.CheckedChanged += new System.EventHandler(this.ScaleCheckBox_CheckedChanged);
            // 
            // aliasingCheckBox
            // 
            this.aliasingCheckBox.AutoSize = true;
            this.aliasingCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.aliasingCheckBox.Location = new System.Drawing.Point(0, 30);
            this.aliasingCheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.aliasingCheckBox.Name = "aliasingCheckBox";
            this.aliasingCheckBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.aliasingCheckBox.Size = new System.Drawing.Size(188, 23);
            this.aliasingCheckBox.TabIndex = 2;
            this.aliasingCheckBox.Text = "Enable AntiAliasing";
            this.aliasingCheckBox.UseVisualStyleBackColor = true;
            this.aliasingCheckBox.CheckedChanged += new System.EventHandler(this.AliasingCheckBox_CheckedChanged);
            // 
            // settingsLabel
            // 
            this.settingsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.settingsLabel.ForeColor = System.Drawing.Color.Crimson;
            this.settingsLabel.Location = new System.Drawing.Point(0, 0);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(188, 30);
            this.settingsLabel.TabIndex = 1;
            this.settingsLabel.Text = "Display settings:";
            this.settingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapesLabel
            // 
            this.shapesLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.shapesLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.shapesLabel.ForeColor = System.Drawing.Color.Crimson;
            this.shapesLabel.Location = new System.Drawing.Point(6, 5);
            this.shapesLabel.Name = "shapesLabel";
            this.shapesLabel.Size = new System.Drawing.Size(188, 30);
            this.shapesLabel.TabIndex = 0;
            this.shapesLabel.Text = "Available shapes:";
            this.shapesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imagePanel
            // 
            this.imagePanel.BackColor = System.Drawing.Color.Bisque;
            this.imagePanel.Controls.Add(this.displayPictureBox);
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Padding = new System.Windows.Forms.Padding(15);
            this.imagePanel.Size = new System.Drawing.Size(599, 378);
            this.imagePanel.TabIndex = 5;
            // 
            // displayPictureBox
            // 
            this.displayPictureBox.BackColor = System.Drawing.Color.DarkGray;
            this.displayPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.displayPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayPictureBox.Location = new System.Drawing.Point(15, 15);
            this.displayPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.displayPictureBox.Name = "displayPictureBox";
            this.displayPictureBox.Size = new System.Drawing.Size(569, 348);
            this.displayPictureBox.TabIndex = 0;
            this.displayPictureBox.TabStop = false;
            this.displayPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DisplayPictureBox_MouseDown);
            this.displayPictureBox.MouseLeave += new System.EventHandler(this.DisplayPictureBox_MouseLeave);
            this.displayPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisplayPictureBox_MouseMove);
            this.displayPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DisplayPictureBox_MouseUp);
            // 
            // EditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(799, 400);
            this.Controls.Add(this.imagePanel);
            this.Controls.Add(this.dataPanel);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "EditorView";
            this.Text = "EditorView";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.dataPanel.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.backgorundColorPanel.ResumeLayout(false);
            this.lineThickenessPanel.ResumeLayout(false);
            this.lineThickenessPanel.PerformLayout();
            this.zoomPaddingPanel.ResumeLayout(false);
            this.zoomPaddingPanel.PerformLayout();
            this.imagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.displayPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip;
        private Panel dataPanel;
        private ToolStripStatusLabel viewportLabel;
        private ToolStripStatusLabel zoomLabel;
        private Label shapesLabel;
        private Panel imagePanel;
        private PictureBox displayPictureBox;
        private ListBox shapesListBox;
        private Panel settingsPanel;
        private Panel zoomPaddingPanel;
        private CheckBox ignoreTransparencyCheckBox;
        private CheckBox scaleCheckBox;
        private CheckBox aliasingCheckBox;
        private Label settingsLabel;
        private Label paddingLabel;
        private Panel lineThickenessPanel;
        private TextBox lineThicknessTextBox;
        private Label lineThicknessLabel;
        private TextBox paddingTextBox;
        private Panel backgorundColorPanel;
        private Label colorChangeLabel;
        private Label backgroundColorLabel;
    }
}
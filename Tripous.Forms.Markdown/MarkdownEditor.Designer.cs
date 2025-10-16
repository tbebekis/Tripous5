namespace Tripous.Forms
{
    partial class MarkdownEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MarkdownEditor));
            MarkdownTextBox.MarkdownPolicy markdownPolicy1 = new MarkdownTextBox.MarkdownPolicy();
            Splitter = new SplitContainer();
            fMarkdownBox = new MarkdownTextBox();
            ToolBar = new ToolStrip();
            ((ISupportInitialize)Splitter).BeginInit();
            Splitter.Panel1.SuspendLayout();
            Splitter.SuspendLayout();
            ((ISupportInitialize)fMarkdownBox).BeginInit();
            SuspendLayout();
            // 
            // Splitter
            // 
            Splitter.Dock = DockStyle.Fill;
            Splitter.Location = new Point(0, 0);
            Splitter.Name = "Splitter";
            // 
            // Splitter.Panel1
            // 
            Splitter.Panel1.Controls.Add(fMarkdownBox);
            Splitter.Panel1.Controls.Add(ToolBar);
            Splitter.Size = new Size(822, 604);
            Splitter.SplitterDistance = 391;
            Splitter.SplitterWidth = 6;
            Splitter.TabIndex = 0;
            // 
            // fMarkdownBox
            // 
            fMarkdownBox.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            fMarkdownBox.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            fMarkdownBox.AutoScrollMinSize = new Size(0, 14);
            fMarkdownBox.BackBrush = null;
            fMarkdownBox.BackColor = Color.FromArgb(30, 30, 30);
            fMarkdownBox.BracketsStyle = null;
            fMarkdownBox.CaretColor = Color.White;
            fMarkdownBox.CharHeight = 14;
            fMarkdownBox.CharWidth = 8;
            fMarkdownBox.CurrentLineColor = Color.FromArgb(40, 45, 55);
            fMarkdownBox.DelayedTextChangedInterval = 120;
            fMarkdownBox.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            fMarkdownBox.Dock = DockStyle.Fill;
            fMarkdownBox.ForeColor = Color.FromArgb(230, 230, 230);
            fMarkdownBox.Hotkeys = resources.GetString("fMarkdownBox.Hotkeys");
            fMarkdownBox.IndentBackColor = Color.Transparent;
            fMarkdownBox.IsReplaceMode = false;
            fMarkdownBox.LineNumberColor = Color.FromArgb(130, 140, 150);
            fMarkdownBox.Location = new Point(0, 25);
            fMarkdownBox.Name = "fMarkdownBox";
            fMarkdownBox.Paddings = new Padding(0);
            fMarkdownBox.Policy = markdownPolicy1;
            fMarkdownBox.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            fMarkdownBox.ServiceColors = (ServiceColors)resources.GetObject("fMarkdownBox.ServiceColors");
            fMarkdownBox.ServiceLinesColor = Color.FromArgb(55, 65, 80);
            fMarkdownBox.Size = new Size(391, 579);
            fMarkdownBox.TabIndex = 1;
            fMarkdownBox.WordWrap = true;
            fMarkdownBox.WordWrapIndent = 2;
            fMarkdownBox.Zoom = 100;
            // 
            // ToolBar
            // 
            ToolBar.ImageScalingSize = new Size(24, 24);
            ToolBar.Location = new Point(0, 0);
            ToolBar.Name = "ToolBar";
            ToolBar.Size = new Size(391, 25);
            ToolBar.TabIndex = 0;
            ToolBar.Text = "toolStrip1";
            // 
            // UC_MarkdownWriter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Splitter);
            Name = "UC_MarkdownWriter";
            Size = new Size(822, 604);
            Splitter.Panel1.ResumeLayout(false);
            Splitter.Panel1.PerformLayout();
            ((ISupportInitialize)Splitter).EndInit();
            Splitter.ResumeLayout(false);
            ((ISupportInitialize)fMarkdownBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer Splitter;
        private ToolStrip ToolBar;
        private MarkdownTextBox fMarkdownBox;
    }
}

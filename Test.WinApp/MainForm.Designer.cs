
namespace Test.WinApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            StatusBar = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            ProgressBar = new ToolStripProgressBar();
            ToolBar = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            Splitter = new SplitContainer();
            Pager = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            edtLog = new RichTextBox();
            StatusBar.SuspendLayout();
            ToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Splitter).BeginInit();
            Splitter.Panel1.SuspendLayout();
            Splitter.Panel2.SuspendLayout();
            Splitter.SuspendLayout();
            Pager.SuspendLayout();
            SuspendLayout();
            // 
            // StatusBar
            // 
            StatusBar.Items.AddRange(new ToolStripItem[] { lblStatus, ProgressBar });
            StatusBar.Location = new Point(0, 439);
            StatusBar.Name = "StatusBar";
            StatusBar.Size = new Size(784, 22);
            StatusBar.TabIndex = 2;
            StatusBar.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = false;
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(300, 17);
            // 
            // ProgressBar
            // 
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(180, 16);
            // 
            // ToolBar
            // 
            ToolBar.ImageScalingSize = new Size(24, 24);
            ToolBar.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
            ToolBar.Location = new Point(0, 0);
            ToolBar.Name = "ToolBar";
            ToolBar.Size = new Size(784, 31);
            ToolBar.TabIndex = 3;
            ToolBar.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(28, 28);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // Splitter
            // 
            Splitter.Cursor = Cursors.HSplit;
            Splitter.Dock = DockStyle.Fill;
            Splitter.Location = new Point(0, 31);
            Splitter.Name = "Splitter";
            Splitter.Orientation = Orientation.Horizontal;
            // 
            // Splitter.Panel1
            // 
            Splitter.Panel1.Controls.Add(Pager);
            // 
            // Splitter.Panel2
            // 
            Splitter.Panel2.Controls.Add(edtLog);
            Splitter.Size = new Size(784, 408);
            Splitter.SplitterDistance = 281;
            Splitter.TabIndex = 4;
            // 
            // Pager
            // 
            Pager.Controls.Add(tabPage1);
            Pager.Controls.Add(tabPage2);
            Pager.Dock = DockStyle.Fill;
            Pager.Location = new Point(0, 0);
            Pager.Name = "Pager";
            Pager.SelectedIndex = 0;
            Pager.Size = new Size(784, 281);
            Pager.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(776, 253);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(776, 253);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // edtLog
            // 
            edtLog.BackColor = SystemColors.Control;
            edtLog.Dock = DockStyle.Fill;
            edtLog.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point);
            edtLog.Location = new Point(0, 0);
            edtLog.Name = "edtLog";
            edtLog.Size = new Size(784, 123);
            edtLog.TabIndex = 0;
            edtLog.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(Splitter);
            Controls.Add(ToolBar);
            Controls.Add(StatusBar);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Test.WinApp";
            WindowState = FormWindowState.Maximized;
            StatusBar.ResumeLayout(false);
            StatusBar.PerformLayout();
            ToolBar.ResumeLayout(false);
            ToolBar.PerformLayout();
            Splitter.Panel1.ResumeLayout(false);
            Splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Splitter).EndInit();
            Splitter.ResumeLayout(false);
            Pager.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer Splitter;
        private System.Windows.Forms.TabControl Pager;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox edtLog;
    }
}


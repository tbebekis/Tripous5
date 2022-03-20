namespace Tripous.Forms
{
    partial class MultiRowPickDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.btnIncludeAll = new System.Windows.Forms.ToolStripButton();
            this.btnExcludeAll = new System.Windows.Forms.ToolStripButton();
            this.btnShowIdColumns = new System.Windows.Forms.ToolStripButton();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.toolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 529);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 32);
            this.panel1.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(629, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 26);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(706, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // toolBar
            // 
            this.toolBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnIncludeAll,
            this.btnExcludeAll,
            this.btnShowIdColumns});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(784, 31);
            this.toolBar.TabIndex = 2;
            this.toolBar.Text = "toolStrip1";
            // 
            // btnIncludeAll
            // 
            this.btnIncludeAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnIncludeAll.Image = global::Tripous.Forms.Properties.Resources.cart_put;
            this.btnIncludeAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIncludeAll.Name = "btnIncludeAll";
            this.btnIncludeAll.Size = new System.Drawing.Size(28, 28);
            this.btnIncludeAll.Text = "Include all";
            // 
            // btnExcludeAll
            // 
            this.btnExcludeAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExcludeAll.Image = global::Tripous.Forms.Properties.Resources.cart_remove;
            this.btnExcludeAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExcludeAll.Name = "btnExcludeAll";
            this.btnExcludeAll.Size = new System.Drawing.Size(28, 28);
            this.btnExcludeAll.Text = "Exclude all";
            // 
            // btnShowIdColumns
            // 
            this.btnShowIdColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowIdColumns.Image = global::Tripous.Forms.Properties.Resources.page_paintbrush;
            this.btnShowIdColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowIdColumns.Name = "btnShowIdColumns";
            this.btnShowIdColumns.Size = new System.Drawing.Size(28, 28);
            this.btnShowIdColumns.Text = "Show Id Columns";
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 31);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(784, 498);
            this.Grid.TabIndex = 3;
            // 
            // MultiRowPickDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "MultiRowPickDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multiple Row Selection";
            this.panel1.ResumeLayout(false);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton btnIncludeAll;
        private System.Windows.Forms.ToolStripButton btnExcludeAll;
        private System.Windows.Forms.ToolStripButton btnShowIdColumns;
        private System.Windows.Forms.DataGridView Grid;
    }
}
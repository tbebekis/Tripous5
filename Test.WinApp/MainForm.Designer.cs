
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
            this.Pager = new System.Windows.Forms.TabControl();
            this.tabTSqlParser = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.edtSql = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnParseSql = new System.Windows.Forms.Button();
            this.Pager.SuspendLayout();
            this.tabTSqlParser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pager
            // 
            this.Pager.Controls.Add(this.tabTSqlParser);
            this.Pager.Controls.Add(this.tabPage2);
            this.Pager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pager.Location = new System.Drawing.Point(0, 0);
            this.Pager.Name = "Pager";
            this.Pager.SelectedIndex = 0;
            this.Pager.Size = new System.Drawing.Size(990, 722);
            this.Pager.TabIndex = 0;
            // 
            // tabTSqlParser
            // 
            this.tabTSqlParser.Controls.Add(this.splitContainer1);
            this.tabTSqlParser.Controls.Add(this.panel1);
            this.tabTSqlParser.Location = new System.Drawing.Point(4, 24);
            this.tabTSqlParser.Name = "tabTSqlParser";
            this.tabTSqlParser.Padding = new System.Windows.Forms.Padding(3);
            this.tabTSqlParser.Size = new System.Drawing.Size(982, 694);
            this.tabTSqlParser.TabIndex = 0;
            this.tabTSqlParser.Text = "Sql Parser";
            this.tabTSqlParser.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(982, 694);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 50);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.edtSql);
            this.splitContainer1.Size = new System.Drawing.Size(976, 641);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 0;
            // 
            // edtSql
            // 
            this.edtSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edtSql.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.edtSql.Location = new System.Drawing.Point(0, 0);
            this.edtSql.Name = "edtSql";
            this.edtSql.Size = new System.Drawing.Size(976, 302);
            this.edtSql.TabIndex = 0;
            this.edtSql.Text = resources.GetString("edtSql.Text");
            this.edtSql.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnParseSql);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(976, 47);
            this.panel1.TabIndex = 1;
            // 
            // btnParseSql
            // 
            this.btnParseSql.Location = new System.Drawing.Point(5, 11);
            this.btnParseSql.Name = "btnParseSql";
            this.btnParseSql.Size = new System.Drawing.Size(184, 30);
            this.btnParseSql.TabIndex = 0;
            this.btnParseSql.Text = "Parse";
            this.btnParseSql.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 722);
            this.Controls.Add(this.Pager);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test.WinApp";
            this.Pager.ResumeLayout(false);
            this.tabTSqlParser.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Pager;
        private System.Windows.Forms.TabPage tabTSqlParser;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox edtSql;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnParseSql;
    }
}


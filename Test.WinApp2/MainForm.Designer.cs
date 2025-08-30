namespace Test.WinApp2
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
            panel1 = new Panel();
            btnTest1 = new Button();
            edtLog = new RichTextBox();
            btnTest2 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnTest2);
            panel1.Controls.Add(btnTest1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(830, 100);
            panel1.TabIndex = 0;
            // 
            // btnTest1
            // 
            btnTest1.Location = new Point(56, 33);
            btnTest1.Name = "btnTest1";
            btnTest1.Size = new Size(116, 32);
            btnTest1.TabIndex = 0;
            btnTest1.Text = "Test 1";
            btnTest1.UseVisualStyleBackColor = true;
            // 
            // edtLog
            // 
            edtLog.BackColor = Color.Gainsboro;
            edtLog.Dock = DockStyle.Fill;
            edtLog.Font = new Font("Courier New", 9F);
            edtLog.Location = new Point(0, 100);
            edtLog.Name = "edtLog";
            edtLog.Size = new Size(830, 604);
            edtLog.TabIndex = 1;
            edtLog.Text = "";
            // 
            // btnTest2
            // 
            btnTest2.Location = new Point(207, 37);
            btnTest2.Name = "btnTest2";
            btnTest2.Size = new Size(75, 23);
            btnTest2.TabIndex = 1;
            btnTest2.Text = "Test 2";
            btnTest2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(830, 704);
            Controls.Add(edtLog);
            Controls.Add(panel1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Test.WinApp2";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private RichTextBox edtLog;
        private Button btnTest1;
        private Button btnTest2;
    }
}

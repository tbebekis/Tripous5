namespace Tripous.Forms
{
    partial class MessageBoxDialog
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
            pnlBase = new Panel();
            pnlContent = new Panel();
            Memo = new RichTextBox();
            pnlTop = new Panel();
            picBox = new PictureBox();
            edtTitle = new TextBox();
            pnlBottom = new Panel();
            btnYes = new Button();
            btnNo = new Button();
            btnCancel = new Button();
            pnlBase.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlTop.SuspendLayout();
            ((ISupportInitialize)picBox).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBase
            // 
            pnlBase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlBase.Controls.Add(pnlContent);
            pnlBase.Controls.Add(pnlBottom);
            pnlBase.Location = new Point(7, 7);
            pnlBase.Margin = new Padding(4, 3, 4, 3);
            pnlBase.Name = "pnlBase";
            pnlBase.Size = new Size(534, 359);
            pnlBase.TabIndex = 2;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(Memo);
            pnlContent.Controls.Add(pnlTop);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 0);
            pnlContent.Margin = new Padding(4, 3, 4, 3);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(534, 316);
            pnlContent.TabIndex = 1;
            // 
            // Memo
            // 
            Memo.Dock = DockStyle.Fill;
            Memo.Font = new Font("Microsoft Sans Serif", 8.25F);
            Memo.Location = new Point(0, 46);
            Memo.Margin = new Padding(4, 3, 4, 3);
            Memo.Name = "Memo";
            Memo.Size = new Size(534, 270);
            Memo.TabIndex = 1;
            Memo.Text = "";
            Memo.WordWrap = false;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(picBox);
            pnlTop.Controls.Add(edtTitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 3, 4, 3);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(534, 46);
            pnlTop.TabIndex = 0;
            // 
            // picBox
            // 
            picBox.ImeMode = ImeMode.NoControl;
            picBox.Location = new Point(7, 5);
            picBox.Margin = new Padding(4, 3, 4, 3);
            picBox.Name = "picBox";
            picBox.Size = new Size(37, 37);
            picBox.TabIndex = 1;
            picBox.TabStop = false;
            // 
            // edtTitle
            // 
            edtTitle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            edtTitle.BorderStyle = BorderStyle.FixedSingle;
            edtTitle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            edtTitle.Location = new Point(83, 7);
            edtTitle.Margin = new Padding(4, 3, 4, 3);
            edtTitle.Name = "edtTitle";
            edtTitle.ReadOnly = true;
            edtTitle.Size = new Size(449, 20);
            edtTitle.TabIndex = 0;
            edtTitle.TabStop = false;
            edtTitle.Text = "edtTitle";
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnYes);
            pnlBottom.Controls.Add(btnNo);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 316);
            pnlBottom.Margin = new Padding(4, 3, 4, 3);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(534, 43);
            pnlBottom.TabIndex = 0;
            // 
            // btnYes
            // 
            btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnYes.ImeMode = ImeMode.NoControl;
            btnYes.Location = new Point(247, 7);
            btnYes.Margin = new Padding(4, 3, 4, 3);
            btnYes.Name = "btnYes";
            btnYes.Size = new Size(91, 30);
            btnYes.TabIndex = 2;
            btnYes.Text = "btnYes";
            // 
            // btnNo
            // 
            btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNo.ImeMode = ImeMode.NoControl;
            btnNo.Location = new Point(343, 7);
            btnNo.Margin = new Padding(4, 3, 4, 3);
            btnNo.Name = "btnNo";
            btnNo.Size = new Size(91, 30);
            btnNo.TabIndex = 1;
            btnNo.Text = "btnNo";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.ImeMode = ImeMode.NoControl;
            btnCancel.Location = new Point(438, 7);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(91, 30);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "btnCancel";
            // 
            // MessageBoxDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 373);
            Controls.Add(pnlBase);
            Margin = new Padding(4, 3, 4, 3);
            Name = "MessageBoxDialog";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MessageBoxDialog";
            pnlBase.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ((ISupportInitialize)picBox).EndInit();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.RichTextBox Memo;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.TextBox edtTitle;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnCancel;
    }
}
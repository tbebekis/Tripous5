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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxDialog));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pnlBase = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.Memo = new System.Windows.Forms.RichTextBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.edtTitle = new System.Windows.Forms.TextBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlBase.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "ERROR_ICO.PNG");
            this.imageList.Images.SetKeyName(1, "EXCLAMATION_ICO.PNG");
            this.imageList.Images.SetKeyName(2, "INFORMATION_ICO.PNG");
            this.imageList.Images.SetKeyName(3, "QUESTION_ICO.PNG");
            // 
            // pnlBase
            // 
            this.pnlBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBase.Controls.Add(this.pnlContent);
            this.pnlBase.Controls.Add(this.pnlBottom);
            this.pnlBase.Location = new System.Drawing.Point(6, 6);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(458, 311);
            this.pnlBase.TabIndex = 2;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.Memo);
            this.pnlContent.Controls.Add(this.pnlTop);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(458, 274);
            this.pnlContent.TabIndex = 1;
            // 
            // Memo
            // 
            this.Memo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Memo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Memo.Location = new System.Drawing.Point(0, 40);
            this.Memo.Name = "Memo";
            this.Memo.Size = new System.Drawing.Size(458, 234);
            this.Memo.TabIndex = 1;
            this.Memo.Text = "";
            this.Memo.WordWrap = false;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.picBox);
            this.pnlTop.Controls.Add(this.edtTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(458, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // picBox
            // 
            this.picBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picBox.Location = new System.Drawing.Point(6, 4);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(32, 32);
            this.picBox.TabIndex = 1;
            this.picBox.TabStop = false;
            // 
            // edtTitle
            // 
            this.edtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.edtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edtTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.edtTitle.Location = new System.Drawing.Point(71, 6);
            this.edtTitle.Name = "edtTitle";
            this.edtTitle.ReadOnly = true;
            this.edtTitle.Size = new System.Drawing.Size(385, 20);
            this.edtTitle.TabIndex = 0;
            this.edtTitle.TabStop = false;
            this.edtTitle.Text = "edtTitle";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnYes);
            this.pnlBottom.Controls.Add(this.btnNo);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 274);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(458, 37);
            this.pnlBottom.TabIndex = 0;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYes.Location = new System.Drawing.Point(212, 6);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(78, 26);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "btnYes";
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNo.Location = new System.Drawing.Point(294, 6);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(78, 26);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "btnNo";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(375, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 26);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "btnCancel";
            // 
            // MessageBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 323);
            this.Controls.Add(this.pnlBase);
            this.Name = "MessageBoxDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MessageBoxDialog";
            this.pnlBase.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
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
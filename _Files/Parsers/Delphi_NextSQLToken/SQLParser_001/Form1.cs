using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using bt.Data.SQLParser;

namespace SQLParser_000
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.TextBox Memo;
      private System.Windows.Forms.ListBox ListBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.button1 = new System.Windows.Forms.Button();
         this.Memo = new System.Windows.Forms.TextBox();
         this.ListBox = new System.Windows.Forms.ListBox();
         this.SuspendLayout();
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(500, 448);
         this.button1.Name = "button1";
         this.button1.TabIndex = 0;
         this.button1.Text = "button1";
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // Memo
         // 
         this.Memo.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(161)));
         this.Memo.Location = new System.Drawing.Point(30, 14);
         this.Memo.Multiline = true;
         this.Memo.Name = "Memo";
         this.Memo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.Memo.Size = new System.Drawing.Size(620, 161);
         this.Memo.TabIndex = 1;
         this.Memo.Text = "update\r\n  Customers\r\nset\r\n  NAME = \'Takis\'\r\nwhere\r\n   ID = :ID\r\nand NAME <> (:NAM" +
            "E)\r\n/* paparies */";
         // 
         // ListBox
         // 
         this.ListBox.Location = new System.Drawing.Point(35, 189);
         this.ListBox.Name = "ListBox";
         this.ListBox.Size = new System.Drawing.Size(391, 303);
         this.ListBox.TabIndex = 2;
         // 
         // Form1
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(689, 539);
         this.Controls.Add(this.ListBox);
         this.Controls.Add(this.Memo);
         this.Controls.Add(this.button1);
         this.Name = "Form1";
         this.Text = "Form1";
         this.ResumeLayout(false);

      }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

      private void button1_Click(object sender, System.EventArgs e)
      {
         //public static TokenKind NextToken(string SQL, int P, ref string Token, TokenKind CurSection)
         
         ListBox.Items.Clear();
         string SQL = Memo.Text;
         string Token = "";
         int P = 0;
         TokenKind CurSection = TokenKind.Unknown;
         string S;
         
         while (true)
         {
            CurSection = Tokenizer.NextToken(SQL, ref P, ref Token, CurSection);
            S = string.Format("{0} -> {1}", CurSection.ToString(), Token);
            ListBox.Items.Add(S);
            if (CurSection == TokenKind.End)
              break;

         }
      }
	}
}

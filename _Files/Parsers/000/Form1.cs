using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using bt.Parsers;

namespace SQLPareser_001
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
      private System.Windows.Forms.ListBox ListBox;
      private System.Windows.Forms.TextBox mmoSource;
      private System.Windows.Forms.TextBox mmoTarget;
      private System.Windows.Forms.Button btnProcess;
      private System.Windows.Forms.Button btnReplace;
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
         this.ListBox = new System.Windows.Forms.ListBox();
         this.btnProcess = new System.Windows.Forms.Button();
         this.mmoSource = new System.Windows.Forms.TextBox();
         this.mmoTarget = new System.Windows.Forms.TextBox();
         this.btnReplace = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // ListBox
         // 
         this.ListBox.Location = new System.Drawing.Point(21, 20);
         this.ListBox.Name = "ListBox";
         this.ListBox.Size = new System.Drawing.Size(187, 524);
         this.ListBox.TabIndex = 0;
         // 
         // btnProcess
         // 
         this.btnProcess.Location = new System.Drawing.Point(324, 443);
         this.btnProcess.Name = "btnProcess";
         this.btnProcess.Size = new System.Drawing.Size(149, 23);
         this.btnProcess.TabIndex = 1;
         this.btnProcess.Text = "Process";
         this.btnProcess.Click += new System.EventHandler(this.button1_Click);
         // 
         // mmoSource
         // 
         this.mmoSource.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(161)));
         this.mmoSource.Location = new System.Drawing.Point(230, 28);
         this.mmoSource.Multiline = true;
         this.mmoSource.Name = "mmoSource";
         this.mmoSource.Size = new System.Drawing.Size(401, 189);
         this.mmoSource.TabIndex = 2;
         this.mmoSource.Text = "select \r\n  * \r\nfrom \r\n// σχόλιο\r\n  CUSTOMERS\r\nwhere \r\n       NAME = \'τακης\'\r\n and" +
            " LNAME = :LNAME\r\n and AGE >= 35";
         // 
         // mmoTarget
         // 
         this.mmoTarget.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(161)));
         this.mmoTarget.Location = new System.Drawing.Point(233, 240);
         this.mmoTarget.Multiline = true;
         this.mmoTarget.Name = "mmoTarget";
         this.mmoTarget.Size = new System.Drawing.Size(401, 189);
         this.mmoTarget.TabIndex = 3;
         this.mmoTarget.Text = "";
         // 
         // btnReplace
         // 
         this.btnReplace.Location = new System.Drawing.Point(322, 472);
         this.btnReplace.Name = "btnReplace";
         this.btnReplace.Size = new System.Drawing.Size(150, 23);
         this.btnReplace.TabIndex = 4;
         this.btnReplace.Text = "Replace";
         this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
         // 
         // Form1
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(672, 606);
         this.Controls.Add(this.btnReplace);
         this.Controls.Add(this.mmoTarget);
         this.Controls.Add(this.mmoSource);
         this.Controls.Add(this.btnProcess);
         this.Controls.Add(this.ListBox);
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
      private void button1_Click(object sender, System.EventArgs e)
      {
         ListBox.Items.Clear();
         
         string S = mmoSource.Text;   
         string SQL = "";      
                                  
         Tokenizer Tokenizer = new Tokenizer();
         Tokenizer.SetString(S);
        
         Token T = null;
         
         try
         {
         
            while (true)
            {
               T = Tokenizer.NextToken();
               S = T.Kind.Name;
               S = S + " -> " + T.AsString;

               
               ListBox.Items.Add(S);
               if (T.Kind == Token.TT_EOF)
               {
                  this.mmoTarget.Text = SQL;
                  return;     
               }
               else if (T.Kind == Token.TT_NEWLINE)
               {
                  SQL = SQL + "\r\n";                  
               }
               else 
               {
                  SQL = SQL + T.AsString;
               }             

                             
            }
         }
         catch
         {
            MessageBox.Show("ERROR");
         }
         
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnReplace_Click(object sender, System.EventArgs e)
      {
         ListBox.Items.Clear();
         
         string S = mmoSource.Text;   
         string SQL = "";      
         char Prefix = ':';
         bool IsVariable = false;
                                  
         Tokenizer Tokenizer = new Tokenizer();
         Tokenizer.SetString(S);
        
         Token T = null;
         
         try
         {
         
            while (true)
            {
               T = Tokenizer.NextToken();
               S = T.Kind.Name;
               S = S + " -> " + T.AsString;

               
               ListBox.Items.Add(S);
               if (T.Kind == Token.TT_EOF)
               {
                  this.mmoTarget.Text = SQL;
                  return;     
               }
               else if (T.Kind == Token.TT_NEWLINE)
               {
                  SQL = SQL + "\r\n";                  
               }
               else if (T.Kind == Token.TT_SYMBOL)
               {
                  IsVariable = (T.AsString.IndexOf(Prefix) == 0);
                  if (!IsVariable)
                     SQL = SQL + T.AsString;
               }
               else if (T.Kind == Token.TT_WORD)
               {
                  if (IsVariable)
                  {
                     //SQL = SQL + '@' + T.AsString;
                     SQL = SQL + "?";
                     IsVariable = false;
                  }
                  else SQL = SQL + T.AsString;                       
               }               
               else 
               {
                  SQL = SQL + T.AsString;
               }             

                              
            }
         }
         catch
         {
            MessageBox.Show("ERROR");
         }
      }


	}
}

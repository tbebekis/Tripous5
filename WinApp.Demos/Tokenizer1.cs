using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tripous.Tokenizing;
using Tripous.Parsing;

namespace WinApp.Demos
{
    public partial class Tokenizer1 : UserControl
    {
        public DemoTokenizer1 Demo;

        void AnyClick(object sender, EventArgs ea)
        {
            if (btnExecute == sender)
                Demo.Execute(edtEditor.Text);
        }

        void ControlInitialize()
        {
            btnExecute.Click += AnyClick;
        }

        public void Clear()
        {
            edtLog.Clear();
        }
        public void Append(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                edtLog.AppendText(Text);
            }
        }
        public void AppendLine(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                edtLog.AppendText(Text + Environment.NewLine);
            }
        }
        public void Log(string Text = null)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Clear();
            }
            else
            {
                AppendLine(Text);
            }
        }

        public Tokenizer1()
        {
            InitializeComponent();

            if (!DesignMode)
                ControlInitialize();
        }
    }



    public class DemoTokenizer1 : IDemo
    {
        Tokenizer1 fControl;

        public void ShowUi(TabPage Page)
        {
            if (fControl == null)
            {
                fControl = new Tokenizer1();
                fControl.Parent = Page;
                fControl.Dock = DockStyle.Fill;

                Page.Text = this.Title;
                fControl.Demo = this;
            }
        }

        public void Execute(string Text)
        {
            fControl.Clear();
            StringBuilder SB = new StringBuilder();
            TokenAssembly A = new TokenAssembly(Text);
            while (A.HasMoreElements())
            {
                fControl.AppendLine(A.NextElement());
            }

         }

        public bool Singleton => false;
        public string Title => "Tokenizer 1";
        public string Description => "TokenAssembly demo. Uses the NextElement() ";
    }



}

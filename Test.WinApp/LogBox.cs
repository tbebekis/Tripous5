using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.WinApp
{
    public class LogBox
    {
         
        public LogBox(TextBoxBase Box)
        {
            this.Box = Box;
        }


        public void Clear()
        {
            Box.Clear();
        }
        public void Append(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                Box.AppendText(Text);
            }
        }
        public void Log(string Text = null)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Box.Clear();
            }
            else
            {
                Box.AppendText(Text);
            }
        }

        public TextBoxBase Box { get; private set; }
    }
}

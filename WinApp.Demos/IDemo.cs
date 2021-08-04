using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp.Demos
{
    public interface IDemo
    {
        void ShowUi(TabPage Page);
        bool Singleton { get; }
        string Title { get; }
        string Description { get; }
    }
}

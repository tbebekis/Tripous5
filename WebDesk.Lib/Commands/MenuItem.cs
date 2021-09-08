using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{

    public class MenuItem
    {
        public MenuItem()
        {
        }

        public MenuItem Add(string Title, string Command = "")
        {
            if (Items == null)
                Items = new List<MenuItem>();

            MenuItem Result = new MenuItem() { Title = Title, Command = Command };
            Items.Add(Result);
            return Result;
        }

        public string Title { get; set; }
        public string Command { get; set; }        
        public List<MenuItem> Items { get; set; }
    }
}

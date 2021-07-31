using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{


    public class CommandGroup
    {
        public CommandGroup()
        {
        }

        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Where this view belongs, e.g. Desk, Desk.Menu, etc
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// A name unique among all groups
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Resource string key
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Icon key
        /// </summary>
        public string IconKey { get; set; }
    }

    /// <summary>
    /// The command type. What a command does when it is called.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// Displays a Ui. The command's Tag is the name of Ui View
        /// </summary>
        Ui = 0,
        /// <summary>
        /// Executes a procedure. The command's Tag is the name procedure
        /// </summary>
        Proc = 1,
    }
 

    public class Command
    {
        public Command()
        {
        }

        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Database Id
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// The command type. What a command does when it is called.
        /// </summary>
        public CommandType Type { get; set; }
        /// <summary>
        /// A name unique among all commands in command's group
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Resource string key
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Icon key
        /// </summary>
        public string IconKey { get; set; }
        /// <summary>
        /// Gets meaning according to command's type
        /// </summary>
        public string Tag { get; set; }
    }
}

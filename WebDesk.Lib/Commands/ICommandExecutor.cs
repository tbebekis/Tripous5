using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLib
{
    /// <summary>
    /// Executes commands
    /// </summary>
    public interface ICommandExecutor
    {
        /// <summary>
        /// Executes a specified command.
        /// </summary>
        void Execute(Command Cmd, Dictionary<string, object> Args);
        /// <summary>
        /// Returns true if a specified command can be executed by this executor.
        /// </summary>
        bool CanExecute(Command Cmd);

        /* properties */
        /// <summary>
        /// The executor's name
        /// </summary>
        string Name { get; }
    }
}

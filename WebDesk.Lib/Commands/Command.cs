using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;

namespace WebDesk
{
    /// <summary>
    /// A command or action
    /// </summary>
    public class Command
    {
        static object syncLock = new LockObject();
        static List<ICommandExecutor> Executors = new List<ICommandExecutor>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Command()
        {
        }

        /* static */
        /// <summary>
        /// Registers a command executor.
        /// </summary>
        static public void Register(ICommandExecutor Executor)
        {
            lock (syncLock)
                Executors.Add(Executor);
        }
        /// <summary>
        /// Unregisters  a command executor.
        /// </summary>
        static public void UnRegister(ICommandExecutor Executor)
        {
            lock (syncLock)
                Executors.Remove(Executor);
        }
        /// <summary>
        /// Finds the right executor and uses it to execute a specified command.
        /// </summary>
        static public void Execute(Command Cmd, Dictionary<string, object> Args)
        {
            lock (syncLock)
            {
                foreach (ICommandExecutor Executor in Executors)
                {
                    if (Executor.CanExecute(Cmd))
                    {
                        Executor.Execute(Cmd, Args);
                        return;
                    }
                }
            }
        }

        /* properties */
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The command type. What a command does when it is called.
        /// </summary>
        public CommandType Type { get; set; }
        /// <summary>
        /// A name unique among all commands.
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

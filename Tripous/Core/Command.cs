using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

 

namespace Tripous
{
    /// <summary>
    /// A command or action
    /// </summary>
    public class Command
    {
        static object syncLock = new LockObject();
        static List<ICommandExecutor> Executors = new List<ICommandExecutor>();

        string fTitleKey;
 

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
        /// Returns the command result if an executor handles the specified command. Else null.
        /// </summary>
        static public object Execute(Command Cmd, Dictionary<string, object> Args = null)
        {
            lock (syncLock)
            {
                foreach (ICommandExecutor Executor in Executors)
                {
                    if (Executor.CanExecute(Cmd))
                    {
                        return Executor.Execute(Cmd, Args);
                    }
                }

                return null;
            }
        }
        /// <summary>
        /// Finds the right executor and uses it to execute a specified command.
        /// Returns the command result if an executor handles the specified command. Else null.
        /// </summary>
        static public object ExecuteByName(string Name, Dictionary<string, object> Args = null)
        {
            lock (syncLock)
            {
                foreach (ICommandExecutor Executor in Executors)
                {
                    if (Executor.CanExecuteByName(Name))
                    {
                        return Executor.ExecuteByName(Name, Args);
                    }
                }

                return null;
            }
        }
        /// <summary>
        /// Finds the right executor and uses it to execute a specified command.
        /// Returns the command result if an executor handles the specified command. Else null.
        /// </summary>
        static public object ExecuteById(string Id, Dictionary<string, object> Args = null)
        {
            lock (syncLock)
            {
                foreach (ICommandExecutor Executor in Executors)
                {
                    if (Executor.CanExecuteById(Id))
                    {
                        return Executor.ExecuteById(Id, Args);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Adds a child command
        /// </summary>
        public Command Add(Command Cmd)
        {
            if (Items == null)
                Items = new List<Command>();
            Items.Add(Cmd);

            return Cmd;
        }
        /// <summary>
        /// Adds a child command as a Ui command
        /// </summary>
        public Command AddUi(Command Cmd)
        {
            Cmd.Type = CommandType.Ui;
            return Add(Cmd);
        }
        /// <summary>
        /// Adds a child command as a Proc command
        /// </summary>
        public Command AddProc(Command Cmd)
        {
            Cmd.Type = CommandType.Proc;
            return Add(Cmd); 
        }

        /// <summary>
        /// Adds a child command as a Ui command
        /// </summary>
        public Command AddUi(string Name, string TitleKey = "")
        {
            if (string.IsNullOrWhiteSpace(TitleKey))
                TitleKey = Name;

            Command Result = new Command() { Type = CommandType.Ui , Name = Name, TitleKey = TitleKey };
            return Add(Result);
        }
        /// <summary>
        /// Adds a child command as a Proc command
        /// </summary>
        public Command AddProc(string Name, string TitleKey = "")
        {
            if (string.IsNullOrWhiteSpace(TitleKey))
                TitleKey = Name;

            Command Result = new Command() { Type = CommandType.Proc, Name = Name, TitleKey = TitleKey };
            return Add(Result);
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
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name; }
            set { }
        }


        /// <summary>
        /// Icon key
        /// </summary>
        public string IconKey { get; set; }
        /// <summary>
        /// True when this is a single instance Ui command.
        /// </summary>
        public bool IsSingleInstance { get; set; }
 

        /// <summary>
        /// The list of child commands, if any, else null.
        /// </summary>
        public List<Command> Items { get; set; }
        /// <summary>
        /// User defined parameters
        /// </summary>
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}

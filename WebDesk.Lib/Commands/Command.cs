using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Tripous;

namespace WebLib
{
    /// <summary>
    /// A command or action
    /// </summary>
    public class Command
    {
        static object syncLock = new LockObject();
        static List<ICommandExecutor> Executors = new List<ICommandExecutor>();

        internal class Setup
        {
            Command Cmd;

            public Setup(Command Cmd)
            {
                this.Cmd = Cmd;
            }
            /// <summary>
            /// A name unique among all commands.
            /// </summary>
            public string Name => Cmd.Name;
            /// <summary>
            /// The command type. What a command does when it is called.
            /// </summary>
            public CommandType Type => Cmd.Type;
            /// <summary>
            /// True when this is a single instance Ui command.
            /// </summary>
            public bool IsSingleInstance => Cmd.IsSingleInstance;
            /// <summary>
            /// User defined parameters
            /// </summary>
            public Dictionary<string, object> Params => Cmd.Params;
        }


        Setup fSetup;

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

        /// <summary>
        /// Serializes this instance in order to properly used as a data-* html attribute.
        /// </summary>
        public string SerializeSetup()
        {
            if (this.fSetup == null)
                fSetup = new Setup(this);

            string JsonText = Json.Serialize(fSetup);
            return JsonText;
        }
        /// <summary>
        /// Adds a child command
        /// </summary>
        public void Add(Command Cmd)
        {
            if (Items == null)
                Items = new List<Command>();
            Items.Add(Cmd);
        }
        /// <summary>
        /// Adds a Ui child command
        /// </summary>
        public Command Add(CommandType Type, string Name, string TitleKey = "")
        {
            if (string.IsNullOrWhiteSpace(TitleKey))
                TitleKey = Name;

            Command Result = new Command() { Type = Type, Name = Name, TitleKey = TitleKey};
            Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds a Ui child command
        /// </summary>
        public Command Add(string Name, string TitleKey = "")
        {
            return Add(CommandType.Ui, Name, TitleKey);
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
        /// User defined parameters
        /// </summary>
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// True when this is a single instance Ui command.
        /// </summary>
        public bool IsSingleInstance { get; set; }

        /// <summary>
        /// Returns the localized title.
        /// </summary>
        [JsonIgnore]
        public string Title => DataStore.Localize(TitleKey);

        /// <summary>
        /// The list of child commands, if any, else null.
        /// </summary>
        public List<Command> Items { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json; 

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

        /// <summary>
        /// Serializes this instance in order to properly used as a data-* html attribute.
        /// </summary>
        public string SerializeSetup()
        {
            Dictionary<string, string> Setup = new Dictionary<string, string>();

            Setup[nameof(Name)] = Name;
            Setup[nameof(Type)] = Type.ToString();
            Setup[nameof(Tag)] = Tag;

            string JsonText = Json.Serialize(Setup);
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
        /// Adds a child command
        /// </summary>
        public Command Add(string Name, string TitleKey = "", CommandType Type = CommandType.Ui, string Tag = "")
        {
            if (string.IsNullOrWhiteSpace(TitleKey))
                TitleKey = Name;

            Command Result = new Command() { Name = Name, TitleKey = TitleKey, Type = Type, Tag = Tag};
            Add(Result);
            return Result;
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

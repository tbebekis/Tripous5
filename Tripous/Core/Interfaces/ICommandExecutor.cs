namespace Tripous
{
    /// <summary>
    /// Executes commands
    /// </summary>
    public interface ICommandExecutor
    {
        /// <summary>
        /// Executes a specified command and returns a result.
        /// <para>CAUTION: When an executor handles a command it always has to return a not null result.</para>
        /// </summary>
        object Execute(Command Cmd, Dictionary<string, object> Args = null);
        /// <summary>
        /// Returns true if a specified command can be executed by this executor.
        /// </summary>
        bool CanExecute(Command Cmd);

        /// <summary>
        /// Executes a specified command and returns a result.
        /// <para>CAUTION: When an executor handles a command it always has to return a not null result.</para>
        /// </summary>
        object ExecuteByName(string Name, Dictionary<string, object> Args = null);
        /// <summary>
        /// Returns true if a specified command can be executed by this executor.
        /// </summary>
        bool CanExecuteByName(string Name);

        /// <summary>
        /// Executes a specified command and returns a result.
        /// <para>CAUTION: When an executor handles a command it always has to return a not null result.</para>
        /// </summary>
        object ExecuteById(string Id, Dictionary<string, object> Args = null);
        /// <summary>
        /// Returns true if a specified command can be executed by this executor.
        /// </summary>
        bool CanExecuteById(string Id);

        /* properties */
        /// <summary>
        /// The executor's name
        /// </summary>
        string Name { get; }
    }
}

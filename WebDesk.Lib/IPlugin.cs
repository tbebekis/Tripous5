namespace WebLib
{
 

    /// <summary>
    /// A dynamically loadable external plugin.
    /// <para>NOTE: An external binary component is an assembly with a filename as ewm_FILE_NAME.dll. It uses ewm_ as a filename prefix. </para>
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Called by the web site to initialize the plugin.
        /// </summary>
        void Initialize(IWebAppContext App);
        /// <summary>
        /// Called by the system. 
        /// <para>Instructs plugin to add any object to object mappings may have by calling either:</para>
        /// <para><c>App.AddObjectMap(Type Source, Type Dest, bool TwoWay = false)</c></para>
        /// <para>or the passed in Configurator object which in the current implementantion is an AutoMapper.IMapperConfigurationExpression instance </para>
        /// </summary>
        void AddObjectMaps(object Configurator);

        /// <summary>
        /// Calls a plugin to add commands for the main menu of the application
        /// </summary>
        void AddMainMenuCommands(List<Command> CommandList);
    }
}

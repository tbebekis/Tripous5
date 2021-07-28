using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{
 

    /// <summary>
    /// A dynamically loadable external module.
    /// <para>NOTE: An external module is an assembly with a filename as ewm_FILE_NAME.dll. It uses ewm_ as a filename prefix. </para>
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tripous;
using Tripous.Web;

namespace WebDesk
{
    /// <summary>
    /// Application settings, coming from appsettings.json
    /// </summary>
    public class AppSettings
    {

        /// <summary>
        /// Plugins folder path
        /// </summary>
        public string PluginFolder { get; set; }
        /// <summary>
        /// Security settings
        /// </summary>
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        /// <summary>
        /// HSTS settings
        /// <para>SEE: https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security </para>
        /// </summary>
        public HSTSSettings HSTS { get; set; } = new HSTSSettings();

    }
}

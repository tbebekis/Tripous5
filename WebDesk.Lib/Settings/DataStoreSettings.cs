using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDesk
{

    /// <summary>
    /// Application settings
    /// </summary>
    public class DataStoreSettings
    {
        /// <summary>
        /// General settings
        /// </summary>
        public GeneralSettings General { get; set; } = new GeneralSettings();
        /// <summary>
        /// Http related settings
        /// </summary>
        public HttpSettings Http { get; set; } = new HttpSettings();
    }

}

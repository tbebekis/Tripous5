using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Tripous;
using Tripous.Data;

namespace WebDesk
{
    /// <summary>
    /// A module definition
    /// </summary>
    public class ModuleDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; } = Sys.GenId(true);
        /// <summary>
        /// A name unique among all instances of this type
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
        /// The author (company, person, etc) who created this instance
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Creation date-time
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Modification date-time
        /// </summary>
        public DateTime ModifiedOn { get; set; }
    }

     



}

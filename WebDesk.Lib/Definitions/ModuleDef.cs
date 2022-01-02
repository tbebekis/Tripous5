using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Tripous;
using Tripous.Data;

namespace WebLib
{
    /// <summary>
    /// A module definition
    /// </summary>
    public class ModuleDef
    {
        string fTitleKey;

        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; } = Sys.GenId(true);
        /// <summary>
        /// A name unique among all instances of this type
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

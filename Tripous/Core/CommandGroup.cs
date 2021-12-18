using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

namespace Tripous
{

    /// <summary>
    /// A group of <see cref="Command"/> items.
    /// </summary>
    public class CommandGroup
    {
 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public CommandGroup()
        {
        }

        /* properties */
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Where this view belongs, e.g. Desk, Desk.Menu, etc
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// A name unique among all instances of this type
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLib
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
        /// Resource string key
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Icon key
        /// </summary>
        public string IconKey { get; set; }
    }
}

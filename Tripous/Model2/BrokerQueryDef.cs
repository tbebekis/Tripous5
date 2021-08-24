﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Tripous.Model2
{

    /// <summary>
    /// Describes a SELECT statement.
    /// </summary>
    public class BrokerQueryDef
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerQueryDef()
        {
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
 
        /// <summary>
        /// Clears the property values of this instance.
        /// </summary>
        public void Clear()
        {
            BrokerQueryDef Empty = new BrokerQueryDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(BrokerQueryDef Source)
        {
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public BrokerQueryDef Clone()
        {
            BrokerQueryDef Result = new BrokerQueryDef();
            Sys.AssignObject(this, Result);
            return Result;
        }

        /* properties */
        /// <summary>
        /// The field name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The SELECT statement
        /// </summary>
        public string Sql { get; set; }
        /// <summary>
        /// A string list, where each string  has the format FIELD_NAME=TitleKey.
        /// <para>Determines the visibility of the fields in the drop-down grids: 
        /// if it is empty then all fields are visible  
        /// else only the included fields are visible  
        /// </para>
        /// </summary>
        public List<string> FieldTitleKeys { get; set; } = new List<string>();
    }

}

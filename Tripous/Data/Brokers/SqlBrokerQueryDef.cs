using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Tripous.Data
{

    /// <summary>
    /// Describes a SELECT statement.
    /// </summary>
    public class SqlBrokerQueryDef
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrokerQueryDef()
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
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            // TODO: CheckDescriptor()
        }

        /// <summary>
        /// Clears the property values of this instance.
        /// </summary>
        public void Clear()
        {
            SqlBrokerQueryDef Empty = new SqlBrokerQueryDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(SqlBrokerQueryDef Source)
        {
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public SqlBrokerQueryDef Clone()
        {
            SqlBrokerQueryDef Result = new SqlBrokerQueryDef();
            Sys.AssignObject(this, Result);
            return Result;
        }

        /* properties */
        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The SELECT statement
        /// </summary>
        public string SqlText { get; set; }
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

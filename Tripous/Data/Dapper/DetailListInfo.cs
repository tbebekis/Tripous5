using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Entity/table information for a detail entity
    /// </summary>
    public class DetailListInfo
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public DetailListInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailListInfo(DetailListDescriptor Descriptor)
        {
            this.MasterListPropertyName = Descriptor.MasterListPropertyName;
            this.DetailEntityName = Descriptor.DetailEntityTypeName;
            this.DetailKeyPropertyName = Descriptor.DetailKeyPropertyName;
        }

        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return DetailEntityName;
        }

        /// <summary>
        /// The name of a property in the master entity. A list/collection property where detail entity instances are kept.
        /// </summary>
        public string MasterListPropertyName { get; set; }
        /// <summary>
        /// The detail entity name
        /// </summary>
        public string DetailEntityName { get; set; }
        /// <summary>
        /// The name of property in the detail entity. That property/field, matches the master entity/table primary key field/property.
        /// </summary>
        public string DetailKeyPropertyName { get; set; }

    }


}

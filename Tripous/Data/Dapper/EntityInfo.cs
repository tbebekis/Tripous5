using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Entity/table information. A kind of special Packet.
    /// </summary>
    public class EntityInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityInfo(EntityDescriptor Descriptor)
        {
            EntityName = Descriptor.EntityName;
            TableName = Descriptor.TableName;
            PrimaryKeys = Descriptor.PrimaryKeys;
            Autoincrement = Descriptor.Autoincrement;
            Mode = Descriptor.Mode;
            ConnectionName = Descriptor.ConnectionName;

            // fields
            foreach (var Field in Descriptor.Fields)
            {
                Fields.Add(new PropInfo(Field));
            }

            // master id property
            DetailKeyPropertyName = Descriptor.DetailKeyPropertyName;
            if (Descriptor.MasterEntityType != null)
                MasterEntityTypeName = Descriptor.MasterEntityType.Name;

            // relational lists, aka details
            foreach (var RelationalList in Descriptor.DetailLists)
            {
                this.RelationalLists.Add(new DetailListInfo(RelationalList));
            }


            // relational properties
            foreach (var Relational in Descriptor.Relationals)
            {
                this.Relationals.Add(new RelationalInfo(Relational));

            }
                
        }

        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{EntityName}:{TableName}";
        }

        /// <summary>
        /// Returns a field info object based on a specified field name, if any, else null.
        /// </summary>
        public PropInfo FindByFieldName(string FieldName)
        {
            return Fields.FirstOrDefault(item => Sys.IsSameText(FieldName, item.FieldName));
        }
        /// <summary>
        /// Returns a field info object based on a specified property name, if any, else null.
        /// </summary>
        public PropInfo FindByPropertyName(string PropertyName)
        {
            return Fields.FirstOrDefault(item => Sys.IsSameText(PropertyName, item.PropertyName));
        }

        /// <summary>
        /// The entity name
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// A semicolon delimited list of field names. 
        /// </summary>
        public string PrimaryKeys { get; set; }
        /// <summary>
        /// True when the table provides a single field primary key that is an auto-increment integer.
        /// </summary>
        public bool Autoincrement { get; set; }
        /// <summary>
        /// It's a bit-field property. Indicates the allowable CRUD operations in a database table
        /// </summary>
        public CRUDMode Mode { get; set; }
        /// <summary>
        /// The name of the connection in config file that represents the database this table belongs to.
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// The name of the master (foreign) entity/table of this entity/table, if any, else null.
        /// </summary>
        public string MasterEntityTypeName { get; set; }
        /// <summary>
        /// A name of a property of this entity/table that is a foreign key to a master entity/table, if any, else null.
        /// </summary>
        public string DetailKeyPropertyName { get; set; }

        /// <summary>
        /// The list of fields
        /// </summary>
        public List<PropInfo> Fields { get; set; } = new List<PropInfo>();

        /// <summary>
        /// Detail table information, if any.
        /// </summary>
        public List<DetailListInfo> RelationalLists { get; } = new List<DetailListInfo>();
        /// <summary>
        /// A list with information about relationals properties, if any.
        /// </summary>
        public List<RelationalInfo> Relationals { get; } = new List<RelationalInfo>();
    }

}

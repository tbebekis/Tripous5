using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Tripous.Data
{

    /// <summary>
    /// A registry of tables
    /// </summary>
    static public class EntityDescriptors
    {
        /// <summary>
        /// The table attribute
        /// </summary>
        static public readonly Type TableAttributeType = typeof(EntityAttribute);
        /// <summary>
        /// The field attribute
        /// </summary>
        static public readonly Type FieldAttributeType = typeof(PropAttribute);


        /* public */
        /// <summary>
        /// Loads all entity types marked with the table attribute of a specified assembly
        /// </summary>
        static public void Load(Assembly A)
        { 
            Dictionary<object, Type> Dic = TypeFinder.FindClassesMarkedWith(TableAttributeType, A);

            Type EntityType;
            EntityAttribute TableAttribute;
            EntityDescriptor Table;
            foreach (var Entry in Dic)
            {
                TableAttribute = Entry.Key as EntityAttribute;
                EntityType = Entry.Value;

                if (!Contains(EntityType))
                {
                    Table = new EntityDescriptor(EntityType, TableAttribute);
                    Items.Add(Table);
                }
            }
        }


        /// <summary>
        /// Finds and returns a table descriptor by a specified entity type, if any, else null.
        /// </summary>
        static public EntityDescriptor Find(Type EntityType)
        {
            EntityDescriptor Result = Items.FirstOrDefault((item) => item.EntityType == EntityType);
            return Result;
        }
        /// <summary>
        /// Finds and returns a table descriptor by a specified entity type, if any, else throws an exception.
        /// </summary>
        static public EntityDescriptor Get(Type EntityType)
        {
            EntityDescriptor Result = Find(EntityType);

            if (Result == null)
                Sys.Error($"No TableDescriptor found for: {EntityType.Name}");

            return Result;
        }
        /// <summary>
        /// Returns the index of a table descriptor by a specified entity type, if any, else -1.
        /// </summary>
        static public int IndexOf(Type EntityType)
        {
            EntityDescriptor Result = Find(EntityType);
            return Items.IndexOf(Result);
        }
        /// <summary>
        /// Returns true if a table descriptor exists by a specified entity type, else false.
        /// </summary>
        static public bool Contains(Type EntityType)
        {
            return Find(EntityType) != null;
        }

        /// <summary>
        /// Finds and returns a table descriptor by table name, if any, else null.
        /// <para>NOTE: The specified name could be the entity name or the database table name.</para>
        /// </summary>
        static public EntityDescriptor Find(string TableName)
        {
            EntityDescriptor Result = Items.FirstOrDefault((item) => item.EntityName.IsSameText(TableName));
            if (Result == null)
                Result = Items.FirstOrDefault((item) => item.TableName.IsSameText(TableName));
            return Result;
        }
        /// <summary>
        /// Finds and returns a table descriptor by table name, if any, else throws an exception.
        /// <para>NOTE: The specified name could be the entity name or the database table name.</para>
        /// </summary>
        static public EntityDescriptor Get(string TableName)
        {
            EntityDescriptor Result = Find(TableName);

            if (Result == null)
                Sys.Error($"No TableDescriptor found for: {TableName}");

            return Result;
        }

        /*

                /// <summary>
                /// Returns the index of a table descriptor by table name, if any, else -1.
                /// <para>NOTE: The specified name could be the entity name or the database table name.</para>
                /// </summary>
                static public int IndexOf(string TableName)
                {
                    TableDescriptor Table = Find(TableName);
                    return Items.IndexOf(Table);
                }
                /// <summary>
                /// Returns true if a table descriptor exists by table name, else false.
                /// <para>NOTE: The specified name could be the entity name or the database table name.</para>
                /// </summary>
                static public bool Contains(string TableName)
                {
                    return Find(TableName) != null;
                }
                     */

        /* properties */
        /// <summary>
        /// The list of registered tables
        /// </summary>
        static public List<EntityDescriptor> Items { get; } = new List<EntityDescriptor>();
    }
}

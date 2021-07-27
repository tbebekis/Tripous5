using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

using Dapper;


namespace Tripous.Data
{

    /// <summary>
    /// Describes a detail table/entity
    /// </summary>
    public class DetailListDescriptor
    {
        EntityDescriptor fDetailDescriptor;
        PropDescriptor fDetailKeyFieldDescriptor;

        MethodInfo fAddMethod;
        PropertyInfo fCountProperty;

        string fSelectDetailsSql;
        string fDeleteDetailsSql;

        MethodInfo AddMethod
        {
            get
            {
                if (fAddMethod == null)
                {
                    fAddMethod = MasterListProperty.PropertyType.GetMethod("Add");
                }
                return fAddMethod;
            }
        }
        PropertyInfo CountProperty
        {
            get
            {
                if (fCountProperty == null)
                {
                    fCountProperty = MasterListProperty.PropertyType.GetProperty("Count");
                }

                return fCountProperty;
            }

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DetailListDescriptor(PropertyInfo MasterListProperty, Type DetailEntityType, string DetailKeyPropertyName)
        {
            this.MasterListProperty = MasterListProperty;
            this.DetailEntityType = DetailEntityType;
            this.DetailKeyPropertyName = DetailKeyPropertyName;
        }

        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return DetailEntityType.Name;
        }

        /// <summary>
        /// Adds a detail entity to the MasterListPropertyName property
        /// </summary>
        public void AddDetailEntity(DataEntity MasterEntity, object DetailEntity)
        {
            var List = MasterListProperty.GetValue(MasterEntity);
            AddMethod.Invoke(List, new[] { DetailEntity });
        }
        /// <summary>
        /// Adds a list of detail entities to the MasterListPropertyName property
        /// </summary>
        public void AddDetailEntities(DataEntity MasterEntity, IEnumerable<object> DetailEntities)
        {
            var List = MasterListProperty.GetValue(MasterEntity);

            foreach (var Entity in DetailEntities)
            {
                AddMethod.Invoke(List, new[] { Entity });
            }
        }
        /// <summary>
        /// Returns the MasterListPropertyName property value (the List) as an IEnumerable
        /// </summary>
        public IEnumerable GetListAsEnumerable(DataEntity MasterEntity)
        {
            var List = MasterListProperty.GetValue(MasterEntity);
            return List as IEnumerable;
        }
        /// <summary>
        /// Returns the number of detail entitiels in the MasterListPropertyName property value (the List) 
        /// </summary>
        public int GetDetailCount(DataEntity MasterEntity)
        {
            var List = MasterListProperty.GetValue(MasterEntity);

            return (int)CountProperty.GetValue(List);
        }


        /* properties */
        /// <summary>
        /// The list property on the master entity/table.
        /// </summary>
        public PropertyInfo MasterListProperty { get; private set; }
        /// <summary>
        /// The name of a property in the master entity. A list/collection property where detail entity instances are kept.
        /// </summary>
        public string MasterListPropertyName { get { return MasterListProperty.Name; } }
        /// <summary>
        /// The type of the detail entity
        /// </summary>
        public Type DetailEntityType { get; private set; }
        /// <summary>
        /// The type name of the detail entity
        /// </summary>
        public string DetailEntityTypeName { get { return DetailEntityType.Name; } }
        /// <summary>
        /// The name of property in the detail entity. That property/field, matches the master entity/table primary key field/property.
        /// </summary>
        public string DetailKeyPropertyName { get; set; }

        /// <summary>
        /// Gets the table descriptor of the detail entity/table
        /// </summary>
        public EntityDescriptor DetailDescriptor
        {
            get
            {
                if (fDetailDescriptor == null)
                    fDetailDescriptor = EntityDescriptors.Get(DetailEntityType);

                return fDetailDescriptor;
            }
        }
        /// <summary>
        /// Gets a field descriptor of the detail entity/table. That property/field, matches the master entity/table primary key field/property.
        /// </summary>
        public PropDescriptor DetailKeyFieldDescriptor
        {
            get
            {
                if (fDetailKeyFieldDescriptor == null)
                {
                    fDetailKeyFieldDescriptor = DetailDescriptor.Fields.First(item => Sys.IsSameText(DetailKeyPropertyName, item.PropertyName));
                }

                return fDetailKeyFieldDescriptor;
            }
        }

        /// <summary>
        /// Sql statement text for conducting a SELECT with a WHERE clause which selects all records from the detail table, based on a master id specified by a Sql parameter.
        /// </summary>
        public string SelectDetailsSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fSelectDetailsSql))
                {
                    string FieldName = DetailKeyFieldDescriptor.FieldName;
                    string ParamName = DetailDescriptor.Provider.CreateParamName(FieldName);

                    StringBuilder SB = new StringBuilder();
                    SB.AppendLine(DetailDescriptor.SelectSql);
                    SB.AppendLine("where ");
                    SB.AppendLine($"  {FieldName} = {ParamName} ");

                    fSelectDetailsSql = SB.ToString();
                }

                return fSelectDetailsSql;
            }
            set
            {
                fSelectDetailsSql = value;
            }
        }
        /// <summary>
        /// Sql statement text for conducting a DELETE with a WHERE clause which deletes all records from the detail table, based on a master id specified by a Sql parameter.
        /// </summary>
        public string DeleteDetailsSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fDeleteDetailsSql))
                {
                    string FieldName = DetailKeyFieldDescriptor.FieldName;
                    string ParamName = DetailDescriptor.Provider.CreateParamName(FieldName);

                    StringBuilder SB = new StringBuilder();
                    SB.AppendLine(DetailDescriptor.DeleteSql);
                    SB.AppendLine("where ");
                    SB.AppendLine($"  {FieldName} = {ParamName} ");

                    fDeleteDetailsSql = SB.ToString();
                }

                return fDeleteDetailsSql;
            }
            set
            {
                fDeleteDetailsSql = value;
            }


        }
    }


}

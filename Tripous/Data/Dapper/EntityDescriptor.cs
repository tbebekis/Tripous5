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
    /// Describes a database table field
    /// </summary>
    public class EntityDescriptor 
    {
        readonly Type StringType = typeof(string);

        /* private */
        //TableAttribute Source;
        Dictionary<string, PropertyInfo> PropDic = new Dictionary<string, PropertyInfo>(); // used with Dapper PropertySelectorFunc
        List<PropDescriptor> fPrimaryKeyList;
        SqlProvider fProvider;
        EntityInfo fInfo;

        EntityDescriptor fMasterEntityDescriptor;


        string fDeleteSql;
        string fSelectSql;

        string fSelectByMasterIdSql;

        string fSelectRowSql;
        string fInsertRowSql;
        string fUpdateRowSql;
        string fDeleteRowSql;

        string fPrimaryKeysWhere;
 
        void TrimLastCommaAndSpace(StringBuilder SB)
        {
            if (SB.Length > 0)
            {
                string S = SB.ToString().TrimEnd();
                S = S.TrimEnd(',');
                SB.Clear();
                SB.Append(S);
            }
        }

        /// <summary>
        /// Dapper PropertySelector function, for use with Dapper.SqlMapper.SetTypeMap() method
        /// </summary>
        PropertyInfo PropertySelectorFunc(Type EntityType, string FieldName)
        {
            FieldName = FieldName.ToUpper();
            return PropDic.ContainsKey(FieldName)? PropDic[FieldName]: null;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityDescriptor(Type EntityType, EntityAttribute Source)
        {
            this.EntityType = EntityType;
            //this.Source = Source;             
            this.TableName = Source.TableName;
            this.PrimaryKeys = Source.PrimaryKeys;
            this.Autoincrement = Source.Autoincrement;
            this.Mode = Source.Mode;
            this.ConnectionName = Source.ConnectionName;
            this.PacketType = Source.PacketType;

            // collect property information
            Dictionary<PropAttribute, PropertyInfo> FieldsDic = new Dictionary<PropAttribute, PropertyInfo>();
            Dictionary<DetailListAttribute, PropertyInfo> ReleationalListsDic = new Dictionary<DetailListAttribute, PropertyInfo>();
            Dictionary<RelationalAttribute, PropertyInfo> ReleationalsDic = new Dictionary<RelationalAttribute, PropertyInfo>();

            PropertyInfo[] Properties = EntityType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                object[] Attributes = Property.GetCustomAttributes(false);
                foreach (object Attr in Attributes)
                {
                    if (Attr is PropAttribute)
                    {
                        FieldsDic[Attr as PropAttribute] = Property;
                    }
                    else if (Attr is DetailListAttribute)
                    {
                        ReleationalListsDic[Attr as DetailListAttribute] = Property;
                    }
                    else if (Attr is RelationalAttribute)
                    {
                        ReleationalsDic[Attr as RelationalAttribute] = Property;
                    } 
                }
            }


            // fields in the database table
            foreach (var Entry in FieldsDic)
            {
                PropAttribute Attr = Entry.Key;
                PropertyInfo Property = Entry.Value;

                PropDic[Attr.Name.ToUpper()] = Property;

                PropDescriptor Field = new PropDescriptor(this, Property, Attr);
                Fields.Add(Field);

                if (Field.MasterEntityType != null)
                {
                    this.DetailKeyField = Field;
                }
            }


            // relational lists - aka details
            foreach (var Entry in ReleationalListsDic)
            {
                DetailListAttribute Attr = Entry.Key;
                PropertyInfo Property = Entry.Value;

                DetailListDescriptor RelationalList = new DetailListDescriptor(Property, Attr.DetailEntityType, Attr.DetailKeyPropertyName);
                DetailLists.Add(RelationalList);
            }


            // relational references
            foreach (var Entry in ReleationalsDic)
            {
                RelationalAttribute Attr = Entry.Key;
                PropertyInfo Property = Entry.Value;

                PropDescriptor KeyField = Find(Attr.KeyPropertyName);
                if (KeyField != null)
                {
                    RelationalDescriptor ReleationalDescriptor = new RelationalDescriptor(Property, KeyField);
                    Relationals.Add(ReleationalDescriptor);
                }
 
            }

 
            // register to Dapper
            CustomPropertyTypeMap DapperMap = new CustomPropertyTypeMap(EntityType, PropertySelectorFunc);
            SqlMapper.SetTypeMap(EntityType, DapperMap);
        }

 
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityDescriptor(Type EntityType, string TableName, string PrimaryKeys, bool Autoincrement, CRUDMode Mode, string ConnectionName, Type PacketType)
        {
            this.EntityType = EntityType;
            this.TableName = TableName;
            this.PrimaryKeys = PrimaryKeys;
            this.Autoincrement = Autoincrement;
            this.Mode = Mode;
            this.ConnectionName = ConnectionName;
            this.PacketType = PacketType;

            // register to Dapper
            CustomPropertyTypeMap DapperMap = new CustomPropertyTypeMap(EntityType, PropertySelectorFunc);
            SqlMapper.SetTypeMap(EntityType, DapperMap);
        }

        /* public */
        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{EntityName}:{TableName}";
        }

        /// <summary>
        /// A validity check to perform just before the entity is saved in the database.
        /// <para>Adds errors to a specified StringBuilder. </para>
        /// </summary>
        public void BeforeSaveCheck(object Entity, StringBuilder Errors, bool IsInsert)
        {
            if (Entity != null && Entity.GetType() == this.EntityType)
            {
                
                object Value;
                string Error;

                foreach (var Field in Fields)
                {
                    Value = Field.GetValue(Entity);

                    if (!Field.Nullable)
                    {
                        if (Value == null)
                        {
                            Error = $"Property/Field can not be null. Entity: {EntityName}, Property: {Field.PropertyName}, Field: {Field.FieldName}";
                            Errors.AppendLine(Error);                            
                        }
                    }

                    if (Field.PropertyType == StringType && Value != null)
                    {
                        if (Field.Length > 0 && (Value as string).Length > Field.Length)
                        {
                            Error = $"Property/Field value exceeds MaxLength. Entity: {EntityName}, Property: {Field.PropertyName}, Field: {Field.FieldName}, MaxLength: {Field.Length}";
                            Errors.AppendLine(Error);
                        }
                    }
                }
            }
 
        }


        /// <summary>
        /// Adds a field descriptor.
        /// <para>CAUTION: to be used only when the table descriptor is constructed "manually", that is without calling the constructor with the TableAttribute. </para>
        /// </summary>
        public PropDescriptor AddField(string PropertyName, string FieldName, bool Nullable, string TypeName, int Length = 0, string Description = "")
        {
            PropDescriptor Result = new PropDescriptor(this, PropertyName, FieldName, Nullable, TypeName, Length, Description);
            this.Fields.Add(Result);
            return Result;
        }

        /// <summary>
        /// Finds and returns a field by property or field name, if any, else null.
        /// <para>NOTE: The specified field name could be the name of the field in the database table or the property name in the entity.</para>
        /// </summary>
        public PropDescriptor Find(string PropertyOrFieldName)
        {
            PropDescriptor Result = Fields.FirstOrDefault((item) => item.PropertyName.IsSameText(PropertyOrFieldName));
            if (Result == null)
                Result = Fields.FirstOrDefault((item) => item.FieldName.IsSameText(PropertyOrFieldName));
            return Result;
        }
        /// <summary>
        /// Finds and returns a field by property or field name, if any, else throws an exception.
        /// <para>NOTE: The specified field name could be the name of the field in the database table or the property name in the entity.</para>
        /// </summary>
        public PropDescriptor Get(string PropertyOrFieldName)
        {
            PropDescriptor Result = Find(PropertyOrFieldName);
            if (Result == null)
                Sys.Error($"Property or Field not found. Entity: {EntityName}, PropertyOrField: {PropertyOrFieldName}");
            return Result;
        }
        /// <summary>
        /// Returns the index of a field, by field name, if any, else -1.
        /// <para>NOTE: The specified field name could be the name of the field in the database table or the property name in the entity.</para>
        /// </summary>
        public int IndexOf(string FieldName)
        {
            PropDescriptor Field = Find(FieldName);
            return Fields.IndexOf(Field);
        }
        /// <summary>
        /// Returns true if a field exists by field name, else false.
        /// <para>NOTE: The specified field name could be the name of the field in the database table or the property name in the entity.</para>
        /// </summary>
        public bool Contains(string FieldName)
        {
            return Find(FieldName) != null;
        }

        /// <summary>
        /// Finds and returnd a PropertyInfo of the Entity, based on a specified property name
        /// </summary>
        public PropertyInfo FindProperty(string PropertyName)
        {
            foreach (var Entry in PropDic)
            {
                if (Sys.IsSameText(PropertyName, Entry.Key))
                    return Entry.Value;
            }

            return null;
        }

        /// <summary>
        /// Returns true if the specified field name is one of the primary key fields.
        /// <para>NOTE: The specified field name could be the name of the field in the database table or the property name in the entity.</para>
        /// </summary>
        public bool IsPrimaryKey(string FieldName)
        {
            return IsPrimaryKey(Find(FieldName));
        }
        /// <summary>
        /// Returns true if the specified field is one of the primary key fields.
        /// </summary>
        public bool IsPrimaryKey(PropDescriptor Field)
        {
            return Field == null ? false : this.PrimaryKeyList.Contains(Field);
        }

        /// <summary>
        /// Creates and returns a Dapper DynamicParameters collection, based on a specified Entity.
        /// </summary>
        [CLSCompliant(false)]
        public DynamicParameters CreateParams(DataEntity Entity)
        {
            if (Entity.GetType() != this.EntityType)
                Sys.Error("A TableDescriptor of {0} can not create parameters for {1}", this.EntityType.Name, Entity.GetType().Name);

            var Params = new DynamicParameters();

            PropertyInfo Prop;
            foreach (var Field in Fields)
            {
                Prop = Field.Property;
                Params.Add(Field.FieldName, Prop.GetValue(Entity));
            }

            return Params;
        }

        /// <summary>
        /// Gets the value of the primary key property from a specified entity.
        /// <para>WARNING: If the entity has a compound primary key then an exception is thrown.</para>
        /// </summary>
        public object GetPrimaryKeyValue(object Entity)
        {
            if (PrimaryKeyList.Count > 1)
                Sys.Error($"{EntityType.FullName} Entity has a compound primary key");

            PropertyInfo Property = FindProperty(PrimaryKeyList[0].PropertyName);

            return Property.GetValue(Entity);
        }

        /// <summary>
        /// The user of an <see cref="EntityFilter" /> may use property names in filter conditions.
        /// This method replaces those property names with database field names.
        /// </summary>
        public void ProcessEntityFilter(EntityFilter Filter)
        {

            Action<List<EntityFilter>> ProcessFilterItems = null;

            ProcessFilterItems = (Items) =>
            {
                PropDescriptor PropDes;
                foreach (var Item in Items)
                {
                    if (Item.IsGroup)
                    {
                        ProcessFilterItems(Item.Items);
                    }
                    else
                    {
                        PropDes = this.Find(Item.FieldName);
                        if (PropDes != null)
                            Item.FieldName = PropDes.FieldName;
                    }
                }
            };

            ProcessFilterItems(Filter.Items);
        }

        /* properties */
        /// <summary>
        /// The name of the connection in config file that represents the database this table belongs to.
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// Gets the data provider associated to this database table.
        /// </summary>
        public SqlProvider Provider
        {
            get
            {
                // get provider on-demand
                if (fProvider == null)
                {
                    if (!string.IsNullOrWhiteSpace(this.ConnectionName))
                    {
                        SqlConnectionInfo ConnectionInfo = Db.GetConnectionInfo(ConnectionName);
                        fProvider = ConnectionInfo.GetSqlProvider();  
                    }
                    else
                    {
                        return null;
                    }
                }
                return fProvider;
            }
        }
        /// <summary>
        /// Returns the parameter prerix, e.g @
        /// </summary>
        public string ParamPrefix { get { return Provider.NativePrefix.ToString(); } }
        /// <summary>
        /// The type of the entity that represents the database table.
        /// </summary>
        public Type EntityType { get; private set; }
        /// <summary>
        /// The entity name
        /// </summary>
        public string EntityName { get { return EntityType.Name; } }
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// A semicolon delimited list of field names. 
        /// <para>CAUTION: When multiple, then the order is sigificant and must be kept everywhere (Service, Controller, etc).</para>
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
        /// The Packet (model) type to configure a mapping.
        /// </summary>
        public Type PacketType { get; set; }
        
        /// <summary>
        /// The type of the master entity/table of this entity/table, if any, else null.
        /// </summary>
        public Type MasterEntityType { get { return DetailKeyField != null ? DetailKeyField.MasterEntityType : null; } }
        /// <summary>
        /// The descriptor of the master (foreign) entity/table of this entity/table, if any, else null.
        /// </summary>
        public EntityDescriptor MasterEntityDescriptor
        {
            get
            {
                if (fMasterEntityDescriptor == null)
                {
                    fMasterEntityDescriptor = EntityDescriptors.Find(MasterEntityType);
                }

                return fMasterEntityDescriptor;
            }
        }
        /// <summary>
        /// A field descriptor of this entity/table that is a foreign key to a master (foreign) entity/table, if any, else null.
        /// </summary>
        public PropDescriptor DetailKeyField { get; set; }
        /// <summary>
        /// A name of a property of this entity/table that is a foreign key to a master entity/table, if any, else null.
        /// </summary>
        public string DetailKeyPropertyName { get { return DetailKeyField != null ? DetailKeyField.PropertyName : string.Empty; } }
        
        /// <summary>
        /// The field list of the table.
        /// </summary>
        public List<PropDescriptor> Fields { get; } = new List<PropDescriptor>();
        /// <summary>
        /// A field with the primary key fields
        /// </summary>
        public List<PropDescriptor> PrimaryKeyList
        {
            get
            {
                // construct the primary key field list on-demand
                if (fPrimaryKeyList == null)
                {
                    // collect the primary key fields
                    if (!string.IsNullOrWhiteSpace(this.PrimaryKeys))
                    {
                        fPrimaryKeyList = new List<PropDescriptor>();

                        PropDescriptor Field;
                        string[] Parts = this.PrimaryKeys.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string FieldName in Parts)
                        {
                            Field = Find(FieldName.Trim());
                            if (Field != null)
                            {
                                fPrimaryKeyList.Add(Field);
                            }
                        }
                    }
                    else
                    {
                        return new List<PropDescriptor>();
                    }
                }
                return fPrimaryKeyList;
            }
        }

        /// <summary>
        /// Table information
        /// </summary>
        public EntityInfo Info
        {
            get
            {
                if (fInfo == null)
                {
                    fInfo = new EntityInfo(this);
                }
                return fInfo;
            }

        }

        /// <summary>
        /// Sql statement text for SELECT without WHERE clause
        /// </summary>
        public string SelectSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fSelectSql))
                {
                    fSelectSql = string.Format("select * from {0} ", this.TableName);
                }

                return fSelectSql;
            }
            set
            {
                fSelectSql = value;
            }
        }
        /// <summary>
        /// Sql statement text for SELECT without WHERE clause
        /// </summary>
        public string DeleteSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fDeleteSql))
                {
                    fDeleteSql = string.Format("delete from {0} ", this.TableName);
                }

                return fDeleteSql;
            }
            set
            {
                fDeleteSql = value;
            }
        }

        /// <summary>
        /// Returns a SELECT Sql statement for executing a GetByMasterId() method.
        /// </summary>
        public string SelectByMasterIdSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fSelectByMasterIdSql) && DetailKeyField != null)
                {
                    string FieldName = DetailKeyField.FieldName;
                    string ParamName = Provider.CreateParamName(DetailKeyField.FieldName);

                    StringBuilder SB = new StringBuilder();
                    SB.AppendLine("where ");
                    SB.AppendLine($"  {FieldName} = {ParamName} ");

                    fSelectByMasterIdSql = SelectSql + SB.ToString();
                }
                return fSelectByMasterIdSql;
            }
            set
            {
                fSelectByMasterIdSql = value;
            }
        }
 
        /// <summary>
        /// Sql statement text for conducting SELECT of a row with WHERE cluse
        /// </summary>
        public string SelectRowSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fSelectRowSql))
                {
                    fSelectRowSql = SelectSql + Environment.NewLine + PrimaryKeysWhere;
                }

                return fSelectRowSql;
            }
            set
            {
                fSelectRowSql = value;
            }
        }
        /// <summary>
        /// Sql statement text for conducting INSERT of a row 
        /// </summary>
        public string InsertRowSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fInsertRowSql))
                {
                    /* field lists preparation */
                    StringBuilder InsertFields = new StringBuilder();                // insert field list
                    StringBuilder InsertParams = new StringBuilder();               // insert params field list
 
                    string FieldName;
                    string ParamName;

                    // prepare field and param lists
                    InsertFields.AppendLine();
                    InsertParams.AppendLine(); 

                    foreach (var Field in Fields)
                    {
                        FieldName = Field.FieldName;
                        ParamName = Provider.CreateParamName(FieldName);

                        if (!Field.IsPrimaryKey())
                        {
                            InsertFields.AppendLine($"  {FieldName}, ");
                            InsertParams.AppendLine($"  {ParamName}, "); 
                        }
                        else if (!this.Autoincrement)
                        {
                            InsertFields.AppendLine($"  {FieldName}, ");
                            InsertParams.AppendLine($"  {ParamName}, ");
                        }
                    }

                    // remove last comma and space
                    TrimLastCommaAndSpace(InsertFields);
                    TrimLastCommaAndSpace(InsertParams);

                    /* Insert */
                    string SqlText = "insert into {0} ( {1}" + Environment.NewLine + " ) values ( {2}" + Environment.NewLine + " ) ";
                    fInsertRowSql = string.Format(SqlText, this.TableName, InsertFields.ToString(), InsertParams.ToString());
                }

                return fInsertRowSql;
            }
            set
            {
                fInsertRowSql = value;
            }
        }
        /// <summary>
        /// Sql statement text conducting UPDATE of a row with WHERE clause
        /// </summary>
        public string UpdateRowSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fUpdateRowSql))
                {
 
                    StringBuilder SB = new StringBuilder();                 // update field list AND update params field list

                    string FieldName;
                    string ParamName;

                    // prepare field and param lists 
                    SB.AppendLine();

                    foreach (var Field in Fields)
                    {
                        FieldName = Field.FieldName;
                        ParamName = Provider.CreateParamName(FieldName);

                        if (!Field.IsPrimaryKey())
                        {
                            SB.AppendLine($"  {FieldName} = {ParamName}, ");
                        }
                    }

                    // remove last comma and space
                    TrimLastCommaAndSpace(SB);


                    /* Update */
                    string SqlText = "update {0} " + Environment.NewLine + "set {1} " + Environment.NewLine + PrimaryKeysWhere;
                    fUpdateRowSql = string.Format(SqlText, this.TableName, SB.ToString());

                }

                return fUpdateRowSql;
            }
            set
            {
                fUpdateRowSql = value;
            }
        }
        /// <summary>
        /// Sql statement text conducting DELETE of a row with WHERE clause
        /// </summary>
        public string DeleteRowSql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fDeleteRowSql))
                {
                    fDeleteRowSql = DeleteSql + Environment.NewLine + PrimaryKeysWhere;
                }

                return fDeleteRowSql;
            }
            set
            {
                fDeleteRowSql = value;
            }
        }
        
        /// <summary>
        /// WHERE Sql containing the primary key fields
        /// </summary>
        public string PrimaryKeysWhere
        {
            get
            {
                // prepare primary keys WHERE on-demand
                if (string.IsNullOrWhiteSpace(fPrimaryKeysWhere))
                {
                    StringBuilder SB = new StringBuilder();

                    string FieldName;
                    string ParamName;

                    // prepare the WHERE clause with primary key(s)
                    SB.AppendLine("where ");
                    for (int i = 0; i < PrimaryKeyList.Count; i++)
                    {
                        PropDescriptor Field = PrimaryKeyList[i];
                        FieldName = Field.FieldName;
                        ParamName = Provider.CreateParamName(FieldName);

                        if (i == 0)
                        {
                            SB.AppendLine($"  {FieldName} = {ParamName} ");
                        }
                        else
                        {
                            SB.AppendLine($"  and {FieldName} = {ParamName} ");
                        }
                    }

                    fPrimaryKeysWhere = SB.ToString();
                }

                return fPrimaryKeysWhere;
            }
            set
            {
                fPrimaryKeysWhere = value;
            }
        }

        /// <summary>
        /// A list of descriptors describing relations to detail tables/entities
        /// </summary>
        public List<DetailListDescriptor> DetailLists { get; } = new List<DetailListDescriptor>();
        /// <summary>
        ///  A list of descriptors describing an one-to-one relation. Describes a non-list property which is a reference to another Entity.
        /// </summary>
        public List<RelationalDescriptor> Relationals { get; } = new List<RelationalDescriptor>();
    }


}

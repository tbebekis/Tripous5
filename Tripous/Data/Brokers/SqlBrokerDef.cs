using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using Tripous.Data;

namespace Tripous.Data
{
    /// <summary>
    /// Describes a broker
    /// </summary>
    public class SqlBrokerDef
    {
        static List<SqlBrokerDef> RegistryList = new List<SqlBrokerDef>();

        string fTitle;
        string fMainTableName;
        string fEntityName;


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrokerDef()
        {
        }

        /* static */
        /// <summary>
        /// Returns a registered item, if any, else null.
        /// </summary>
        static public SqlBrokerDef Find(string Name)
        {
            return RegistryList.Find(item => Sys.IsSameText(item.Name, Name));
        }
        /// <summary>
        /// Returns true if an item is registered.
        /// </summary>
        static public bool Contains(string Name)
        {
            return Find(Name) != null;
        }

        /// <summary>
        /// Returns the index of an item in the internal registry list.
        /// </summary>
        static public int IndexOf(SqlBrokerDef Def)
        {
            return IndexOf(Def.Name);
        }
        /// <summary>
        /// Returns the index of an item in the internal registry list.
        /// </summary>
        static public int IndexOf(string Name)
        {
            for (int i = 0; i < RegistryList.Count; i++)
            {
                if (Sys.IsSameText(RegistryList[i].Name, Name))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Registers an item.
        /// <para>NOTE: If an item with the same name is already registered, the specified item replaces the existing item.</para>
        /// </summary>
        static public SqlBrokerDef Register(SqlBrokerDef Def)
        {
            int Index = IndexOf(Def);
            if (Index != -1)
            {
                RegistryList[Index] = Def;
                return RegistryList[Index];
            }
            else
            {
                RegistryList.Add(Def);
                return Def;
            }
        }
        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        static public SqlBrokerDef Register(string ConnectionName, string Name, string MainTableName, string TitleKey, string TypeClassName)
        {
            SqlBrokerDef Def = new SqlBrokerDef();

            Def.Name = Name;
            Def.ConnectionName = ConnectionName;
            Def.MainTableName = MainTableName;
            Def.TitleKey = TitleKey;
            Def.TypeClassName = TypeClassName;

            return Register(Def);
        }
        /// <summary>
        /// Adds a broker to the list.
        /// <para><see cref="ConnectionName"/> becomes the <see cref="SysConfig.DefaultConnection"/> value. </para>
        /// <para><see cref="TitleKey"/> becomes the specified name.</para>
        /// </summary>
        static public SqlBrokerDef Register(string Name, string MainTableName, string TypeClassName)
        {
            return Register(SysConfig.DefaultConnection, Name, MainTableName, Name, TypeClassName);
        }
        /// <summary>
        /// Adds a broker to the list.
        /// <para><see cref="ConnectionName"/> becomes the <see cref="SysConfig.DefaultConnection"/> value. </para>
        /// <para>The <see cref="Name"/>, <see cref="MainTableName"/> and <see cref="TitleKey"/> are assigned by a single specified value.</para>
        /// </summary>
        static public SqlBrokerDef Register(string Name, string TypeClassName)
        {
            return Register(SysConfig.DefaultConnection, Name, Name, Name, TypeClassName);
        }
        /// <summary>
        /// Adds a broker to the list.
        /// <para><see cref="ConnectionName"/> becomes the <see cref="SysConfig.DefaultConnection"/> value. </para>
        /// <para>The <see cref="Name"/>, <see cref="MainTableName"/> and <see cref="TitleKey"/> are assigned by a single specified value.</para>
        /// <para><see cref="TypeClassName"/> becomes the <see cref="SqlBroker"/> class.</para>
        /// </summary>
        static public SqlBrokerDef Register(string Name)
        {
            return Register(SysConfig.DefaultConnection, Name, Name, Name, typeof(SqlBroker).FullName);
        }

        /// <summary>
        /// Unregisters a specified item.
        /// </summary>
        static public void UnRegister(SqlBrokerDef Def)
        {
            RegistryList.Remove(Def);
        }

        /// <summary>
        /// Returns a list of code fields. A code field is associated to a code provider. The code providers procudes the value of the field on INSERTs.
        /// </summary>
        static public SqlBrokerFieldDef[] GetCodeFields(SqlBrokerDef Def)
        {
            List<SqlBrokerFieldDef> Result = new List<SqlBrokerFieldDef>();

            foreach (var Field in Def.MainTable.Fields)
            {
                if (!string.IsNullOrWhiteSpace(Field.CodeProviderName))
                {
                    if (!CodeProviderDef.Contains(Field.CodeProviderName))
                        Sys.Throw($"No code provider found for a field. Broker: {Def.Name}, CodeProvider: {Field.CodeProviderName}, Field: {Field.Name}");

                    Result.Add(Field);
                } 
            }

            return Result.ToArray();
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
            SqlBrokerDef Empty = new SqlBrokerDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(SqlBrokerDef Source)
        {
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public SqlBrokerDef Clone()
        {
            SqlBrokerDef Result = new SqlBrokerDef();
            Sys.AssignObject(this, Result);
            return Result;
        }



        /// <summary>
        /// Creates and adds a table to tables.
        /// </summary>
        public SqlBrokerTableDef AddTable(string TableName, string TitleKey = "")
        {
            SqlBrokerTableDef Result = FindTable(TableName);
            if (Result == null)
            {
                Result = new SqlBrokerTableDef() { Name = TableName, TitleKey = TitleKey };
                Tables.Add(Result);
            }

            return Result;
        }
        /// <summary>
        /// Finds a table descriptor by Name.
        /// <para>A null or empty Name returns the MainTable. MainTable is also returned when Name is Item.</para>
        /// <para>If Name is Lines the LinesTableName descriptor is returned.</para>
        /// <para>If Name is SubLines the SubLinesTableName descriptor is returned.</para>
        /// </summary>
        public SqlBrokerTableDef FindTable(string Name)
        {

            if (string.IsNullOrWhiteSpace(Name) || Sys.IsSameText(Name, "Item") || Sys.IsSameText(Name, SqlBrokerTableDef.ITEM))
                Name = MainTableName;
            else if (Sys.IsSameText(Name, "Lines") || Sys.IsSameText(Name, SqlBrokerTableDef.LINES))
                Name = LinesTableName;
            else if (Sys.IsSameText(Name, "SubLines") || Sys.IsSameText(Name, SqlBrokerTableDef.SUBLINES))
                Name = SubLinesTableName;

            return Tables.Find(item => item.Name.IsSameText(Name));

        }

        /// <summary>
        /// Returns an array of SelectSql items, 
        /// those added by the application and
        /// those added by the user at runtime
        /// </summary>
        public SelectSql[] GetMergedSelectSqlList()
        {
            SelectSql[] statements = SelectList.ToArray();
            List<SelectSql> List = new List<SelectSql>(statements);

            /*
                        // get any statements designed by the user
                        statements = GetDesignedSelectSqlList();            
                        if (statements.Length > 0)
                            List.AddRange(statements); 
             */

            return List.ToArray();
        }




        /* properties */
        /// <summary>
        /// The Name must be unique.
        /// </summary> 
        public string Name { get; set; }

 
        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(fTitle) ? fTitle : (!string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name); }
            set { fTitle = value; }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// Gets or sets the connection name (database)
        /// </summary>
        public string ConnectionName { get; set; } = SysConfig.DefaultConnection;
 

        /// <summary>
        /// Gets or set the name of the main table
        /// </summary>
        public string MainTableName
        {
            get { return !string.IsNullOrWhiteSpace(fMainTableName) ? fMainTableName : Name; }
            set { fMainTableName = value; }
        }
        /// <summary>
        /// Gets or sets the name of the detail table, if any
        /// </summary>
        public string LinesTableName { get; set; }
        /// <summary>
        /// Gets or sets the name of the sub-detail table, if any
        /// </summary>
        public string SubLinesTableName { get; set; }


        /// <summary>
        /// The name of the Entity this broker represents
        /// </summary>
        public string EntityName
        {
            get { return !string.IsNullOrWhiteSpace(fEntityName) ? fEntityName : this.Name; }
            set { this.fEntityName = value; }
        }


        /// <summary>
        /// Returns the connection info (database)
        /// </summary>
        [JsonIgnore]
        public SqlConnectionInfo ConnectionInfo { get { return Db.GetConnectionInfo(ConnectionName); } }
        /// <summary>
        /// if the MainTableName exists in Tables then this property returns a reference to it
        /// </summary>
        [JsonIgnore]
        public SqlBrokerTableDef MainTable { get { return Tables.Find(item => item.Name.IsSameText(MainTableName)); } }
        /// <summary>
        /// Returns the primary key field name of the <see cref="MainTable"/>
        /// <para>WARNING: It provides a setter for the json serializer only, it is read-only actually </para>
        /// </summary>
        [JsonIgnore]
        public string PrimaryKeyField => MainTable == null ? "Id" : MainTable.PrimaryKeyField;
        /// <summary>
        /// The main select statement
        /// </summary>
        [JsonIgnore]
        public SelectSql MainSelect
        {
            get
            {
                SelectSql Result = SelectList.Find(item => item.Name.IsSameText(Sys.MainSelect));
                if (Result == null)
                {
                    Result = new SelectSql();
                    Result.Name = Sys.MainSelect;
                    if (!string.IsNullOrWhiteSpace(MainTableName))
                        Result.Text = $"select * from {MainTableName}";  
                    SelectList.Insert(0, Result);
                }

                return Result;
            }
        }

        /// <summary>
        /// The list of select statements
        /// </summary>
        public List<SelectSql> SelectList { get; set; } = new List<SelectSql>();
        /// <summary>
        /// Gets the table descriptors
        /// </summary>
        public List<SqlBrokerTableDef> Tables { get; set; } = new List<SqlBrokerTableDef>();
        /// <summary>
        /// A list of SELECT Sql statements that executed once at the initialization of the broker and may be used
        /// in various situations, i.e. Locators
        /// </summary>
        public List<SqlBrokerQueryDef> Queries { get; set; } = new List<SqlBrokerQueryDef>();
        /// <summary>
        /// When is true indicates that the OID is a Guid string.  
        /// </summary>
        public bool GuidOids { get; set; } = SysConfig.GuidOids;
        /// <summary>
        /// Indicates that deletes should happen top to bottom, so if any database foreign constraint exists, then let
        /// an exception to be thrown.
        /// </summary>
        public bool NoCascadeDeletes { get; set; }

        /// <summary>
        /// Gets or sets the class name of the <see cref="System.Type"/> this descriptor describes.
        /// <para>NOTE: The valus of this property may be a string returned by the <see cref="Type.AssemblyQualifiedName"/> property of the type. </para>
        /// <para>In that case, it consists of the type name, including its namespace, followed by a comma, followed by the display name of the assembly
        /// the type belongs to. It might looks like the following</para>
        /// <para><c>Tripous.Forms.BaseDataEntryForm, Tripous, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</c></para>
        /// <para></para>
        /// <para>Otherwise it must be a type name registered to the <see cref="TypeStore"/> either directly or
        /// just by using the <see cref="TypeStoreItemAttribute"/> attribute.</para>
        /// <para>In the case of a type registered with the TypeStore, a safe way is to use a Namespace.TypeName combination
        /// both, when registering and when retreiving a type.</para>
        /// <para></para>
        /// <para>Regarding types belonging to the various Tripous namespaces, using just the TypeName is enough.
        /// Most of the Tripous types are already registered to the TypeStore with just their TypeName.</para>
        /// </summary>
        public string TypeClassName { get; set; } = typeof(SqlBroker).Name;

    }
}

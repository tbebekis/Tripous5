/*--------------------------------------------------------------------------------------        
                            Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using Tripous.Data;


namespace Tripous.Model
{
    /// <summary>
    /// Describes a broker. A broker is Tripous business object. A broker is actually
    /// a set of correlated tables and code.
    /// </summary>
    public class BrokerDescriptor : Descriptor
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string SSysDataType = "Broker"; 
 
        string fEntityName;

 
        /// <summary>
        /// Field
        /// </summary>
        protected MemoryStream scriptStream = new MemoryStream();

        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();

            this.Name = string.Empty;
            this.TitleKey = string.Empty;

            this.DescriptorMode = DescriptorMode.System;
            this.ConnectionName = string.Empty;
            this.MainTableName = string.Empty;
            this.LinesTableName = string.Empty;
            this.SubLinesTableName = string.Empty;
            this.TypeClassName = string.Empty;
            this.CodeProducerName = string.Empty;
            this.GuidOids = true;
            this.PessimisticMode = false;
            this.NoCascadeDeletes = false;
 
            SelectList.Clear();
            Tables.Clear();
            Queries.Clear();
 
        }


        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public BrokerDescriptor()
        {
            Tables.Owner = this;            
            Queries.Owner = this;
            GuidOids = SysConfig.GuidOids;            
        }

        /* public */
        /// <summary>
        /// Override
        /// </summary>
        public override void CheckDescriptor()
        {
            base.CheckDescriptor();

            if (string.IsNullOrEmpty(ConnectionName))
                NotFullyDefinedError("ConnectionName");

            if (string.IsNullOrEmpty(MainTableName))
                NotFullyDefinedError("MainTableName");

            if (Tables.Count == 0)
                NotFullyDefinedError("Tables");

        }
        /// <summary>
        /// Finds a table descriptor by Name.
        /// <para>A null or empty Name returns the MainTable. MainTable is also returned when Name is Item.</para>
        /// <para>If Name is Lines the LinesTableName descriptor is returned.</para>
        /// <para>If Name is SubLines the SubLinesTableName descriptor is returned.</para>
        /// </summary>
        public TableDescriptor FindTableDescriptor(string Name)
        {

            if (string.IsNullOrEmpty(Name) || Sys.IsSameText(Name, "Item"))
                Name = MainTableName;
            else if (Sys.IsSameText(Name, "Lines"))
                Name = LinesTableName;
            else if (Sys.IsSameText(Name, "SubLines"))
                Name = SubLinesTableName;

            return Tables.Find(Name);

        }
        /// <summary>
        /// Ensures that a MainSelect statement exists.
        /// </summary>
        public void EnsureMainSelect()
        {
            SelectSql mainSelect = SelectList.Find(item => item.Name == Sys.MainSelect);
            if (mainSelect == null)
            {
                mainSelect = new SelectSql();
                mainSelect.Name = Sys.MainSelect;
                if (!string.IsNullOrWhiteSpace(MainTableName))
                    mainSelect.Text = string.Format("select * from {0}", MainTableName);
                SelectList.Insert(0, mainSelect);
            }
        }

        /// <summary>
        /// Returns the DataType for the SelectSql statements of this instance, stored in SYS_DATA table.
        /// </summary>
        public string GetSelectSqlDataType()
        {
            string DataType = SelectSqlSysDataItem.FormatDataType("Broker", this.Name);
            return DataType;
        }
        /// <summary>
        /// Returns an array of SelectSql  items, configured by the user at runtime
        /// </summary>
        public SelectSql[] GetDesignedSelectSqlList()
        {
            List<SelectSql> List = null;
            string DataType = GetSelectSqlDataType();
            List = SelectSqlSysDataItem.GetDesignedSelectSqlList(DataType);
            return List.ToArray();
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

            statements = GetDesignedSelectSqlList();            // get any statements designed by the user
            if (statements.Length > 0)
                List.AddRange(statements);

            return List.ToArray();
        }



        /* properties */
        /// <summary>
        /// Indicates the platform where a Ui element may displayed, such as 
        /// in desktop or web applications, or any kind of application.
        /// <para>Defaults to UiMode.All</para>
        /// </summary>
        public UiMode UiMode { get; set; } = Tripous.UiMode.Desktop | Tripous.UiMode.Web;
        /// <summary>
        /// Gets or sets the connection name (database)
        /// </summary>
        public string ConnectionName { get; set; } = SysConfig.DefaultConnection;
        /// <summary>
        /// Gets or sets the class name of the <see cref="System.Type"/> this descriptor describes.
        /// <para>NOTE: The valus of this property may be a string returned by the <see cref="Type.AssemblyQualifiedName"/> property of the type. </para>
        /// <para>In that case, it consists of the type name, including its namespace, followed by a comma, followed by the display name of the assembly
        /// the type belongs to. It might looks like the following</para>
        /// <para><c>Tripous.Forms.BaseDataEntryForm, Tripous, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</c></para>
        /// <para></para>
        /// <para>In the case of a type registered with the TypeStore, a safe way is to use a Namespace.TypeName combination
        /// both, when registering and when retreiving a type.</para>
        /// <para></para>
        /// <para>Regarding types belonging to the various Tripous namespaces, using just the TypeName is enough.
        /// Most of the Tripous types are already registered to the TypeStore with just their TypeName.</para>
        /// </summary>
        public string TypeClassName { get; set; } = "SqlBroker";

        /// <summary>
        /// Gets or set the name of the main table
        /// <para>CAUTION: Keep this property AFTER the Tables property. Otherwise serializers (when deserializing) will try to add twice the main table.</para>
        /// </summary>
        public string MainTableName { get; set; }
        /// <summary>
        /// Gets or sets the name of the detail table, if any
        /// </summary>
        public string LinesTableName { get; set; }
        /// <summary>
        /// Gets or sets the name of the sub-detail table, if any
        /// </summary>
        public string SubLinesTableName { get; set; }

 
        /// <summary>
        /// Gets or sets the Name of the code producer descriptor associated to this broker.
        /// </summary>
        public string CodeProducerName { get; set; }
        /// <summary>
        /// An integer Id from the SYS_ENTITY table 
        /// <para>It may points to an application Entity (for example Customer, Order, Employee, etc)</para>
        /// <para>Defaults to 0, meaning no entity Id.</para>
        /// <para>NOTE: EntityId is used by forms in order to call SysAction and Document services. No EntityId, no such services.</para>
        /// </summary>
        public int EntityId { get; set; }
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
        [Newtonsoft.Json.JsonIgnore]
        public SqlConnectionInfo ConnectionInfo { get { return Db.GetConnectionInfo(ConnectionName); } }
        /// <summary>
        /// if the MainTableName exists in Tables then this property returns a reference to it
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public TableDescriptor MainTable { get { return Tables.Find(MainTableName); } }
        /// <summary>
        /// Returns the primary key field name of the <see cref="MainTable"/>
        /// <para>WARNING: It provides a setter for the json serializer only, it is read-only actually </para>
        /// </summary>
        public string PrimaryKeyField
        {
            get { return MainTable == null ? "Id" : MainTable.PrimaryKeyField; }
            set { }
        }
        /// <summary>
        /// The list of select statements
        /// </summary>
        public List<SelectSql> SelectList { get; set; } = new List<SelectSql>();

        /// <summary>
        /// The main select statement
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public SelectSql MainSelect
        {
            get
            {
                EnsureMainSelect();
                return SelectList.Find(item => item.Name == Sys.MainSelect);
            }
        }
        /// <summary>
        /// Gets the table descriptors
        /// </summary>
        public TableDescriptors Tables { get; } = new TableDescriptors();
        /// <summary>
        /// Gets the query descriptors. Define SELECT Sql statements that
        /// executed once at the initialization of the broker and may be used
        /// in various situations, i.e. Locators
        /// </summary>
        public QueryDescriptors Queries { get; } = new QueryDescriptors();
        /// <summary>
        /// When is true indicates that the OID is a Guid string.  
        /// </summary>
        public bool GuidOids { get; set; }
        /// <summary>
        /// Indicates that pessimistic mode should be used. That is to flag a row in the database while it is selected
        /// and de-flag it when saved/deleted.
        /// </summary>
        public bool PessimisticMode { get; set; }
        /// <summary>
        /// Indicates that deletes should happen top to bottom, so if any database foreign constraint exists, then let
        /// an exception to be thrown.
        /// </summary>
        public bool NoCascadeDeletes { get; set; }






    }
}

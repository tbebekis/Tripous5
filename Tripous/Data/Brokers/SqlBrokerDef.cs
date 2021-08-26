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
        static List<SqlBrokerDef> Descriptors = new List<SqlBrokerDef>();

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
        /// Returns a descriptor by a specified name if any, else, null
        /// </summary>
        static public SqlBrokerDef FindDescriptor(string Name)
        {
            return Descriptors.Find(item => item.Name.IsSameText(Name));
        }
        /// <summary>
        /// Returns true if a descriptor is already registered under a specified name.
        /// </summary>
        static public bool DescriptorExists(string Name)
        {
            return FindDescriptor(Name) != null;
        }
        /// <summary>
        /// Registers a descriptor. If it finds a descriptor returns the already registered descriptor.
        /// </summary>
        static public SqlBrokerDef RegisterDescriptor(string Name, string Text)
        {
            SqlBrokerDef Result = FindDescriptor(Name);
            if (Result == null)
            {
                Result = new SqlBrokerDef() { Name = Name };
                Descriptors.Add(Result);
            }

            return Result;
        }


        /// <summary>
        /// Creates and returns an instance of a <see cref="SqlBroker"/> based on a specified descriptor.
        /// </summary>
        static public SqlBroker Create(string DescriptorName, bool Initialized, bool AsListBroker)
        {
            return Create(FindDescriptor(DescriptorName), Initialized, AsListBroker);
        }
        /// <summary>
        /// Creates and returns an instance of a <see cref="SqlBroker"/> based on a specified descriptor.
        /// </summary>
        static public SqlBroker Create(SqlBrokerDef Descriptor, bool Initialized, bool AsListBroker)
        {
            if (Descriptor == null)
                Sys.Throw($"Cannot create a {nameof(SqlBroker)}. Descriptor is null.");

            SqlBroker Result = TypeStore.Create(Descriptor.TypeClassName) as SqlBroker;
            Result.Descriptor = Descriptor;

            if ((Result.CodeProducer == null) && !string.IsNullOrWhiteSpace(Descriptor.CodeProducerName))
            { 
                CodeProvider CodeProvider = CodeProviderDef.Create(Descriptor.CodeProducerName, Descriptor.MainTableName); 
                Result.CodeProducer = CodeProvider;
            }

            if (Initialized)
                Result.Initialize(AsListBroker || Result.IsListBroker);

            return Result;
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

        /* properties */
        /// <summary>
        /// The Name must be unique.
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        [JsonIgnore]
        public string Title => !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name;
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
        public SelectSql MainSelect => SelectList.Find(item => item.Name.IsSameText(Sys.MainSelect));

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

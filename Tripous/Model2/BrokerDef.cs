using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using Tripous.Data;

namespace Tripous.Model2
{
    /// <summary>
    /// Describes a broker
    /// </summary>
    public class BrokerDef
    {
        string fEntityName;


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerDef()
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
            BrokerDef Empty = new BrokerDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(BrokerDef Source)
        {
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public BrokerDef Clone()
        {
            BrokerDef Result = new BrokerDef();
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
        public BrokerTableDef MainTable { get { return Tables.Find(item => item.Name.IsSameText(MainTableName)); } }
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
        public List<BrokerTableDef> Tables { get; set; } = new List<BrokerTableDef>();
        /// <summary>
        /// A list of SELECT Sql statements that executed once at the initialization of the broker and may be used
        /// in various situations, i.e. Locators
        /// </summary>
        public List<BrokerQueryDef> Queries { get; set; } = new List<BrokerQueryDef>();
        /// <summary>
        /// When is true indicates that the OID is a Guid string.  
        /// </summary>
        public bool GuidOids { get; set; } = SysConfig.GuidOids;
        /// <summary>
        /// Indicates that deletes should happen top to bottom, so if any database foreign constraint exists, then let
        /// an exception to be thrown.
        /// </summary>
        public bool NoCascadeDeletes { get; set; }

 

    }
}

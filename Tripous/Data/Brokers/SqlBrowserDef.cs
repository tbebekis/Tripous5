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
    /// Describes a <see cref="SqlBrowser"/>.
    /// </summary>
    public class SqlBrowserDef
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrowserDef()
        {
        }

        /* public */
        /// <summary>
        /// Copies <see cref="SelectSql"/> statements using as source the <see cref="SqlBroker"/> specified by a name.
        /// </summary>
        public void CopyBrokerStatements()
        {
            CopyBrokerStatements(this.SqlBrokerName);
        }
        /// <summary>
        /// Copies <see cref="SelectSql"/> statements using as source the <see cref="SqlBroker"/> specified by a name.
        /// </summary>
        public void CopyBrokerStatements(string SqlBrokerName)
        {
            if (string.IsNullOrEmpty(SqlBrokerName))
                return;

            SqlBrokerDef BrokerDes = SqlBrokerDef.FindDescriptor(SqlBrokerName); 
            if (BrokerDes == null)
                return;

            SelectList.Clear();
            SelectList.AddRange(BrokerDes.SelectList);
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
        /// Gets or sets the primary key field
        /// </summary>
        public string PrimaryKeyField { get; set; } = "Id";
        /// <summary>
        /// Gets or sets the broker descriptor name this browser cooperates with.
        /// <para>This may be an empty string though, meaning that this is an independent browser</para>
        /// </summary>
        public string SqlBrokerName { get; set; }

        /// <summary>
        /// The list of select statements
        /// </summary>
        public List<SelectSql> SelectList { get; set; } = new List<SelectSql>();

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
        public string TypeClassName { get; set; } = typeof(SqlBrowser).Name;


    }
}

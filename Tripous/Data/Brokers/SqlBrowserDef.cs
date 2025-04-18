﻿namespace Tripous.Data
{

    /// <summary>
    /// Describes a <see cref="SqlBrowser"/>.
    /// </summary>
    public class SqlBrowserDef
    {

        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrowserDef()
        {
        }


        /* public */
        /// <summary>
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                Sys.Throw(Res.GS("E_SqlBrowserDef_NameIsEmpty", "SqlBrowserDef Name is empty"));

            if (string.IsNullOrWhiteSpace(this.ConnectionName))
                Sys.Throw(Res.GS("E_SqlBrowserDef_ConnectionNameIsEmpty", "SqlBrowserDef ConnectionName is empty."));

            if (string.IsNullOrWhiteSpace(this.PrimaryKeyField))
                Sys.Throw(Res.GS("E_SqlBrowserDef_PrimaryKeyFieldIsEmpty", "SqlBrowserDef PrimaryKeyField is empty."));

            if (string.IsNullOrWhiteSpace(this.TypeClassName))
                Sys.Throw(Res.GS("E_SqlBrowserDef_TypeClassNameIsEmpty", "SqlBrowserDef TypeClassName is empty."));

            if (this.SelectList == null || this.SelectList.Count == 0)
                Sys.Throw(Res.GS("E_SqlBrowserDef_SelectListIsEmpty", "SqlBrowserDef SelectList is empty."));
        }

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
            if (string.IsNullOrWhiteSpace(SqlBrokerName))
                return;

            SqlBrokerDef BrokerDes = SqlBrokerDef.Find(SqlBrokerName); 
            if (BrokerDes == null)
                return;

            SelectList.Clear();
            SelectList.AddRange(BrokerDes.SelectSqlList);
        }

        /* properties */
        /// <summary>
        /// The Name must be unique.
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name; }
            set { }
        }

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

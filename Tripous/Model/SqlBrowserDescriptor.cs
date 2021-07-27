/*--------------------------------------------------------------------------------------        
                            Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

using Tripous;
using Tripous.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

 


namespace Tripous.Model
{
    /* see: http://en.wikipedia.org/wiki/Select_%28SQL%29  for limiting Select Sql returned rows  */



    /// <summary>
    /// Describes a browser class
    /// </summary>
    public class SqlBrowserDescriptor : Descriptor
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string SSysDataType = "SqlBrowser";
 
        SelectSqlList fSelectList = new SelectSqlList();

        /* overrides */
        /// <summary>
        /// Override
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();

            this.Name = string.Empty;
            this.TitleKey = string.Empty;

            this.ConnectionName = string.Empty;
            this.TypeClassName = string.Empty;
 
            this.BrokerName = string.Empty;
            this.PrimaryKeyField = string.Empty;
 
            this.UiMode = Tripous.UiMode.Desktop | Tripous.UiMode.Web;

            SelectList.Clear();
        }
 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrowserDescriptor()
        {
            fSelectList.Owner = this;
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

            if (string.IsNullOrEmpty(TypeClassName))
                NotFullyDefinedError("TypeClassName");

            if (SelectList.Count == 0)
                NotFullyDefinedError("SelectList");

        }



        /// <summary>
        /// Copies Statements using as source the Broker specified in BrokerName.
        /// <para>It assigns the criteria too.</para>
        /// </summary>
        public void CopyBrokerStatements()
        {
            CopyBrokerStatements(this.BrokerName);
        }
        /// <summary>
        /// Copies Statements using as source the Broker specified in BrokerName.
        /// <para>It assigns the criteria too.</para>
        /// </summary>
        public void CopyBrokerStatements(string BrokerName)
        {
            if (string.IsNullOrEmpty(BrokerName))
                return;

            BrokerDescriptor BrokerDes = Registry.Brokers.Find(BrokerName);
            if (BrokerDes == null)
                return;

            SelectList.Clear();
            SelectList.Assign(BrokerDes.SelectList);
        }

        /// <summary>
        /// Ensures that a MainSelect statement exists.
        /// </summary>
        public void EnsureMainSelect()
        {
            SelectSql mainSelect = SelectList.Find(Sys.MainSelect);
            if (mainSelect == null)
            {
                mainSelect = new SelectSql();
                mainSelect.Name = Sys.MainSelect;
                if (!string.IsNullOrWhiteSpace(MainTableName))
                    mainSelect.Text = string.Format("select * from {0}", this.MainTableName);
                SelectList.Insert(0, mainSelect);
            }
        }


 

        /* properties */
        /// <summary>
        /// Indicates the platform where a Ui element may displayed, such as 
        /// in desktop or web applications, or any kind of application.
        /// <para>Defaults to UiMode.All</para>
        /// </summary>
        public UiMode UiMode { get; set; } = Tripous.UiMode.Desktop | Tripous.UiMode.Web;
        /// <summary>
        /// Gets or sets the name of the database connection this descriptor works with.
        /// </summary>
        public string ConnectionName { get; set; } = SysConfig.DefaultConnection;
        /// <summary>
        /// Gets or set the name of the main table
        /// </summary>
        public string MainTableName { get; set; }
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
        public string TypeClassName { get; set; } = "SqlBrowser";

 
        /// <summary>
        /// Gets or sets the broker descriptor name this browser cooperates with.
        /// <para>This may be an empty string though, meaning that this is an independent browser</para>
        /// </summary>
        public string BrokerName { get; set; }

        /// <summary>
        /// Gets or sets the primary key field
        /// </summary>
        public string PrimaryKeyField { get; set; } = "Id";
 
 


 
        /// <summary>
        /// The list of select statements
        /// </summary>
        public SelectSqlList SelectList
        {
            get
            {
                EnsureMainSelect();
                return fSelectList;
            }
        }
        /// <summary>
        /// The main select statement
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public SelectSql MainSelect
        {
            get
            {
                EnsureMainSelect();
                return SelectList.Find(Sys.MainSelect);
            }
        }
 
 
    }
}

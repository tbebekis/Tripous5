using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Tripous.Data;

namespace Tripous.Data
{


    /// <summary>
    /// Describes a Locator.
    /// <para>A locator represents (returns) a single value, but it can handle and display multiple values
    /// in order to help the end user in identifying and locating that single value.</para>
    /// <para>For example, a TRADE data table has a CUSTOMER_ID column, representing that single value, but the user interface
    /// has to display information from the CUSTOMER table, specifically, the ID, CODE and NAME columns.</para>
    /// <para>The TRADE table is the target data table and the CUSTOMER_ID is the DataField field name.</para>
    /// </summary>
    public class LocatorDef
    {
        static List<LocatorDef> Descriptors = new List<LocatorDef>();
        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorDef()
        {
        }

        /* static */
        /// <summary>
        /// Returns a descriptor by a specified name if any, else, null
        /// </summary>
        static public LocatorDef FindDef(string Name)
        {
            return Descriptors.Find(item => item.Name.IsSameText(Name));
        }
        /// <summary>
        /// Returns true if a descriptor is already registered under a specified name.
        /// </summary>
        static public bool DefExists(string Name)
        {
            return FindDef(Name) != null;
        }
        /// <summary>
        /// Registers a descriptor. If it finds a descriptor returns the already registered descriptor.
        /// </summary>
        static public LocatorDef Register(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                Sys.Throw("Cannot register Locator. No name defined");

            LocatorDef Result = FindDef(Name);
            if (Result == null)
            {
                Result = new LocatorDef() { Name = Name };
                Descriptors.Add(Result);
            }

            return Result;
        }
        /// <summary>
        /// Registers a descriptor. If it finds a descriptor returns the already registered descriptor.
        /// </summary>
        static public LocatorDef Register(LocatorDef Def)
        {
            if (Def == null)
                Sys.Throw("Cannot register a null Locator");

            if (string.IsNullOrWhiteSpace(Def.Name))
                Sys.Throw("Cannot register Locator. No name defined");

            LocatorDef Result = FindDef(Def.Name);
            if (Result == null)
            {
                Descriptors.Add(Def);
                Result = Def;
            }

            return Result;
        }

        /* public */
        /// <summary>
        /// Override
        /// </summary>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name) ? Name : base.ToString();
        }
        /// <summary>
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            // TODO: CheckDescriptor()
        }


        /// <summary>
        /// Adds and returns a field descriptor. 
        /// </summary>
        public LocatorFieldDef Add(string Name, string DataField, string TitleKey, DataFieldType DataType = DataFieldType.String,
                                bool Visible = true, bool Searchable = true, bool ListVisible = true, string TableName = "")
        {
            LocatorFieldDef Result = new LocatorFieldDef(Name, DataField, TitleKey, DataType, Visible, Searchable, ListVisible, TableName);
            Fields.Add(Result);
            return Result;
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
        /// The SELECT statement to execute
        /// </summary>
        public string SqlText { get; set; }

        /// <summary>
        /// The name of the list table
        /// </summary>
        public string ListTableName { get; set; }
        /// <summary>
        /// The key field of the list table. The value of this field goes to the DataField
        /// </summary>
        public string ListKeyField { get; set; } = "Id";
        /// <summary>
        /// A command that ends up displaying the zoom UI (drill down)
        /// </summary>
        public string ZoomCommand { get; set; }
        
        /// <summary>
        /// Indicates whether the locator is readonly
        /// </summary>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// The list of descriptor fields.
        /// </summary>
        public List<LocatorFieldDef> Fields { get; set; } = new List<LocatorFieldDef>();
    }
}

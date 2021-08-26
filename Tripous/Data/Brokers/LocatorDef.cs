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
        static public LocatorDef FindDescriptor(string Name)
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
        static public LocatorDef RegisterDescriptor(string Name)
        {
            LocatorDef Result = FindDescriptor(Name);
            if (Result == null)
            {
                Result = new LocatorDef() { Name = Name };
                Descriptors.Add(Result);
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
        /// The SELECT statement to execute
        /// </summary>
        public string SelectSql { get; set; }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Tripous.Data;

namespace Tripous.Data
{
 
 
    /// <summary>
    /// A broker field definition
    /// </summary>
    public class SqlBrokerFieldDef
    {
        string fDefaultValue = Sys.NULL;
        string fTitle;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrokerFieldDef()
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
            SqlBrokerFieldDef Empty = new SqlBrokerFieldDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(SqlBrokerFieldDef Source)
        {
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public SqlBrokerFieldDef Clone()
        {
            SqlBrokerFieldDef Result = new SqlBrokerFieldDef();
            Sys.AssignObject(this, Result);
            return Result;
        }
 
        /// <summary>
        /// Returns the SELECT statement for the foreign table.
        /// </summary>
        public string GetForeignSelectSql()
        {
            if (string.IsNullOrWhiteSpace(ForeignTableName) 
                || string.IsNullOrWhiteSpace(ForeignKeyField)
                || (string.IsNullOrWhiteSpace(ForeignFieldList) && string.IsNullOrWhiteSpace(ForeignTableSql)))
                Sys.Throw($"Broker Field not fully defined: {Name}");

            if (!string.IsNullOrWhiteSpace(ForeignTableSql))
                return ForeignTableSql;

            List<string> List = new List<string>();
            List.Add(ForeignKeyField);

            string[] FieldNames = ForeignFieldList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            List.AddRange(FieldNames);
 
            string SqlText = $@" 
select 
    {List.CommaText()} 
from 
{ForeignTableName} 
";

            return SqlText;
        }

        /* properties */
        /// <summary>
        /// The field name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An alias of this field
        /// </summary>
        public string Alias { get; set; }

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
        /// The data-type of the field
        /// </summary>
        public DataFieldType DataType { get; set; }
        /// <summary>
        /// The max length of a string field
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// Gets or sets the decimals of the field. Used when is a float field.
        /// </summary>
        public int Decimals { get; set; }
        /// <summary>
        /// Gets or sets the flags of the field.
        /// </summary>
        public SqlBrokerFieldFlag Flags { get; set; }

        /// <summary>
        /// Gets or sets the default value of the field.
        /// </summary>
        public string DefaultValue
        {
            get { return string.IsNullOrWhiteSpace(fDefaultValue) ? Sys.NULL : fDefaultValue; }
            set { fDefaultValue = value; }
        }
        /// <summary>
        /// Gets or sets the expression used to calculate the values in a column, or create an aggregate column
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// The name of a foreign table this field points to, if any, else null.
        /// <example> Lets suppose that we have a CUSTOMER table with a CUSTOMER.COUNTRY_ID field
        /// and a COUNTRY table with ID and NAME fields. To establish a foreign relation
        /// <code>
        ///     this field          = "COUNTRY_ID";     // CUSTOMER.COUNTRY_ID
        ///     ForeignTableName    = "COUNTRY";                           
        ///     ForeignKeyField     = "ID";             // COUNTRY.ID         
        ///     ForeignFieldList    = "ID;NAME";        // COUNTRY.ID, COUNTRY.NAME 
        ///  </code>
        /// </example>
        /// </summary>
        public string ForeignTableName { get; set; }
        /// <summary>
        /// The alias of a foreign table this field points to, if any, else null.
        /// </summary>
        public string ForeignTableAlias { get; set; }
        /// <summary>
        /// The name of the field of the foreign table that becomes the result of a look-up operation
        /// </summary>
        public string ForeignKeyField { get; set; }
        /// <summary>
        /// A semi-colon separated list of field names, e.g. Id;Name
        /// <para>The fields in this list are used in constructing a SELECT statement.</para>
        /// <para>NOTE: The <see cref="ForeignKeyField"/> must be included in this list.</para>
        /// <para>NOTE: When this property has a value then the <see cref="ForeignTableSql"/> is not used.</para>
        /// </summary>
        public string ForeignFieldList { get; set; }
        /// <summary>
        /// A SELECT statement to be used instead of the <see cref="ForeignFieldList"/>.
        /// <para>NOTE: The <see cref="ForeignKeyField"/> must be included in this SELECT statement.</para>
        /// </summary>
        public string ForeignTableSql { get; set; }

        /// <summary>
        /// The name of a <see cref="LocatorDef"/> to be used with this field.
        /// </summary>
        public string LocatorName { get; set; }

        /// <summary>
        /// Returns true when the Required flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsRequired => (SqlBrokerFieldFlag.Required & Flags) == SqlBrokerFieldFlag.Required;
        /// <summary>
        /// Returns true when the IsHidden flag is NOT set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsVisible => !IsHidden;
        /// <summary>
        /// Returns true when the IsHidden flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsHidden => (SqlBrokerFieldFlag.Hidden & Flags) == SqlBrokerFieldFlag.Hidden;
        /// <summary>
        /// Returns true when the ReadOnly flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnly => (SqlBrokerFieldFlag.ReadOnly & Flags) == SqlBrokerFieldFlag.ReadOnly;
        /// <summary>
        /// Returns true when the ReadOnlyUI flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnlyUI => (SqlBrokerFieldFlag.ReadOnlyUI & Flags) == SqlBrokerFieldFlag.ReadOnlyUI;
        /// <summary>
        /// Returns true when the ReadOnlyEdit flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnlyEdit => (SqlBrokerFieldFlag.ReadOnlyEdit & Flags) == SqlBrokerFieldFlag.ReadOnlyEdit;

        /// <summary>
        /// Returns true when the Boolean flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsBoolean => (SqlBrokerFieldFlag.Boolean & Flags) == SqlBrokerFieldFlag.Boolean;
        /// <summary>
        /// Returns true when the Memo flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsMemo => (SqlBrokerFieldFlag.Memo & Flags) == SqlBrokerFieldFlag.Memo;
        /// <summary>
        /// Returns true when the Image flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsImage => (SqlBrokerFieldFlag.Image & Flags) == SqlBrokerFieldFlag.Image;
        /// <summary>
        /// Returns true when the ImagePath flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsImagePath => (SqlBrokerFieldFlag.ImagePath & Flags) == SqlBrokerFieldFlag.ImagePath;
        /// <summary>
        /// Returns true when the Searchable flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsSearchable => (SqlBrokerFieldFlag.Searchable & Flags) == SqlBrokerFieldFlag.Searchable;
        /// <summary>
        /// Returns true when the Extra flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsExtraField => (SqlBrokerFieldFlag.Extra & Flags) == SqlBrokerFieldFlag.Extra;
        /// <summary>
        /// Returns true when the Extra flag is NOT set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsNativeField { get { return !IsExtraField; } }
        /// <summary>
        /// Returns true when the ForeignKey flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsForeignKeyField => (SqlBrokerFieldFlag.ForeignKey & Flags) == SqlBrokerFieldFlag.ForeignKey;

        /// <summary>
        /// Returns true when the NoInsertUpdate flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsNoInsertOrUpdate => (SqlBrokerFieldFlag.NoInsertUpdate & Flags) == SqlBrokerFieldFlag.NoInsertUpdate;
        /// <summary>
        /// Returns true when the Localizable flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsLocalizable => (SqlBrokerFieldFlag.Localizable & Flags) == SqlBrokerFieldFlag.Localizable;
    }
}



 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous.Data;

namespace Tripous.Model2
{
 
 
    /// <summary>
    /// A broker field definition
    /// </summary>
    public class BrokerFieldDef
    {
        string fDefaultValue = Sys.NULL;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerFieldDef()
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
        /// The data-type of the field
        /// </summary>
        public DataFieldType DataType { get; set; }
        /// <summary>
        /// The max length of a string field
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Gets or sets the decimals of the field. Used when is a float field.
        /// </summary>
        public int Decimals { get; set; }
        /// <summary>
        /// Gets or sets the flags of the field.
        /// </summary>
        public BrokerFieldFlag Flags { get; set; }

        /// <summary>
        /// Gets or sets the default value of the field.
        /// </summary>
        public string DefaultValue
        {
            get { return string.IsNullOrEmpty(fDefaultValue) ? Sys.NULL : fDefaultValue; }
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
    }



}

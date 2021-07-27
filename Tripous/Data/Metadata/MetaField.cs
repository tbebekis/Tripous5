/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;


namespace Tripous.Data.Metadata
{
    /// <summary>
    /// Represents schema information for a field in a table
    /// </summary>
    public class MetaField : NamedItem, IMetaNode
    {

        private string defaultValue;
        private string maxLength;
        private string precision;
        private string scale;


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaField()
        {
        }


        /* properties */
        /// <summary>
        /// Get the table this field belongs to
        /// </summary>
        public MetaTable Table { get { return CollectionOwner as MetaTable; } }
        /// <summary>
        /// Gets the display text for the field, i.e. FIELD_NAME datatype(size), null
        /// </summary>
        public string DisplayText
        {
            get
            {
                StringBuilder SB = new StringBuilder();

                SB.Append(Name);

                if (IsPrimaryKey)
                    SB.Append(" PK");

                SB.Append(" " + DataType);

                if ((MetaType != null) && MetaType.IsString)
                    SB.Append(string.Format("({0})", MaxLength));

                SB.Append(string.Format(" {0}", IsNullable ? "null" : "not null"));

                if (IsIdentity)
                    SB.Append(" Identity");

                if (IsUniqueKey)
                    SB.Append(" Unique");

                return SB.ToString();
            }

        }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Field; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }

        
        /// <summary>
        /// Gets the ordinal position of this field in the table
        /// </summary>
        public int Ordinal { get; set; } 
        /// <summary>
        /// Gets the default value, if any, defined for the field, else SysConst.NULL
        /// </summary>
        public string DefaultValue
        {
            get { return string.IsNullOrEmpty(defaultValue) ? string.Empty : defaultValue; }
            set { defaultValue = value; }
        }
        /// <summary>
        /// Get a boolean valued indicating whether this field is nullable
        /// </summary>
        public bool IsNullable { get; set; } 
        /// <summary>
        /// Gets the meta type object, that is an object with data type information regarding this field
        /// </summary>
        public MetaType MetaType { get; set; }
        /// <summary>
        /// Gets the data type as it is defined in the CREATE TABLE (ie varchar, int, float etc)
        /// </summary>
        public string DataType { get { return MetaType != null ? MetaType.Name : string.Empty; } }
        /// <summary>
        /// Gets the .Net data type of this field, ie System.String, System.Double etc
        /// </summary>
        public string NetType { get { return MetaType != null ? MetaType.NetType : string.Empty; } }
        /// <summary>
        /// Gets the maximum length of a string (varchar) field
        /// </summary>
        public string MaxLength
        {
            get { return string.IsNullOrEmpty(maxLength) ? string.Empty : maxLength; }
            set { maxLength = value; }
        }
        /// <summary>
        /// Gets the precision. 
        /// <para>Valid when this is a float etc field</para>
        /// <para>Precision is the number of digits in a number. For example, the number 123.45 has a precision of 5 and a scale of 2.</para>
        /// </summary>
        public string Precision
        {
            get { return string.IsNullOrEmpty(precision) ? string.Empty : precision; }
            set { precision = value; }
        }
        /// <summary>
        /// Gets the scale. 
        /// <para>Valid when this is a float etc field</para>
        /// <para>Precision is the number of digits in a number. For example, the number 123.45 has a precision of 5 and a scale of 2.</para>
        /// </summary>
        public string Scale
        {
            get { return string.IsNullOrEmpty(scale) ? string.Empty : scale; }
            set { scale = value; }
        }

        /* flags */
        /// <summary>
        /// True when this is an identity column (auto-increment, etc)
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// True when this is a primary key field
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// True when this is a unique key field
        /// </summary>
        public bool IsUniqueKey { get; set; }
    }
}

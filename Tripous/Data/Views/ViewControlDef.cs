using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
 
    /// <summary>
    /// Represents a row in a column.
    /// <para>The row is the control container along with its caption text.</para>
    /// <para>The control may be data-bindable or not.</para>
    /// </summary>
    public class ViewControlDef
    {
        /// <summary>
        /// Constant
        /// </summary>
        public const string TextBox = "TextBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string CheckBox = "CheckBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string DateBox = "DateBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string ComboBox = "ComboBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string ListBox = "ListBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string Memo = "Memo";
        /// <summary>
        /// Constant
        /// </summary>
        public const string LocatorBox = "LocatorBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string NumberBox = "NumberBox";
        /// <summary>
        /// Constant
        /// </summary>
        public const string Grid = "Grid";

 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDef()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDef(SqlBrokerFieldDef Field)
        {
            Title = Field.Title;
            TitleKey = Field.TitleKey;

            DataField = Field.Name;

            ReadOnly = Field.IsReadOnly;
            Required = Field.IsRequired;
            TypeName = GetTypeName(Field);
        }

        /* static */
        /// <summary>
        /// Returns the 'TypeName' of a field definition, e.g. 'TextBox', 'CheckBox', etc.
        /// </summary>
        static public string GetTypeName(SqlBrokerFieldDef Field)
        {
            switch (Field.DataType)
            {
                case DataFieldType.String:
                    return Bf.In(FieldFlags.Memo, Field.Flags) ? Memo : TextBox;
  

                case DataFieldType.Integer:
                    return Bf.In(FieldFlags.Boolean, Field.Flags) ? CheckBox : NumberBox;
 
                case DataFieldType.Float:
                case DataFieldType.Decimal:
                    return NumberBox;
 
                case DataFieldType.Date:
                case DataFieldType.DateTime:
                    return DateBox;  
                case DataFieldType.Boolean:
                    return CheckBox;
 
                case DataFieldType.Memo:
                    return Memo;

            }

            return TextBox;
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Title;
        }

        /* properties */
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Sys.None; }
            set { }
        }

        /// <summary>
        /// The HTML Id and HTML Name of the control.
        /// <para>The name of a desktop control.</para>
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// Indicates the control type, such as TextBox
        /// </summary>
        public string TypeName { get; set; } = TextBox;
        /// <summary>
        /// When true the control is readonly
        /// </summary>
        public bool ReadOnly { get; set; }
        /// <summary>
        /// When true the control must have a value
        /// </summary>
        public bool Required { get; set; }

 
        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; } = "";
        /// <summary>
        /// The data field to bind
        /// </summary>
        public string DataField { get; set; } = "";
    }
}

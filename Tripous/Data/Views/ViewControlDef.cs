using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
 
    /// <summary>
    /// Represents a row in a <see cref="UiColumnInfo"/>.
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewControlDef()
        {

        }

        /// <summary>
        /// The caption text of the control
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// The HTML Id and HTML Name of the control.
        /// <para>The name of a desktop control.</para>
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Indicates the control type, such as TextBox
        /// </summary>
        public string TypeName { get; set; }
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
        public string SourceName { get; set; }
        /// <summary>
        /// The data field to bind
        /// </summary>
        public string DataField { get; set; }
    }
}

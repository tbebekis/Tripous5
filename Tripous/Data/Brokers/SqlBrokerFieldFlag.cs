using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Tripous.Data
{
    /// <summary>
    /// A list of possible field flags.
    /// </summary>
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SqlBrokerFieldFlag
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Must be visible
        /// </summary>
        Hidden = 1,
        /// <summary>
        /// Determines whether the field can be modified
        /// </summary>
        ReadOnly = 2,
        /// <summary>
        /// Concerns controls that display the field    
        /// </summary>
        ReadOnlyUI = 4,
        /// <summary>
        /// The field is editable when inserting only
        /// </summary>
        ReadOnlyEdit = 8,
        /// <summary>
        /// Can not be null
        /// </summary>
        Required = 0x10,
        /// <summary>
        /// It is an integer field that must be displayed in a check box control. 0 = false, 1 = true.
        /// </summary>
        Boolean = 0x20,
        /// <summary>
        /// A memo text field, text blob or just varchar. Must by displayed in a multi-line editor.
        /// </summary>
        Memo = 0x40,
        /// <summary>
        /// An HTML memo field. . Must by displayed in a multi-line editor.
        /// </summary>
        HtmlMemo = 0x80,
        /// <summary>
        /// An image blob field.
        /// </summary>
        Image = 0x100,
        /// <summary>
        /// A string field that contains a path to an image
        /// </summary>
        ImagePath = 0x200,
        /// <summary>
        /// The field generates a criterion item 
        /// </summary>
        Searchable = 0x400,
        /// <summary>
        /// The field does NOT exist in the database. It just added to the DataTable schema for some reason.
        /// </summary>
        Extra = 0x800,
        /// <summary>
        /// It is a foreign key field.  
        /// </summary>
        ForeignKey = 0x1000,
        /// <summary>
        /// The field is not used with INSERT or UPDATE statements. 
        /// <para>It maybe something like the ExtraField or an identity/autoinc field,
        /// in a position other than that of a primary key</para>
        /// </summary>
        NoInsertUpdate = 0x2000,
        /// <summary>
        /// Must be a string or memo field.
        /// </summary>
        Localizable = 0x4000, 

    }
}

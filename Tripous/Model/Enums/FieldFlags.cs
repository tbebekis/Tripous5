/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Model
{
    /// <summary>
    /// A list of possible field flags.
    /// </summary>
    [Flags]
    public enum FieldFlags
    {
        /// <summary>
        /// Container of the flags is set
        /// </summary>
        None = 0,
        /// <summary>
        /// Must be visible
        /// </summary>
        Visible = 1,
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
        /// A memo text field, text blob or just varchar. Must by displayed in a multi-line edit.
        /// </summary>
        Memo = 0x40,
        /// <summary>
        /// An image blob field.
        /// </summary>
        Image = 0x80,
        /// <summary>
        /// A string field that contains a path to an image
        /// </summary>
        ImagePath = 0x100,
        /// <summary>
        /// The field generates a criterion item 
        /// </summary>
        Searchable = 0x200,
        /// <summary>
        /// The field does NOT exist in the database. It just added to the DataTable schema for some reason.
        /// </summary>
        ExtraField = 0x400,
        /// <summary>
        /// It is a look up field. A field that is added using the FieldDescriptors.AddLookUp() method
        /// </summary>
        LookUpField = 0x800,
        /// <summary>
        /// The field is not used with INSERT or UPDATE statements. 
        /// <para>It maybe something like the ExtraField or an identity/autoinc field,
        /// in a position other than that of a primary key</para>
        /// </summary>
        NoInsertUpdate = 0x1000,
        /// <summary>
        /// Must be a string or memo field.
        /// </summary>
        Localizable = 0x2000,
    }


    /// <summary>
    /// FieldFlags shortcuts
    /// </summary>
    static public class FF
    {
        /// <summary>
        /// Visible | Searchable | Required
        /// </summary>
        public const FieldFlags VSR = FieldFlags.Visible | FieldFlags.Searchable | FieldFlags.Required;
        /// <summary>
        /// Visible | Searchable | Required | ReadOnlyUI
        /// </summary>
        public const FieldFlags Code = VSR | FieldFlags.ReadOnlyUI;
        /// <summary>
        /// Visible | Required
        /// </summary>
        public const FieldFlags VR = FieldFlags.Visible | FieldFlags.Required;
    }
}

/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using Tripous.Data;


namespace Tripous.Model
{
    /// <summary>
    /// A base class used to inherit field Descriptor classes.
    /// </summary>
    public class FieldDescriptorBase : Descriptor
    {
        private SimpleType dataType = SimpleType.None;
        private int size = -1;
        private int displayWidth = 0;
        private int decimals = 0;
        private string zoomCommand = "";

 
 

        /// <summary>
        /// Returns the Alias.
        /// </summary>
        protected override string GetAlias()
        {
            if (!string.IsNullOrEmpty(fAlias))
                return fAlias;
            else
            {
                if (!string.IsNullOrEmpty(TableAlias))
                    return Sys.FieldAlias(TableAlias, Name);
                else
                    return Name;
            }
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldDescriptorBase()
        {
        }

        /* methods */
        /// <summary>
        /// Returns a string representation of this instance;
        /// </summary>
        public override string ToString()
        {
            return FullName + " (" + Title + ")" + " - (" + Alias + ")";
        }

        /// <summary>
        /// Override
        /// </summary>
        public override void CheckDescriptor()
        {
            base.CheckDescriptor();

            if (DataType == SimpleType.None)
                NotFullyDefinedError("DataType");

        }

        /* properties */
        /// <summary>
        /// Gets or sets the data type of the field.
        /// </summary>
        public SimpleType DataType
        {
            get { return dataType; }
            set
            {
                if (dataType != value)
                {
                    dataType = value;
                    if ((dataType == SimpleType.Float) && (Decimals <= 0))
                        Decimals = 2;
                }
            }
        }
        /// <summary>
        /// Gets or sets the size (max length) of the field.
        /// </summary>
        public int Size
        {
            get
            {
                return size > 0 ? size : -1;
            }
            set { size = value; }
        }
        /// <summary>
        /// Gets or sets the display width of the field. Used when it is displayed in grids.
        /// </summary>
        public int DisplayWidth
        {
            get { return displayWidth >= 0 ? displayWidth : 0; }
            set { displayWidth = value; }
        }
        /// <summary>
        /// Gets or sets the decimals of the field. Used when is a float field.
        /// </summary>
        public int Decimals
        {
            get { return decimals >= 0 ? decimals : 0; }
            set { decimals = value; }
        }
        /// <summary>
        /// Gets or sets the <see cref="FieldFlags"/> flags of the field.
        /// </summary>
        public FieldFlags Flags { get; set; }
        /// <summary>
        /// Gets or sets the zoom  command path. 
        /// <para>A zoom command is used by locators and other drill-down controls.</para>
        /// <para>It is something similar to PROCESSOR.COMMAND. For example MAIN_PROCESSOR.CUSTOMER</para>
        /// </summary>
        public string ZoomCommand
        {
            get { return string.IsNullOrEmpty(zoomCommand) ? "" : zoomCommand; }
            set { zoomCommand = value; }
        }


        /// <summary>
        /// Returns true when the <see cref="FieldFlags.Visible"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsVisible { get { return (FieldFlags.Visible & Flags) == FieldFlags.Visible; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.ReadOnly"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsReadOnly { get { return (FieldFlags.ReadOnly & Flags) == FieldFlags.ReadOnly; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.ReadOnlyUI"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsReadOnlyUI { get { return (FieldFlags.ReadOnlyUI & Flags) == FieldFlags.ReadOnlyUI; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.ReadOnlyEdit"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsReadOnlyEdit { get { return (FieldFlags.ReadOnlyEdit & Flags) == FieldFlags.ReadOnlyEdit; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.Required"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsRequired { get { return (FieldFlags.Required & Flags) == FieldFlags.Required; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.Boolean"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsBoolean { get { return (FieldFlags.Boolean & Flags) == FieldFlags.Boolean; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.Memo"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsMemo { get { return (FieldFlags.Memo & Flags) == FieldFlags.Memo; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.Image"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsImage { get { return (FieldFlags.Image & Flags) == FieldFlags.Image; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.ImagePath"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsImagePath { get { return (FieldFlags.ImagePath & Flags) == FieldFlags.ImagePath; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.Searchable"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsSearchable { get { return (FieldFlags.Searchable & Flags) == FieldFlags.Searchable; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.ExtraField"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsExtraField { get { return (FieldFlags.ExtraField & Flags) == FieldFlags.ExtraField; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.LookUpField"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsLookUpField { get { return (FieldFlags.LookUpField & Flags) == FieldFlags.LookUpField; } }
        /// <summary>
        /// Returns true if the field is not a lookup or extra field. That is, is a field that belongs to the underlying table.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsNativeField { get { return !IsExtraField; } }
        /// <summary>
        /// Returns true when the <see cref="FieldFlags.NoInsertUpdate"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsNoInsertOrUpdate { get { return (FieldFlags.NoInsertUpdate & Flags) == FieldFlags.NoInsertUpdate; } }
        /// <summary>
        ///  Returns true when the <see cref="FieldFlags.Localizable"/> flag is set in <see cref="Flags"/>.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool IsLocalizable { get { return (FieldFlags.Localizable & Flags) == FieldFlags.Localizable; } }


        /// <summary>
        /// Gets the table descriptor this field belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public TableDescriptorBase Table { get { return CollectionOwner as TableDescriptorBase; } }
        /// <summary>
        /// Returns the connection info of the database this field belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public SqlConnectionInfo ConnectionInfo
        {
            get
            {
                if (Table != null)
                    return Table.ConnectionInfo;
                return null;
            }
        }
        /// <summary>
        /// Returns the table Name of the table this field belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string TableName { get { return Table == null ? string.Empty : Table.Name; } }
        /// <summary>
        /// Returns the table Alias of the table this field belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string TableAlias { get { return Table == null ? string.Empty : Table.Alias; } }
        /// <summary>
        /// Returns the table Title of the table this field belongs to.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string TableTitle { get { return Table == null ? string.Empty : Table.Title; } }
        /// <summary>
        /// Returns TABLE_NAME.FIELD_NAME
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string FullName { get { return Table == null ? Name : Table.Name + "." + Name; } }

    }
}

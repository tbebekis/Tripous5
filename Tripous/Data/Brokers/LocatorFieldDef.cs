﻿namespace Tripous.Data
{

    /// <summary>
    /// Describes a Locator field.
    /// <para>A field such that may associate a column in the data table (the dest) to a column in the list table (the source).</para>
    /// </summary>
    public class LocatorFieldDef
    {
        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorFieldDef()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public LocatorFieldDef(string Name, string DataField, string TitleKey, DataFieldType DataType = DataFieldType.String, 
                                bool Visible = true, bool Searchable = true, bool ListVisible = true, string TableName = "")
        {
            this.Name = Name;
            this.DataField = DataField;
            this.TitleKey = TitleKey;
            this.DataType = DataType;

            this.Visible = Visible;
            this.Searchable = Searchable;
            this.ListVisible = ListVisible;

            this.TableName = TableName;
            
        }


        /* public */
        /// <summary>
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                Sys.Throw(Res.GS("E_LocatorFieldDef_NameIsEmpty", "LocatorFieldDef Name is empty"));

            if (string.IsNullOrWhiteSpace(this.TableName))
                Sys.Throw(Res.GS("E_LocatorFieldDef_TableNameIsEmpty", "LocatorFieldDef TableName is empty"));
        }


        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetDataField(string Value)
        {
            this.DataField = Value;
            return this;
        }
        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetTitle(string Value)
        {
            this.Title = Value;
            return this;
        }

        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetVisible(bool Value)
        {
            this.Visible = Visible;
            return this;
        }
        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetSearchable(bool Value)
        {
            this.Searchable = Searchable;
            return this;
        }
        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetListVisible(bool Value)
        {
            this.ListVisible = ListVisible;
            return this;
        }
        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetIsIntegerBoolean(bool Value)
        {
            this.IsIntegerBoolean = IsIntegerBoolean;
            return this;
        }

        /// <summary>
        /// Sets a property and returns this instance.
        /// </summary>
        public LocatorFieldDef SetWidth(int Value)
        {
            this.Width = Width;
            return this;
        }

        /* properties */
        /// <summary>
        /// The field name in the list (source) table
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The table name of the list (source) table
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// When not empty/null then it denotes a field in the dest data table where to put the value of this field.
        /// </summary>
        public string DataField { get; set; }
        /// <summary>
        /// The data-type of the field
        /// </summary>
        public DataFieldType DataType { get; set; } = DataFieldType.String;

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
        /// Indicates whether a TextBox for this field is visible in a LocatorBox
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// When true the field can be part in a where clause in a SELECT statement.
        /// </summary>
        public bool Searchable { get; set; } = true;
        /// <summary>
        /// Indicates whether the field is visible when the list table is displayed
        /// </summary>
        public bool ListVisible { get; set; } = true;


        /// <summary>
        /// Used to notify criterial links to treat the field as an integer boolea field (1 = true, 0 = false)
        /// </summary>
        public bool IsIntegerBoolean { get; set; }

        /// <summary>
        ///  Controls the width of the text box in a LocatorBox. In pixels.
        /// </summary>
        public int Width { get; set; } = 70;


    }
}

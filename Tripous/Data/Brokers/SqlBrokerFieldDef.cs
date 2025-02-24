﻿namespace Tripous.Data
{
 
 
    /// <summary>
    /// A broker field definition
    /// </summary>
    public class SqlBrokerFieldDef
    {
        string fDefaultValue = Sys.NULL;
        string fTitleKey;
        string fAlias;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrokerFieldDef()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrokerFieldDef(SqlBrokerTableDef TableDef)
        {
            this.TableDef = TableDef;
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
        /// Throws an exception if this descriptor is not fully defined
        /// </summary>
        public virtual void CheckDescriptor()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                Sys.Throw(Res.GS("E_SqlBrokerFieldDef_NameIsEmpty", "SqlBrokerFieldDef Name is empty"));

            if (string.IsNullOrWhiteSpace(this.Alias))
                Sys.Throw(Res.GS("E_SqlBrokerFieldDef_TextIsEmpty", "SqlBrokerFieldDef Alias  is empty. "));

            if (this.DataType == DataFieldType.Unknown)
                Sys.Throw(Res.GS("E_SqlBrokerFieldDef_DataTypeIsEmpty", "SqlBrokerFieldDef DataType is Unknown. "));
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
            if (string.IsNullOrWhiteSpace(LookUpTableName) 
                || string.IsNullOrWhiteSpace(LookUpKeyField)
                || (string.IsNullOrWhiteSpace(LookUpFieldList) && string.IsNullOrWhiteSpace(LookUpTableSql)))
                Sys.Throw($"Broker Field not fully defined: {Name}");

            if (!string.IsNullOrWhiteSpace(LookUpTableSql))
                return LookUpTableSql;

            List<string> List = new List<string>();
            List.Add(LookUpKeyField);

            string[] FieldNames = LookUpFieldList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            List.AddRange(FieldNames);
 
            string SqlText = $@" 
select 
    {List.CommaText()} 
from 
{LookUpTableName} 
";

            return SqlText;
        }

        /* for fluent syntax */
        /// <summary>
        /// Sets the <see cref="Alias"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetAlias(string Value)
        {
            this.Alias = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="Title"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetTitle(string Value)
        {
            this.Title = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="TitleKey"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetTitleKey(string Value)
        {
            this.TitleKey = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="MaxLength"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetMaxLength(int Value)
        {
            this.MaxLength = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="Decimals"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetDecimals(int Value)
        {
            this.Decimals = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="Flags"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetFlags(FieldFlags Value)
        {
            bool IsBoolean = Bf.In(FieldFlags.Boolean, this.Flags);
            this.Flags = Value;
            if (IsBoolean)
                this.Flags |= FieldFlags.Boolean;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="CodeProviderName"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetCodeProviderName(string Value)
        {
            this.CodeProviderName = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="DefaultValue"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetDefaultValue(string Value)
        {
            this.DefaultValue = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="Expression"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetExpression(string Value)
        {
            this.Expression = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="LookUpTableName"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetForeignTableName(string Value)
        {
            this.LookUpTableName = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="LookUpTableAlias"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetForeignTableAlias(string Value)
        {
            this.LookUpTableAlias = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="LookUpKeyField"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetForeignKeyField(string Value)
        {
            this.LookUpKeyField = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="LookUpFieldList"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetForeignFieldList(string Value)
        {
            this.LookUpFieldList = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="LookUpTableSql"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetForeignTableSql(string Value)
        {
            this.LookUpTableSql = Value;
            return this;
        }
        /// <summary>
        /// Sets the <see cref="LocatorName"/> and returns this instance.
        /// </summary>
        public SqlBrokerFieldDef SetLocatorName(string Value)
        {
            this.LocatorName = Value;
            return this;
        }

        /* properties */
        // SqlBrokerTableDef

        /// <summary>
        /// The master definition this instance belongs to.
        /// </summary>
        [JsonIgnore]
        public SqlBrokerTableDef TableDef { get; }
        /// <summary>
        /// The field name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An alias of this field
        /// </summary>
        public string Alias
        {
            get { return !string.IsNullOrWhiteSpace(fAlias) ? fAlias : Name; }
            set { fAlias = value; }
        }

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
        /// The data-type of the field
        /// </summary>
        public DataFieldType DataType { get; set; }
        /// <summary>
        /// The max length of a string field
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// Gets or sets the decimals of the field. Used when is a float field. -1 means is not set.
        /// </summary>
        public int Decimals { get; set; } = -1;
        /// <summary>
        /// Gets or sets the flags of the field.
        /// </summary>
        public FieldFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the Name of the code producer descriptor associated to this field.
        /// </summary>
        public string CodeProviderName { get; set; }

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
        /// <para><strong>NOTE:</strong>This idea comes from old Tripous versions where it was used with LookUp controls such as ComboBox.
        /// For examples of use in UIs check the Tripous2 ControlHandlerStandard class the Bind() method.
        /// </para>
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
        public string LookUpTableName { get; set; }
        /// <summary>
        /// The alias of a foreign table this field points to, if any, else null.
        /// </summary>
        public string LookUpTableAlias { get; set; }
        /// <summary>
        /// The name of the field of the foreign table that becomes the <strong>result</strong> of a look-up operation
        /// </summary>
        public string LookUpKeyField { get; set; }
        /// <summary>
        /// A semi-colon separated list of field names, e.g. Id;Name
        /// <para>The fields in this list are used in constructing a SELECT statement.</para>
        /// <para>NOTE: The <see cref="LookUpKeyField"/> must be included in this list.</para>
        /// <para>NOTE: When this property has a value then the <see cref="LookUpTableSql"/> is not used.</para>
        /// </summary>
        public string LookUpFieldList { get; set; }
        /// <summary>
        /// A SELECT statement to be used instead of the <see cref="LookUpFieldList"/>.
        /// <para>NOTE: The <see cref="LookUpKeyField"/> must be included in this SELECT statement.</para>
        /// </summary>
        public string LookUpTableSql { get; set; }
 
        /// <summary>
        /// The name of a <see cref="LocatorDef"/> to be used with this field.
        /// </summary>
        public string LocatorName { get; set; }

        /// <summary>
        /// Returns true when the Required flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsRequired => (FieldFlags.Required & Flags) == FieldFlags.Required;
        /// <summary>
        /// Returns true when the IsHidden flag is NOT set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsVisible => !IsHidden;
        /// <summary>
        /// Returns true when the IsHidden flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsHidden => (FieldFlags.Hidden & Flags) == FieldFlags.Hidden;
        /// <summary>
        /// Returns true when the ReadOnly flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnly => (FieldFlags.ReadOnly & Flags) == FieldFlags.ReadOnly;
        /// <summary>
        /// Returns true when the ReadOnlyUI flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnlyUI => (FieldFlags.ReadOnlyUI & Flags) == FieldFlags.ReadOnlyUI;
        /// <summary>
        /// Returns true when the ReadOnlyEdit flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnlyEdit => (FieldFlags.ReadOnlyEdit & Flags) == FieldFlags.ReadOnlyEdit;

        /// <summary>
        /// Returns true when the Boolean flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsBoolean => (FieldFlags.Boolean & Flags) == FieldFlags.Boolean;
        /// <summary>
        /// Returns true when the Memo flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsMemo => (FieldFlags.Memo & Flags) == FieldFlags.Memo;
        /// <summary>
        /// Returns true when the Image flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsImage => (FieldFlags.Image & Flags) == FieldFlags.Image;
        /// <summary>
        /// Returns true when the ImagePath flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsImagePath => (FieldFlags.ImagePath & Flags) == FieldFlags.ImagePath;
        /// <summary>
        /// Returns true when the Searchable flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsSearchable => (FieldFlags.Searchable & Flags) == FieldFlags.Searchable;
        /// <summary>
        /// Returns true when the Extra flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsExtraField => (FieldFlags.Extra & Flags) == FieldFlags.Extra;
        /// <summary>
        /// Returns true when the Extra flag is NOT set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsNativeField { get { return !IsExtraField; } }
        /// <summary>
        /// Returns true when the ForeignKey flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsForeignKeyField => (FieldFlags.ForeignKey & Flags) == FieldFlags.ForeignKey;

        /// <summary>
        /// Returns true when the NoInsertUpdate flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsNoInsertOrUpdate => (FieldFlags.NoInsertUpdate & Flags) == FieldFlags.NoInsertUpdate;
        /// <summary>
        /// Returns true when the Localizable flag is set in Flags.
        /// </summary>
        [JsonIgnore]
        public bool IsLocalizable => (FieldFlags.Localizable & Flags) == FieldFlags.Localizable;
    }
}



 
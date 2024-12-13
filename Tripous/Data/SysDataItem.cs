namespace Tripous.Data
{
  

    /// <summary>
    /// Represents the data of a row in the system data table.
    /// <para>System data table is a database table which stores information
    /// regarding system data, such as reports, descriptors, resources etc.</para>
    /// </summary>
    public class SysDataItem  
    {
        string fTitleKey;
 
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SysDataItem()
        {
        }

        /* methods */
        /// <summary>
        /// Clears the property values of this instance.
        /// </summary>
        public void Clear()
        {
            DataType = string.Empty;    
            DataName = string.Empty;
            TitleKey = string.Empty;
            Notes = string.Empty;

            Tag1 = string.Empty;
            Tag2 = string.Empty;

            Data1 = string.Empty;
            Data2 = string.Empty;
            Data3 = string.Empty;
            Data4 = string.Empty;
        }


        /// <summary>
        /// Loads this instance from a file
        /// </summary>
        public virtual void LoadFromFile(string FilePath)
        {
            Clear();
            Json.LoadFromFile(this, FilePath);
        }
        /// <summary>
        /// Saves this instance to FileName
        /// </summary>
        public virtual void SaveToFile(string FilePath)
        {
            Json.SaveToFile(this, FilePath);
        }

        /// <summary>
        /// Loads this instance from Stream
        /// </summary>
        public virtual void LoadFromStream(Stream Stream)
        {
            if (Stream != null)
            {
                Clear();
                Stream.Position = 0;
                Json.FromJsonStream(this, Stream);
            }
        }
        /// <summary>
        /// Saves this instance to Stream
        /// </summary>
        public virtual void SaveToStream(Stream Stream)
        {
            if (Stream != null)
            {
                Json.ToJsonStream(this, Stream);
            }
        }

        /// <summary>
        /// Loads the Row to this instance.
        /// The Table must have the schema of the SYS_DATA database table.
        /// The Data1 blob field contains, as xml, all the properties, Definition included.
        /// </summary>
        public virtual void LoadFromRow(DataRow Row)
        {
            if (Row != null)
            {
                /* maybe there are different values in the fields */
                DataType = Sys.AsString(Row["DataType"], DataType);
                DataName = Sys.AsString(Row["DataName"], DataName);
                TitleKey = Sys.AsString(Row["TitleKey"], TitleKey);

                Notes = Sys.AsString(Row["Notes"], Notes);

                Owner = Sys.AsString(Row["Owner"], Owner);

                Tag1 = Sys.AsString(Row["Tag1"], Tag1);
                Tag2 = Sys.AsString(Row["Tag2"], Tag2);
                Tag3 = Sys.AsString(Row["Tag3"], Tag3);
                Tag4 = Sys.AsString(Row["Tag4"], Tag4);

                Data1 = Row.BlobToString("Data1");
                Data2 = Row.BlobToString("Data2");
                Data3 = Row.BlobToString("Data3");
                Data4 = Row.BlobToString("Data4"); 
            }
        }
        /// <summary>
        /// Saves this instance data to the Row.
        /// Properties are saved twice.
        /// First in the table related fields
        /// and second all together as xml, Definition included, into the Data1 blob field.
        /// Besides that the Definition stream alone is saved, again, into the Data3 blob field.
        /// </summary>
        public virtual void SaveToRow(DataRow Row)
        {
            if (Row != null)
            {
                Row["DataType"] = DataType;
                Row["DataName"] = DataName;
                Row["TitleKey"] = TitleKey;

                Row["Notes"] = Notes;

                Row["Owner"] = Owner;

                Row["Tag1"] = Tag1;
                Row["Tag2"] = Tag2;
                Row["Tag3"] = Tag3;
                Row["Tag4"] = Tag4;

                Row.StringToBlob("Data1", Data1);
                Row.StringToBlob("Data2", Data2);
                Row.StringToBlob("Data3", Data3);
                Row.StringToBlob("Data4", Data4); 
            }
        }

        /// <summary>
        /// Saves this instance to a browser Row in a safe manner,
        /// that is it first checks to see if a field exists in the specified Row.
        /// <para>NOTE: DataX fields are not saved.</para>
        /// </summary>
        public virtual void SaveToBrowserRow(DataRow Row)
        {
            if (Row != null)
            {
                DataTable Table = Row.Table;
                if (Table.ContainsColumn("DataType"))
                    Row["DataType"] = DataType;
                if (Table.ContainsColumn("DataName"))
                    Row["DataName"] = DataName;

                if (Table.ContainsColumn("TitleKey"))
                    Row["TitleKey"] = TitleKey;
                if (Table.ContainsColumn("Notes"))
                    Row["Notes"] = Notes;

                if (Table.ContainsColumn("Owner"))
                    Row["Owner"] = Owner;

                if (Table.ContainsColumn("Tag1"))
                    Row["Tag1"] = Tag1;
                if (Table.ContainsColumn("Tag2"))
                    Row["Tag2"] = Tag2;
                if (Table.ContainsColumn("Tag3"))
                    Row["Tag3"] = Tag3;
                if (Table.ContainsColumn("Tag4"))
                    Row["Tag4"] = Tag4;

            }
        }

        /// <summary>
        /// Loads this item from the database using the DataType and DataName
        /// <para>Returns the Id of the SYS_DATA</para>
        /// </summary>
        public virtual object Load()
        {
            SysData.Select(this);

            return this;
        }
        /// <summary>
        /// Saves this item to the database using the DataType and DataName
        /// <para>Returns the Id of the SYS_DATA</para>
        /// </summary>
        public virtual object Save()
        {
            if (!IsValidItem)
                Sys.Throw("Can not save SysDataItem. Invalid SysDataItem");

            SysData.Save(this);

            return this;
        }

        /* properties */
        /// <summary>
        /// Gets or sets the data type
        /// </summary>
        public virtual string DataType { get; set; } = Sys.None;
        /// <summary>
        /// Gets or sets the data name
        /// </summary>
        public virtual string DataName { get; set; } = Sys.None;
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : DataName; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : DataName; }
            set { }
        }
        /// <summary>
        /// Gets or sets the notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// A string indicating the owner of the entry in the table.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets a user defined string tag.
        /// </summary>
        public string Tag1 { get; set; }
        /// <summary>
        /// Gets or sets a user defined string tag.
        /// </summary>
        public string Tag2 { get; set; }
        /// <summary>
        /// Gets or sets a user defined string tag.
        /// </summary>
        public string Tag3 { get; set; }
        /// <summary>
        /// Gets or sets a user defined string tag.
        /// </summary>
        public string Tag4 { get; set; }

        /// <summary>
        /// Text blob data
        /// </summary>
        public string Data1 { get; set; }
        /// <summary>
        /// Text blob data
        /// </summary>
        public string Data2 { get; set; }
        /// <summary>
        /// Text blob data
        /// </summary>
        public string Data3 { get; set; }
        /// <summary>
        /// Text blob data
        /// </summary>
        public string Data4 { get; set; }

        /// <summary>
        /// Returns true if both DataType and DataName are not null or empty. 
        /// <para>Usefull for determining the validity of data after a load operation, because the Clear() is called before the operation.</para>
        /// </summary>
        [JsonIgnore]
        public virtual bool IsValidItem { get { return !string.IsNullOrWhiteSpace(DataType) && !string.IsNullOrWhiteSpace(DataName); } }
 



    }


}

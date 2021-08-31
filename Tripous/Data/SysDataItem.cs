/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.IO;

using Newtonsoft.Json;
 

namespace Tripous.Data
{
  

    /// <summary>
    /// Represents the data of a row in the system data table.
    /// <para>System data table is a database table which stores information
    /// regarding system data, such as reports, descriptors, resources etc.</para>
    /// </summary>
    public class SysDataItem  
    { 
        /// <summary>
        /// Field
        /// </summary>
        protected string fTitleKey;
 
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
            StoreName = string.Empty;  
            Notes = string.Empty;

            Category1 = string.Empty;
            Category2 = string.Empty;

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
                StoreName = Sys.AsString(Row["StoreName"], StoreName);
                Notes = Sys.AsString(Row["Notes"], Notes);

                Category1 = Sys.AsString(Row["Category1"], Category1);
                Category2 = Sys.AsString(Row["Category2"], Category2);

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
                Row["Title"] = TitleKey;
                Row["StoreName"] = StoreName;
                Row["Notes"] = Notes;

                Row["Category1"] = Category1;
                Row["Category2"] = Category2;

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
                if (Table.ContainsColumn("StoreName"))
                    Row["StoreName"] = StoreName;
                if (Table.ContainsColumn("Notes"))
                    Row["Notes"] = Notes;

                if (Table.ContainsColumn("Category1"))
                    Row["Category1"] = Category1;
                if (Table.ContainsColumn("Category2"))
                    Row["Category2"] = Category2;

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

            SysData.Commit(this);

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
        /// Gets or sets the title
        /// </summary>
        public virtual string TitleKey
        {
            get { return string.IsNullOrWhiteSpace(fTitleKey) ? DataName : fTitleKey; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets or sets the database connection name
        /// </summary>
        public virtual string StoreName { get; set; } = SysConfig.DefaultConnection;
        /// <summary>
        /// Gets or sets the notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a category
        /// </summary>
        public string Category1 { get; set; }
        /// <summary>
        /// Gets or sets a category
        /// </summary>
        public string Category2 { get; set; }

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

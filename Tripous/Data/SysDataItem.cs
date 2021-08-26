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
    public class SysDataItem : Assignable
    {
        /// <summary>
        /// Constant. The definition stream index in the dataStreams array
        /// </summary>
        public const int DEF_STREAM_INDEX = 2;

        /// <summary>
        /// Field
        /// </summary>
        protected string fDataType;
        /// <summary>
        /// Field
        /// </summary>
        protected string fDataName;
        /// <summary>
        /// Field
        /// </summary>
        protected string fTitle;
        /// <summary>
        /// Field
        /// </summary>
        protected string fStoreName;
        /// <summary>
        /// Field
        /// </summary>
        protected string fNotes;
        /// <summary>
        /// Field
        /// </summary>
        protected string fCategory1;
        /// <summary>
        /// Field
        /// </summary>
        protected string fCategory2;
 

        /// <summary>
        /// Field
        /// </summary>
        protected MemoryStream[] fStreams = new MemoryStream[6];

        /* protected */
        /// <summary>
        /// Returns the data type 
        /// </summary>
        protected virtual string GetDataType()
        {
            return !string.IsNullOrWhiteSpace(RigidDataType) ? RigidDataType : (!string.IsNullOrWhiteSpace(fDataType) ? fDataType : Sys.None);
        }
        /// <summary>
        /// Sets the data type 
        /// </summary>
        protected virtual void SetDataType(string Value)
        {
            fDataType = Sys.IsSameText(Value, Sys.None) ? string.Empty : Value;
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public SysDataItem()
        {
            for (int i = 0; i < fStreams.Length; i++)
                fStreams[i] = new MemoryStream();
        }

        /* methods */
        /// <summary>
        /// Clears the property values of this instance.
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();

            // DataType = string.Empty;     NO
            DataName = string.Empty;
            Title = string.Empty;
            // StoreName = string.Empty;    NO
            Notes = string.Empty;

            Category1 = string.Empty;
            Category2 = string.Empty;
 

            for (int i = 0; i < fStreams.Length; i++)
                fStreams[i].SetLength(0);
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
                Clear();

                /* all properties */
                Row.BlobToStream("Data1", fStreams[0]);
                fStreams[0].Position = 0;
                Json.FromJsonStream(this, fStreams[0]);

                /* maybe there are different values in the fields */
                DataType = Sys.AsString(Row["DataType"], DataType);
                DataName = Sys.AsString(Row["DataName"], DataName);
                Title = Sys.AsString(Row["Title"], Title);
                StoreName = Sys.AsString(Row["StoreName"], StoreName);
                Notes = Sys.AsString(Row["Notes"], Notes);

                Category1 = Sys.AsString(Row["Category1"], Category1);
                Category2 = Sys.AsString(Row["Category2"], Category2);
 

                /* the rest blobs */
                Row.BlobToStream("Data2", fStreams[1]);
                Row.BlobToStream("Data3", fStreams[2]);
                Row.BlobToStream("Data4", fStreams[3]);
 
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
                Row["Title"] = Title;
                Row["StoreName"] = StoreName;
                Row["Notes"] = Notes;

                Row["Category1"] = Category1;
                Row["Category2"] = Category2;
 

                /* all properties */
                Json.ToJsonStream(this, fStreams[0]);
                fStreams[0].Position = 0;
                Row.StreamToBlob("Data1", fStreams[0]);

                /* the rest blobs */
                Row.StreamToBlob("Data2", fStreams[1]);
                Row.StreamToBlob("Data3", fStreams[2]);
                Row.StreamToBlob("Data4", fStreams[3]);
 
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
                if (Table.ContainsColumn("Title"))
                    Row["Title"] = Title;
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



        /* data stream handling */
        /// <summary>
        /// Gets the data stream specified by Index
        /// </summary>
        public virtual Stream GetDataStream(int Index)
        {
            return fStreams[Index];
        }
        /// <summary>
        /// Copies the content of the Source to the data stream specified by Index
        /// </summary>
        public virtual void SetDataStream(int Index, Stream Source)
        {
            fStreams[Index].SetLength(0);
            if ((Source != null) && (Source.Length > 0))
            {
                Streams.Assign(Source, fStreams[Index]);
            }
        }

        /// <summary>
        /// Gets the data stream specified by Index as string.
        /// <para>WARNIG: The stream must contain string data.</para>
        /// </summary>
        public virtual string GetStreamAsString(int Index)
        {
            return fStreams[Index].GetAsString();
        }
        /// <summary>
        /// Sets the data stream specified by Index to Value.
        /// </summary>
        public virtual void SetStreamAsString(int Index, string Value)
        {
            fStreams[Index].SetAsString(Value);
        }

        /* when SupportsDescriptor is true */
        /// <summary>
        /// Loads this instance from the specified descriptor
        /// </summary>
        public virtual void AssignFrom(Descriptor Source)
        {
        }
        /// <summary>
        /// Saves this instance to the specified descriptor
        /// </summary>
        public virtual void AssignTo(Descriptor Dest)
        {
        }
        /// <summary>
        /// Returns the descriptor
        /// </summary>
        public virtual Descriptor GetDescriptor()
        {
            return null;
        }

        /* properties */
        /// <summary>
        /// Gets or sets the data type
        /// </summary>
        public virtual string DataType
        {
            get { return GetDataType(); }
            set
            {
                SetDataType(value);
                OnPropertyChanged("DataType");
            }
        }
        /// <summary>
        /// Gets or sets the data name
        /// </summary>
        public virtual string DataName
        {
            get { return string.IsNullOrWhiteSpace(fDataName) ? Sys.None : fDataName; }
            set
            {
                fDataName = Sys.IsSameText(value, Sys.None) ? string.Empty : value;
                OnPropertyChanged("DataName");
            }
        }
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public virtual string Title
        {
            get { return string.IsNullOrWhiteSpace(fTitle) ? DataName : fTitle; }
            set
            {
                fTitle = value;
                OnPropertyChanged("Title");
            }
        }
        /// <summary>
        /// Gets or sets the database connection name
        /// </summary>
        public virtual string StoreName
        {
            get { return string.IsNullOrWhiteSpace(fStoreName) ? SysConfig.DefaultConnection : fStoreName; }
            set
            {
                fStoreName = Sys.IsSameText(value, Sys.None) ? string.Empty : value;
                OnPropertyChanged("StoreName");
            }
        }
        /// <summary>
        /// Gets or sets the notes
        /// </summary>
        public string Notes
        {
            get { return string.IsNullOrWhiteSpace(fNotes) ? string.Empty : fNotes; }
            set
            {
                fNotes = value;
                OnPropertyChanged("Notes");
            }
        }

        /// <summary>
        /// Gets or sets a category
        /// </summary>
        public string Category1
        {
            get { return string.IsNullOrWhiteSpace(fCategory1) ? string.Empty : fCategory1; }
            set
            {
                fCategory1 = value;
                OnPropertyChanged("Category1");
            }
        }
        /// <summary>
        /// Gets or sets a category
        /// </summary>
        public string Category2
        {
            get { return string.IsNullOrWhiteSpace(fCategory2) ? string.Empty : fCategory2; }
            set
            {
                fCategory2 = value;
                OnPropertyChanged("Category2");
            }
        }

        
 
        /// <summary>
        /// Gets or sets the definition text.
        /// <para>NOTE: The definition text is a normal text. No from/to hexadecimal string convertion takes place.</para>
        /// </summary>
        public virtual string Definition
        {
            get { return GetStreamAsString(DEF_STREAM_INDEX); }   // fStreams[DEF_STREAM_INDEX].GetAsString(); 
            set { SetStreamAsString(DEF_STREAM_INDEX, value);  }  // fStreams[DEF_STREAM_INDEX].SetAsString(value);
        }
        /// <summary>
        /// Gets or "sets" (assigns) the definition stream.
        /// </summary>
        [JsonIgnore]
        public virtual Stream DefStream
        {
            get { return GetDataStream(DEF_STREAM_INDEX); }
            set { SetDataStream(DEF_STREAM_INDEX, value); }
        }
        /// <summary>
        /// Gets or sets the RigidDataType text.
        /// <para>WARNING: If RigidDataType is not null or empty then by default the 
        /// DataType property returns that RigidDataType</para>
        /// </summary>
        [JsonIgnore]
        public virtual string RigidDataType { get; protected set; }

        /// <summary>
        /// Returns true if both DataType and DataName are not null or empty. 
        /// <para>Usefull for determining the validity of data after a load operation, because the Clear() is called before the operation.</para>
        /// </summary>
        public virtual bool IsValidItem { get { return !string.IsNullOrWhiteSpace(DataType) && !string.IsNullOrWhiteSpace(DataName); } }
        /// <summary>
        /// True if this instance supports the descriptor methods
        /// that is it knows how to load/save itself to an IDescriptor
        /// </summary>
        public virtual bool SupportsDescriptor { get { return false; } }



    }


}

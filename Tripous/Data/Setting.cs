using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tripous.Parsing;

namespace Tripous.Data
{

 
    /// <summary>
    /// A setting item
    /// </summary>
    public class Setting
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Setting()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public Setting(DataRow Row)
        {
            this.LoadFrom(Row, ValueOnly: false);
        }

        /// <summary>
        /// Returns the value as a specific type, according to <see cref="DataType"/>.
        /// </summary>
        object GetValue()
        {
            object Result = null;

            switch (DataType)
            {
                case SettingDataType.String:
                    Result  = Value == null ? "" : Value;
                    break;
                case SettingDataType.Integer:
                    Result  = Value == null ? 0 : Value;
                    break;
                case SettingDataType.Float:
                    Result  = Value == null ? 0D : Value;
                    break;
                case SettingDataType.Decimal:
                    Result  = Value == null ? 0M : Value;
                    break;
                case SettingDataType.Date:
                    Result = Value == null ? DateTime.MinValue.Date : Convert.ToDateTime(Value).Date;
                    break;
                case SettingDataType.DateTime:
                    Result  = Value == null ? DateTime.MinValue : Convert.ToDateTime(Value);
                    break;
                case SettingDataType.Boolean:
                    Result  = Value == null ? false : Convert.ToInt32(Value);
                    break;
                case SettingDataType.SingleSelect:
                    Result  = Value == null ? -1 : Convert.ToInt32(Value);
                    break;
                case SettingDataType.MultiSelect:
                    int[] Values = Value as int[];
                    Result = Value == null ? new int[0] : Values;
                    break;
            }

            return Result;
        }

        /// <summary>
        /// Loads an instance of this class from a specified <see cref="DataRow"/>
        /// </summary>
        void LoadFrom(DataRow Row, bool ValueOnly)
        {
            string[] Parts;

            if (!ValueOnly)
            {
                Id = Row.AsString("Id");
                DisplayOrder = Row.AsInteger("DisplayOrder");
                Enum.TryParse("Active", out SettingDataType vDataType);
                DataType = vDataType;
                TitleKey = Row.AsString("TitleKey");

                // select list items
                if (DataType == SettingDataType.SingleSelect || DataType == SettingDataType.MultiSelect)
                {
                    if (SelectList == null)
                        SelectList = new List<SettingSelectItem>();
                    else
                        SelectList.Clear();

                    string S = Row.AsString("SelectList");
                    Parts = S.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    string[] Line;
                    SettingSelectItem SelectItem;
                    foreach (string Item in Parts)
                    {
                        Line = S.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (Line.Length == 2 && Line.All(item => !string.IsNullOrWhiteSpace(item)))
                        {
                            SelectItem = new SettingSelectItem();
                            SelectItem.Id = Convert.ToInt32(Line[0]);
                            SelectItem.ValueKey = Line[1];
                            SelectList.Add(SelectItem);
                        }
                    }
                }
            }


            // value
            switch (DataType)
            {
                case SettingDataType.String:
                    Value = Row.AsString("VString");
                    break;
                case SettingDataType.Integer:
                    Value = Row.AsInteger("VInteger");
                    break;
                case SettingDataType.Float:
                    Value = Row.AsFloat("VDecimal");
                    break;
                case SettingDataType.Decimal:
                    Value = Row.AsDecimal("VDecimal");
                    break;
                case SettingDataType.Date:
                case SettingDataType.DateTime:
                    Value = Row.AsDateTime("VDateTime");
                    break;
                case SettingDataType.Boolean:
                    Value = Row.AsInteger("VInteger", 0) == 1;
                    break;
                case SettingDataType.SingleSelect:
                    Value = Row.AsInteger("VInteger");
                    break;
                case SettingDataType.MultiSelect:
                    Parts = Row.AsString("VString").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    List<int> ValueList = new List<int>();
                    foreach (string Id in Parts)
                        ValueList.Add(Convert.ToInt32(Id));
                    Value = ValueList.ToArray();
                    break;
            }

        }
        /// <summary>
        /// Creates and returns a dictionary suitable for saving this instance to database using Sql statements.
        /// </summary>
        Dictionary<string, object> SaveToDictionary()
        {
            StringBuilder SB;
            Dictionary<string, object> Result = new Dictionary<string, object>();

            Result["Id"] = Id;
            Result["DisplayOrder"] = DisplayOrder;
            Result["DataType"] = DataType.ToString();
            Result["TitleKey"] = TitleKey;

            switch (DataType)
            {
                case SettingDataType.String:
                    Result["VString"] = GetValue();
                    break;
                case SettingDataType.Integer:
                    Result["VInteger"] = GetValue();
                    break;
                case SettingDataType.Float:
                    Result["VDecimal"] = GetValue();
                    break;
                case SettingDataType.Decimal:
                    Result["VDecimal"] = GetValue();
                    break;
                case SettingDataType.Date:
                    Result["VDateTime"] = GetValue();
                    break;
                case SettingDataType.DateTime:
                    Result["VDateTime"] = GetValue();
                    break;
                case SettingDataType.Boolean:
                    Result["VDateTime"] = GetValue();
                    break;
                case SettingDataType.SingleSelect:
                    Result["VInteger"] = GetValue();
                    break;
                case SettingDataType.MultiSelect:
                    int[] Values = Value as int[];
                    if (Values != null)
                    {
                        SB = new StringBuilder();
                        foreach (int V in Values)
                        {
                            if (SB.Length > 0)
                                SB.Append(",");
                            SB.Append(V.ToString());
                        }

                        Result["VString"] = SB.ToString();
                    }
                    else
                    {
                        Result["VString"] = "";
                    }
 
                    break;
            }

            // select list items
            Result["SelectList"] = "";
            if ((DataType == SettingDataType.SingleSelect || DataType == SettingDataType.MultiSelect) && SelectList != null)
            {
                SB = new StringBuilder();

                foreach (SettingSelectItem Item in SelectList)
                {
                    if (SB.Length > 0)
                        SB.Append(";");
                    SB.Append($"{Item.Id}|{Item.ValueKey}");
                }

                Result["SelectList"] = SB.ToString();
            }


            return Result;
        }

        /* static */
        /// <summary>
        /// Returns a table definition for a database table appropriate for this class.
        /// </summary>
        static public string GetTableDef(string TableName = "AppSettings")
        {
 
            string SqlText = $@"
create table {TableName} (
     Id                      @NVARCHAR(96)          @NOT_NULL primary key
    ,DisplayOrder            integer default 0      @NOT_NULL      
    ,DataType                @NVARCHAR(16)          @NOT_NULL    
    ,TitleKey                @NVARCHAR(96)          @NOT_NULL    

    ,VString                @NVARCHAR(512)          @NULL
    ,VInteger               integer                 @NULL
    ,VDecimal               @DECIMAL                @NULL
    ,VDateTime              @DATE_TIME              @NULL
    
    ,SelectList             @NVARCHAR(4096)         @NULL
 )
";       
 
            return SqlText;
        }
        /// <summary>
        /// Deletes a setting from the database table, based on a specified Id.
        /// </summary>
        static public void Delete(SqlStore SqlStore, string Id, string TableName = "AppSettings")
        {
            string Prefix = SqlProvider.GlobalPrefix.ToString();
            string SqlText;

            SqlText = $@"delete from {TableName} where Id = {Prefix}Id ";
            SqlStore.ExecSql(SqlText, Id);
        }

        /* public */
        /// <summary>
        /// Loads an instance of this class from a specified <see cref="DataRow"/>
        /// </summary>
        public void Load(DataRow Row)
        {
            LoadFrom(Row, ValueOnly: false);
        }
        /// <summary>
        /// Loads the value only of this instance from the database table
        /// </summary>
        public void LoadValue(SqlStore SqlStore, string TableName = "AppSettings")
        {
            string Prefix = SqlProvider.GlobalPrefix.ToString();
            string SqlText;

            SqlText = $@"select * {TableName} where Id = {Prefix}Id ";
            DataRow Row = SqlStore.SelectResults(SqlText, Id);
            if (Row != null)
                LoadFrom(Row, ValueOnly: true);
        }
        /// <summary>
        /// Saves an instance of this class in the specified database table.
        /// <para>INSERTs or UPDATEs depending on the value of the <see cref="Id"/> property.</para>
        /// </summary>
        public void Save(SqlStore SqlStore, string TableName = "AppSettings")
        {
            string SqlText;

            string Prefix = SqlProvider.GlobalPrefix.ToString();

            SqlText = $"select Id from {TableName} where Id = '{Id}' ";
            DataTable Table = SqlStore.Select(SqlText);

            

            if (Table.Rows.Count == 0)
            {
                SqlText = $@"
insert int {TableName} (
     Id               
    ,DisplayOrder     
    ,DataType         
    ,TitleKey         
                      
    ,VString          
    ,VInteger         
    ,VDecimal         
    ,VDateTime        
                      
    ,SelectList       
) values (
     {Prefix}Id               
    ,{Prefix}DisplayOrder     
    ,{Prefix}DataType         
    ,{Prefix}TitleKey         
                      
    ,{Prefix}VString          
    ,{Prefix}VInteger         
    ,{Prefix}VDecimal         
    ,{Prefix}VDateTime        
                      
    ,{Prefix}SelectList   
)";
            }
            else
            {
                SqlText = $@"
update {TableName} set
     DisplayOrder = {Prefix}DisplayOrder   
    ,DataType     = {Prefix}DataType             
    ,TitleKey     = {Prefix}TitleKey      
                                   
    ,VString      = {Prefix}VString      
    ,VInteger     = {Prefix}VInteger     
    ,VDecimal     = {Prefix}VDecimal     
    ,VDateTime    = {Prefix}VDateTime    
                                   
    ,SelectList   = {Prefix}SelectList  
where
    Id = {Prefix}Id  
";

                Dictionary<string, object> ParamsDic = SaveToDictionary();
                SqlStore.ExecSql(SqlText, ParamsDic);
            }
        }
        /// <summary>
        /// Saves just the <see cref="Value"/> of a specified instance to the specified database table.
        /// <para>The <see cref="Id"/> of the instance must have a value.</para>
        /// </summary>
        public void SaveValue(SqlStore SqlStore, string TableName = "AppSettings")
        {
            string Prefix = SqlProvider.GlobalPrefix.ToString();
            string SqlText;

            SqlText = $@"
update {TableName} set
     VString      = {Prefix}VString      
    ,VInteger     = {Prefix}VInteger     
    ,VDecimal     = {Prefix}VDecimal     
    ,VDateTime    = {Prefix}VDateTime    
                                   
    ,SelectList   = {Prefix}SelectList  
where
    Id = {Prefix}Id  
";
            Dictionary<string, object> ParamsDic = SaveToDictionary();
            SqlStore.ExecSql(SqlText, ParamsDic);
        }


        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public string AsString()
        {
            return GetValue() as string;
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public int AsInteger()
        {
            return (int)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public double AsFloat()
        {
            return (double)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public decimal AsDecimal()
        {
            return (decimal)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public DateTime AsDate()
        {
            return (DateTime)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public DateTime AsDateTime()
        {
            return (DateTime)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public bool AsBoolean()
        {
            return (bool)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public int AsSingleSelect()
        {
            return (int)GetValue();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public int[] AsMultiSelect()
        {
            return (int[])GetValue();
        }

        /* properties */
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The display order of this instance
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// The data-type
        /// </summary>
        public SettingDataType DataType { get; set; }
        /// <summary>
        /// The title label or a resource string key
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// The setting value as it comes from the storage medium, i.e. database.
        /// <para>When the data-type is String, Integer, Float, Decimal, Date, DateTime and Boolean: Value contains the actual typed value.</para>
        /// <para>When the data-type is SingleSelect: Value contains the integer Id of the selected item, found in <see cref="SelectList"/>.</para>
        /// <para>When the data-type is MultiSelct: Value contains an integer array with the Ids of the selected items, found in <see cref="SelectList"/>.</para>
        /// </summary>
        public object Value { get; set; }
       
        /// <summary>
        /// A list of items to be used when this is a single or multi select setting.
        /// </summary>
        public List<SettingSelectItem> SelectList { get; set; } = new List<SettingSelectItem>();
    }




    /// <summary>
    /// To be used when this is a single or multi select <see cref="Setting"/> instances.
    /// </summary>
    public class SettingSelectItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingSelectItem()
        {
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A resource string key or a value for the item
        /// </summary>
        public string ValueKey { get; set; }
    }
}

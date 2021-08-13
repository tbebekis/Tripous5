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
        /* private */
        string fTitleKey;

        /// <summary>
        /// Returns the value as a specific type, according to <see cref="DataType"/>.
        /// <para>If <see cref="Value"/> is null, returns a default value.</para>
        /// </summary>
        object GetValueSafe()
        {
            object Result = null;

            switch (DataType)
            {
                case SettingDataType.String:
                    Result = Value == null ? "" : Value;
                    break;
                case SettingDataType.Integer:
                    Result = Value == null ? 0 : Value;
                    break;
                case SettingDataType.Float:
                    Result = Value == null ? 0D : Value;
                    break;
                case SettingDataType.Decimal:
                    Result = Value == null ? 0M : Value;
                    break;
                case SettingDataType.Date:
                    Result = Value == null ? DateTime.MinValue.Date : Convert.ToDateTime(Value).Date;
                    break;
                case SettingDataType.DateTime:
                    Result = Value == null ? DateTime.MinValue : Convert.ToDateTime(Value);
                    break;
                case SettingDataType.Boolean:
                    Result = Value == null ? false : Convert.ToInt32(Value);
                    break;
                case SettingDataType.SingleSelect:
                    Result = Value == null ? "-1" : Value;
                    break;
                case SettingDataType.MultiSelect:
                    string[] Values = Value as string[];
                    Result = Value == null ? new string[0] : Values;
                    break;
            }

            return Result;
        }
        Type GetValueType()
        {
            Type Result = typeof(string);

            switch (DataType)
            {
                case SettingDataType.String:
                    Result = typeof(string);
                    break;
                case SettingDataType.Integer:
                    Result = typeof(int);
                    break;
                case SettingDataType.Float:
                    Result = typeof(double);
                    break;
                case SettingDataType.Decimal:
                    Result = typeof(decimal);
                    break;
                case SettingDataType.Date:
                    Result = typeof(DateTime);
                    break;
                case SettingDataType.DateTime:
                    Result = typeof(DateTime);
                    break;
                case SettingDataType.Boolean:
                    Result = typeof(bool);
                    break;
                case SettingDataType.SingleSelect:
                    Result = typeof(string);
                    break;
                case SettingDataType.MultiSelect:
                    Result = typeof(string[]);
                    break;
            }

            return Result;
        }

        /* construction */
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
            this.Load(Row, ValueOnly: false);
        }


        /* public */
        /// <summary>
        /// Loads an instance of this class from a specified <see cref="DataRow"/>
        /// </summary>
        public void Load(DataRow Row, bool ValueOnly)
        {
            if (!ValueOnly)
            {
                Id = Row.AsString("Id");
                TitleKey = Row.AsString("TitleKey");
                DisplayOrder = Row.AsInteger("DisplayOrder");
                Enum.TryParse(Row.AsString("DataType"), out SettingDataType vDataType);
                DataType = vDataType;

                SelectList = JsonConvert.DeserializeObject<List<SettingSelectItem>>(Row.AsString("SelectList"));
                SortSelectList();
            }

            // value
            TextValue = Row.AsString("TextValue");
        }
        /// <summary>
        /// Creates and returns a dictionary suitable for saving this instance to database using Sql statements.
        /// </summary>
        public Dictionary<string, object> GetAsSqlParams()
        { 
            Dictionary<string, object> Result = new Dictionary<string, object>();

            Result["Id"] = Id;
            Result["TitleKey"] = TitleKey;
            Result["DisplayOrder"] = DisplayOrder;
            Result["DataType"] = DataType.ToString();
            Result["TextValue"] = TextValue;
            Result["SelectList"] = JsonConvert.SerializeObject(SelectList);

            return Result;
        }


        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return $"{Id} - {DataType}";
        }

 
        /// <summary>
        /// Loads the value only of this instance from the database table
        /// </summary>
        public void LoadValue(SqlStore SqlStore = null, string TableName = "AppSettings")
        {
            if (SqlStore == null)
                SqlStore = SqlStores.Default;

            string Prefix = SqlProvider.GlobalPrefix.ToString();
            string SqlText;

            SqlText = $@"select * from {TableName} where Id = {Prefix}Id ";
            DataRow Row = SqlStore.SelectResults(SqlText, Id);
            if (Row != null)
                Load(Row, ValueOnly: true);
        }
        /// <summary>
        /// Saves just the <see cref="Value"/> of a specified instance to the specified database table.
        /// <para>The <see cref="Id"/> of the instance must have a value.</para>
        /// </summary>
        public void SaveValue(SqlStore SqlStore = null, string TableName = "AppSettings")
        {
            if (SqlStore == null)
                SqlStore = SqlStores.Default;

            string Prefix = SqlProvider.GlobalPrefix.ToString();
            string SqlText;

            SqlText = $@"
update {TableName} set
     TextValue   = {Prefix}TextValue   
where
    Id = {Prefix}Id  
";
            Dictionary<string, object> ParamsDic = GetAsSqlParams();
            SqlStore.ExecSql(SqlText, ParamsDic);
        }

        /// <summary>
        /// Saves an instance of this class in the specified database table.
        /// <para>INSERTs or UPDATEs depending on the value of the <see cref="Id"/> property.</para>
        /// </summary>
        public void SaveToTable(SqlStore SqlStore = null, string TableName = "AppSettings")
        {
            if (SqlStore == null)
                SqlStore = SqlStores.Default;

            string SqlText;

            string Prefix = SqlProvider.GlobalPrefix.ToString();

            SqlText = $"select Id from {TableName} where Id = '{Id}' ";
            DataTable Table = SqlStore.Select(SqlText);

            

            if (Table.Rows.Count == 0)
            {
                SqlText = $@"
insert into {TableName} (
     Id                 
    ,TitleKey           
    ,DisplayOrder       
    ,DataType           
    ,TextValue          
                        
    ,SelectList         
) values (
     {Prefix}Id  
    ,{Prefix}TitleKey         
    ,{Prefix}DisplayOrder     
    ,{Prefix}DataType        
    ,{Prefix}TextValue          
            
    ,{Prefix}SelectList   
)";
            }
            else
            {
                SqlText = $@"
update {TableName} set
     DisplayOrder = {Prefix}DisplayOrder  
    ,TitleKey     = {Prefix}TitleKey    
    ,DataType     = {Prefix}DataType             
    ,TextValue    = {Prefix}TextValue      
              
    ,SelectList   = {Prefix}SelectList  
where
    Id = {Prefix}Id  
";
            }



            Dictionary<string, object> ParamsDic = GetAsSqlParams();
            SqlStore.ExecSql(SqlText, ParamsDic);
        }


        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public string AsString()
        {
            return GetValueSafe() as string;
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public int AsInteger()
        {
            return (int)GetValueSafe();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public double AsFloat()
        {
            return (double)GetValueSafe();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public decimal AsDecimal()
        {
            return (decimal)GetValueSafe();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public DateTime AsDate()
        {
            return (DateTime)GetValueSafe();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public DateTime AsDateTime()
        {
            return (DateTime)GetValueSafe();
        }
        /// <summary>
        /// Returns the value as a specific type.
        /// </summary>
        public bool AsBoolean()
        {
            return (bool)GetValueSafe();
        }
        /// <summary>
        /// Returns the string Id of the selected <see cref="SettingSelectItem"/> item.
        /// </summary>
        public string AsSingleSelect()
        {
            return (string)GetValueSafe();
        }
        /// <summary>
        /// Returns a string array with the Ids of the selected <see cref="SettingSelectItem"/> items.
        /// </summary>
        public string[] AsMultiSelect()
        {
            return (string[])GetValueSafe();
        }

        /// <summary>
        /// Returns the title (label) of this setting, localized.
        /// </summary>
        public string GetTitle()
        {
            string Result = Res.GS(TitleKey);
            return Result;
        }
        /// <summary>
        /// Sorts the <see cref="SelectList"/> based on <see cref="SettingSelectItem.DisplayOrder"/>.
        /// </summary>
        public void SortSelectList()
        {
            if (SelectList != null)
                SelectList.Sort((A, B) => { return A.DisplayOrder - B.DisplayOrder; });
        }

        /// <summary>
        /// Returns the value serialized as a string.
        /// </summary>
        public string GetValueAsJsonText()
        {
            object V = GetValueSafe();
            string JsonText = JsonConvert.SerializeObject(V);
            return JsonText;
        }
        /// <summary>
        /// Sets the value deserializing a specified json text.
        /// </summary>
        public void SetValueFromJsonText(string JsonText)
        {
            Value = JsonConvert.DeserializeObject(JsonText, GetValueType());
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
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Id; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// The setting value as it comes from the storage medium, i.e. database.
        /// <para>When the data-type is String, Integer, Float, Decimal, Date, DateTime and Boolean: Value contains the actual typed value.</para>
        /// <para>When the data-type is SingleSelect: Value contains the integer Id of the selected item, found in <see cref="SelectList"/>.</para>
        /// <para>When the data-type is MultiSelct: Value contains an integer array with the Ids of the selected items, found in <see cref="SelectList"/>.</para>
        /// </summary>
        [JsonIgnore]
        public object Value { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Value"/> as string, json text actually.
        /// <para>NOTE: Used in serialization only.</para>
        /// </summary>
        public string TextValue
        {
            get { return GetValueAsJsonText(); }
            set { SetValueFromJsonText(value); } 
        }
       
        /// <summary>
        /// A list of items to be used when this is a single or multi select setting.
        /// </summary>
        public List<SettingSelectItem> SelectList { get; set; } = new List<SettingSelectItem>();
    }

 
}

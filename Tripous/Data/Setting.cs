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

        /* static */
        /// <summary>
        /// Returns a table definition for a database table appropriate for this class.
        /// </summary>
        static public string GetTableDef(string TableName = "AppSettings")
        {
            DataTableDef Table = new DataTableDef();
            Table.Name = TableName;

            Table.AddPrimaryKey();
            Table.AddField("DisplayOrder", DataFieldType.Integer, false, "", "0");            
            Table.AddStringField("Name", 96, true);
            Table.AddStringField("DataType", 16, true);
            Table.AddStringField("TitleKey", 96, true);

            Table.AddStringField("StringValue", 96, false);
            Table.AddField("IntegerValue", DataFieldType.Integer, false);
            Table.AddField("FloatValue", DataFieldType.Float, false);
            Table.AddField("DecimalValue", DataFieldType.Decimal, false);
            Table.AddField("DateTimeValue", DataFieldType.DateTime, false);
            Table.AddStringField("SelectList", 1024 * 4, false);

            Table.AddStringField("Code", 40, true, null, "''");
            
            Table.AddStringField("CustomerId", 40, true);
            Table.AddField("Date", DataFieldType.DateTime, false);

            Table.AddUniqueConstraint("Name");

            string Result = Table.GetDefText();
            return Result;
        }
        /// <summary>
        /// Loads an instance of this class from a specified <see cref="DataRow"/>
        /// </summary>
        static public Setting Load(DataRow Row)
        {
            Setting Result = new Setting();
            Load(Result, Row);
            return Result;
        }
        /// <summary>
        /// Loads an instance of this class from a specified <see cref="DataRow"/>
        /// </summary>
        static public void Load(Setting Instance, DataRow Row)
        {
            string[] Parts;

            Instance.Id = Row.AsString("Id");
            Instance.DisplayOrder = Row.AsInteger("DisplayOrder");
            Instance.Name = Row.AsString("Name");
            Enum.TryParse("Active", out SettingDataType DataType);
            Instance.DataType = DataType;
            Instance.TitleKey = Row.AsString("TitleKey");

            // select list items
            if (DataType == SettingDataType.SingleSelect || DataType == SettingDataType.MultiSelect)
            {
                if (Instance.SelectList == null)
                    Instance.SelectList = new List<SettingSelectItem>();
                else
                    Instance.SelectList.Clear();

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
                        Instance.SelectList.Add(SelectItem);
                    }                    
                }
            }

            // value
            switch (DataType)
            {
                case SettingDataType.String:
                    Instance.Value = Row.AsString("StringValue");
                    break;
                case SettingDataType.Integer:
                    Instance.Value = Row.AsInteger("IntegerValue");
                    break;
                case SettingDataType.Float:
                    Instance.Value = Row.AsFloat("FloatValue");
                    break;
                case SettingDataType.Decimal:
                    Instance.Value = Row.AsDecimal("DecimalValue");
                    break;
                case SettingDataType.Date:
                case SettingDataType.DateTime:
                    Instance.Value = Row.AsDateTime("DateTimeValue");
                    break;
                case SettingDataType.Boolean:
                    Instance.Value = Row.AsInteger("IntegerValue", 0) == 1;
                    break;
                case SettingDataType.SingleSelect:
                    Instance.Value = Row.AsInteger("IntegerValue");
                    break;
                case SettingDataType.MultiSelect:
                    Parts = Row.AsString("StringValue").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    List<int> ValueList = new List<int>();
                    foreach (string Id in Parts)
                        ValueList.Add(Convert.ToInt32(Id));
                    Instance.Value = ValueList.ToArray();
                    break;
            }

        }

        /// <summary>
        /// Saves an instance of this class in the specified database table.
        /// <para>INSERTs or UPDATEs depending on the value of the <see cref="Id"/> property.</para>
        /// </summary>
        static public void Save(Setting Instance, string TableName = "AppSettings")
        {
#warning TODO: Setting.Save
        }
        /// <summary>
        /// Saves just the <see cref="Value"/> of a specified instance to the specified database table.
        /// <para>The <see cref="Id"/> of the instance must have a value.</para>
        /// </summary>
        static public void SaveValue(Setting Instance, string TableName = "AppSettings")
        {
#warning TODO: Setting.SaveValue
        }
        /// <summary>
        /// Deletes a setting from the database table, based on a specified Id.
        /// </summary>
        static public void Delete(string Id, string TableName = "AppSettings")
        {
#warning TODO: Setting.Delete
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
        /// The name. Must be unique among all settings
        /// </summary>
        public string Name { get; set; }
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

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
    /// Represents a list of settings
    /// </summary>
    public class Settings
    {
        const string DefaultTableName = "AppSettings";
        SqlStore fSqlStore;
        string fTableName;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Settings()
        {
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
    ,TitleKey                @NVARCHAR(96)          @NOT_NULL  
    ,DisplayOrder            integer default 0      @NOT_NULL      
    ,DataType                @NVARCHAR(24)          @NOT_NULL 
    ,TextValue               @NVARCHAR(4096)        @NOT_NULL 
   
    ,SelectList              @NBLOB_TEXT            @NULL
 )
";

            return SqlText;
        }


        /* public */
        /// <summary>
        /// Loads settings from a database table
        /// </summary>
        public void LoadFromTable()
        {
            Items.Clear();

            string SqlText = $@"select * from {TableName} ";
            DataTable Table = SqlStore.Select(SqlText);

            foreach (DataRow Row in Table.Rows)
                Items.Add(new Setting(Row));
        }
        /// <summary>
        /// Saves settings to a database table
        /// </summary>
        public void SaveToTable()
        {
            foreach (Setting Item in Items)
                Item.SaveToTable(SqlStore, TableName);
        }

        /// <summary>
        /// Loads settings from a file
        /// </summary>
        public void LoadFromFile(string FilePath)
        {
            Json.LoadFromFile(this, FilePath);
            foreach (Setting Item in Items)
                Item.SortSelectList();
        }
        /// <summary>
        /// Saves settings to a file.
        /// </summary>
        public void SaveToFile(string FilePath)
        {
            Json.SaveToFile(this, FilePath);
        }

        /// <summary>
        /// Deletes a setting from the database table, based on a specified Id.
        /// </summary>
        public void Delete(string Id)
        {
            string Prefix = SqlProvider.GlobalPrefix.ToString();
            string SqlText = $@"delete from {TableName} where Id = {Prefix}Id ";
            SqlStore.ExecSql(SqlText, Id);

            Setting Item = Items.FirstOrDefault(item => item.Id == Id);
            if (Item != null)
                Items.Remove(Item);
        }

        /* properties */
        /// <summary>
        /// Indexer. Returns a <see cref="Setting"/> item based on a specified Id.
        /// </summary>
        public Setting this[string SettingId] => Items.FirstOrDefault(item => item.Id == SettingId);
        /// <summary>
        /// The name of the database table to use in persisting the settings
        /// </summary>
        public string TableName
        {
            get { return !string.IsNullOrWhiteSpace(fTableName) ? fTableName : DefaultTableName; }
            set { fTableName = value; }
        }
        /// <summary>
        /// The <see cref="Tripous.Data.SqlStore"/> to use in reading/writing the settings.
        /// </summary>
        [JsonIgnore]
        public SqlStore SqlStore
        {
            get { return fSqlStore != null ? fSqlStore : SqlStores.Default; }
            set { fSqlStore = value; }
        }
        /// <summary>
        /// The list of settings
        /// </summary>
        public List<Setting> Items { get; set; } = new List<Setting>();
    }
}

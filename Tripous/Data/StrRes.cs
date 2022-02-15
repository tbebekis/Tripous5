using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Tripous.Data
{

    /// <summary>
    /// Helper class for the SYS_STR_RES system table
    /// </summary>
    static public class StrRes
    {
        /*

        create table {SysTables.StrRes} (
            Id                      {SysConfig.PrimaryKeyStr()} 
           ,LanguageCode            @NVARCHAR(40)        @NOT_NULL    
           ,TableName               @NVARCHAR(96)        @NULL           
           ,TableId                 {SysConfig.ForeignKeyStr()} @NULL           
           ,EntryKey                @NVARCHAR(96)        @NOT_NULL		 
           ,EntryValue              @NBLOB_TEXT          @NOT_NULL                         
         )

         */

        static SqlStore fStore;
        static SqlBroker fBroker;

        /// <summary>
        /// Returns the default store
        /// </summary>
        static SqlStore Store
        {
            get
            {
                if (fStore == null)
                    fStore = SqlStores.CreateDefaultSqlStore();

                return fStore;
            }
        }
        /// <summary>
        /// Returns the SYS_STR_RES Broker
        /// </summary>
        static SqlBroker Broker
        {
            get
            {
                if (fBroker == null)
                    fBroker = SqlBroker.CreateSingleTableBroker(SysTables.StrRes);

                return fBroker;
            }
        }


        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        static StrRes()
        { 
        }


        /// <summary>
        /// Returns a <see cref="DataTable"/> with two fields: EntryKey and EntryValue.
        /// <para>The result returns general resource strings (not translatable table data) of a specified language.</para>
        /// </summary>
        static public DataTable GetStringList(string LanguageCode)
        {
            string SqlText = $@"
select
    EntryKey     
   ,EntryValue  
from
    {SysTables.StrRes}
where
        LanguageCode = '{LanguageCode}'
    and (TableName is null or TableName = '')
";
            DataTable Result = Store.Select(SqlText);
            return Result;
        }
        /// <summary>
        /// Returns general resource strings (not translatable table data) of a specified language.
        /// </summary>
        static public Dictionary<string, string> GetStringListAsDictionary(string LanguageCode)
        {            
            Dictionary<string, string> Result = new Dictionary<string, string>();

            DataTable Table = GetStringList(LanguageCode);
            foreach (DataRow Row in Table.Rows)
                Result[Row.AsString("EntryKey")] = Row.AsString("EntryValue");

            return Result;
        }
        /// <summary>
        /// Returns a <see cref="DataTable"/> with two fields: EntryKey and EntryValue.
        /// <para>The result returns translatable table data of a specified Id (data-row) of a specified Table.</para>
        /// <para>For example strings of Products table of a row with Id = 1234.</para>
        /// <para>The EntryKey is the FieldName, e.g. ProductName where the EntryValue is the translated product name in a specified language.</para>
        /// </summary>
        static public DataTable GetTableIdStringList(string LanguageCode, string TableName, object TableId)
        {
            string sTableId = Sys.IdStr(TableId);

            string SqlText = $@"
select
    EntryKey     
   ,EntryValue  
from
    {SysTables.StrRes}
where
        LanguageCode = '{LanguageCode}'
    and TableName = '{TableName}'
    and TableId = {sTableId}
";
            DataTable Result = Store.Select(SqlText);
            return Result;
        }

        /// <summary>
        /// Saves (INSERTs or UPDATESs) an entry of a general resource string in the database table.
        /// </summary>
        static public void Save(string LanguageCode, string EntryKey, string EntryValue)
        {
            string SqlText = $@"
select
    Id
from
    {SysTables.StrRes}
where
        LanguageCode = '{LanguageCode}'
    and EntryKey = '{EntryKey}'
    and (TableName is null or TableName = '')
";

            object Id = Store.SelectResult(SqlText);
            bool IsInsert = Sys.IsNull(Id);

            if (IsInsert)
                Broker.Insert();
            else
                Broker.Edit(Id);

            Broker.Row["LanguageCode"] = LanguageCode;
            Broker.Row["EntryKey"] = EntryKey;
            Broker.Row["EntryValue"] = EntryValue;
            Broker.Commit();
        }
        /// <summary>
        /// Saves (INSERTs or UPDATESs) an entry of a translatable table data resource string in the database table.
        /// <para>For example TableName could be Products, TableId could be 12345, EntryKey could be ProductName (a field in Products table) and EntryValue a value in a specified language.</para>
        /// </summary>
        static public void Save(string LanguageCode, string TableName, object TableId, string EntryKey, string EntryValue)
        {
            string sTableId = Sys.IdStr(TableId);

            string SqlText = $@"
select
    Id
from
    {SysTables.StrRes}
where
        LanguageCode = '{LanguageCode}'
    and EntryKey = '{EntryKey}'
    and TableName = '{TableName}'
    and TableId = {sTableId}
";

            object Id = Store.SelectResult(SqlText);
            bool IsInsert = Sys.IsNull(Id);

            if (IsInsert)
                Broker.Insert();
            else
                Broker.Edit(Id);

            Broker.Row["LanguageCode"] = LanguageCode;
            Broker.Row["TableName"] = TableName;
            Broker.Row["TableId"] = TableId;
            Broker.Row["EntryKey"] = EntryKey;
            Broker.Row["EntryValue"] = EntryValue;
            Broker.Commit();
        }
    }
}

/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.IO;

namespace Tripous.Data
{


    /// <summary>
    /// Handles access to system data table.
    /// <para>System data table is a database table which stores information
    /// regarding system data such as reports, descriptors, resources etc.</para>
    /// <para>WARNING: There is unique contraint on DataType and DataName</para>
    /// </summary>
    public class SysData
    {

        static SqlStore Store;
        static TableSqls Sqls = new TableSqls();

        static string CompanyIdToSql(object oCompanyId)
        {
            if (SysConfig.GuidOids)
                return oCompanyId == null ? string.Empty : oCompanyId.ToString().QS();
            else
                return oCompanyId == null ? "-1" : oCompanyId.ToString();
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        static SysData()
        {
            Store = SqlStores.CreateDefaultSqlStore();
            Db.BuildSql(SysTables.Data, "Id", SysConfig.GuidOids, Store, Sqls, true);
        }


        /// <summary>
        /// Creates and returns a DataTable based on the schema of the system data table.
        /// <para>NOTE: It always returns a DataTable with ALL table columns.</para>
        /// </summary>
        static public DataTable CreateDataTable(/* bool NoBlobs */)
        {
            DataTable Result = new DataTable(SysTables.Data);
            Store.GetNativeSchema(string.Empty, SysTables.Data, string.Empty, Result);

            /*
                        if (NoBlobs)
                        {
                            string FieldName;
                            DataColumn Column;
                            for (int i = 1; i < 6; i++)
                            {
                                FieldName = $"Data{i}";
                                Column = Result.FindColumn(FieldName);
                                if (Column != null)
                                {
                                    Result.Columns.Remove(Column);
                                }

                            }
                        } 
             */

            return Result;
        }
 
 
        /* select with Sql construction */
        /// <summary>
        /// Returns a table with all DataType and DataName system data.
        /// <para>WARNING: Normally this should return a single row, 
        /// since there is a unique contraint regarding DataType and DataName.</para>
        /// </summary>
        static public void Select(string DataType, string DataName, DataTable Table, bool NoBlobs)
        {

            string SqlText = SysTables.GetSystemDataSelectStatement(NoBlobs); 

            StringBuilder SB = new StringBuilder();
            
            if (!string.IsNullOrWhiteSpace(DataType))
                SB.AppendLine($"    DataType = '{DataType}'");

            if (!string.IsNullOrWhiteSpace(DataName))
            {
                if (SB.Length == 0)
                    SB.AppendLine($"    DataName = '{DataName}'");
                else
                    SB.AppendLine($"    and DataName = '{DataName}'");
            }
                

            if (SB.Length > 0)
            {
                SqlText += $@"
where
{SB.ToString()}
";
            }
 

            Store.SelectTo(Table, SqlText);
        }
 
        /// <summary>
        /// Returns a table with all DataType system data.
        /// </summary>
        static public void Select(string DataType, DataTable Table, bool NoBlobs)
        { 
            Select(DataType, DataName: "", Table, NoBlobs); 
        }
        /// <summary>
        /// Returns a table with all system data.
        /// </summary>
        static public void Select(DataTable Table, bool NoBlobs)
        {
            Select(DataType: "", DataName: "", Table, NoBlobs); 
        }

        /// <summary>
        /// Returns a table with all DataType system data.
        /// <para>The passed DataType is used in the WHERE clause with a LIKE clause.</para>
        /// </summary>
        static public DataTable SelectLike(string DataType, bool NoBlobs)
        {
            if (DataType.IndexOf('%') == -1)
                DataType = $"%{DataType}%";

            string SqlText = SysTables.GetSystemDataSelectStatement(NoBlobs);

            SqlText += $@" 
where
    DataType like '{DataType}'
";

            return Store.Select(SqlText);
        }

        /* select, blob selection is controlled by the NoBlobs flag */
        /// <summary>
        /// Returns a single data row of system data, according to the specified DataType and DataName.
        /// <para>It may return null if nothing found.</para>
        /// </summary>
        static public DataRow Select(string DataType, string DataName, bool NoBlobs)
        {
            DataTable Table = CreateDataTable();
            Select(DataType, DataName, Table, NoBlobs);
            return Table.Rows.Count > 0 ? Table.Rows[0] : null;
        }
        /// <summary>
        /// Returns a table with all DataType system data.
        /// </summary>
        static public DataTable Select(string DataType, bool NoBlobs)
        {
            DataTable Result = CreateDataTable();
            Select(DataType, Result, NoBlobs);
            return Result;
        }
        /// <summary>
        /// Returns a table with all system data.
        /// </summary>
        static public DataTable Select(bool NoBlobs)
        {
            DataTable Result = CreateDataTable();
            Select(Result, NoBlobs);
            return Result;
        }

        /// <summary>
        /// Selects an item by a specified id and returns a <see cref="DataTable"/>
        /// </summary>
        static public DataTable SelectById(object Id)
        {
            string SqlText = $@"select * from {SysTables.Data} where Id = {Sys.IdStr(Id)}";
            return Store.Select(SqlText);
        }
 
        /* misc select */
        /// <summary>
        /// Selects the row specified by Id and loads Item.
        /// <para>Returns true if Id exists and a row is returned, else false.</para>
        /// </summary>
        static public bool Select(object Id, SysDataItem Item)
        { 
            string SqlText = $@"select * from {SysTables.Data} where Id = {Sys.IdStr(Id)}";
 
            DataRow Row = Store.SelectResults(SqlText);
            if (Row != null)
                Item.LoadFromRow(Row);

            return Row != null;
        }
        /// <summary>
        /// Selects a row specified by Id and then creates, loads and returns an item.
        /// </summary>
        static public SysDataItem SelectItemById(object Id)
        {
            SysDataItem Item = new SysDataItem();
            Select(Id, Item);
            return Item;
        }
        /// <summary>
        /// Loads Item by selecting Item.DataType and Item.DataName from the system data table.
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType and DataName.</para>
        /// </summary>
        static public object Select(SysDataItem Item)
        {
            return Select(Item.DataType, Item.DataName, Item);
        }
        /// <summary>
        /// Loads Item by selecting DataType and DataName from the system data table.
        /// <para>On success returns the Id of the row, else returns null.</para>
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType and DataName.</para>
        /// </summary>
        static public object Select(string DataType, string DataName, SysDataItem Item)
        {
            DataRow Row = Select(DataType, DataName, false);
            if (Row != null)
            {
                Item.LoadFromRow(Row);
                return Row["Id"];
            }

            Item.Clear();
            return null;
        }
        /// <summary>
        /// Selects and returns the Id of the system data table under the DataType and DataName, if any, else null.
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType and DataName.</para>
        /// </summary>
        static public object SelectId(string DataType, string DataName)
        {
            DataRow Row = Select(DataType, DataName, false);
            return Row != null ? Row["Id"] : null;
        }
        /// <summary>
        /// Returns true if a row exists in the system data table under the DataType and DataName.
        /// </summary>
        static public bool Exists(string DataType, string DataName)
        {
            return !Sys.IsNull(SelectId(DataType, DataName));
        }

        /// <summary>
        /// "Corrects" a data row by setting its Id to either -1 or an existing Id based
        /// on the DataType and DataName column values.
        /// <para>NOTE: Used by the <see cref="Save(DataRow)"/> method.</para>
        /// </summary>
        static public void Correct(DataRow Row)
        {
            if (Row != null)
            {
                string DataType = Sys.AsString(Row["DataType"], string.Empty);
                string DataName = Sys.AsString(Row["DataName"], string.Empty);
                Row["Id"] = SelectId(DataType, DataName);
            }
        }
        /// <summary>
        /// "Corrects" each DataRow of Table by setting its Id to either -1 or an existing Id based
        /// on the DataType and DataName column values. 
        /// <para>NOTE: Used by the <see cref="Save(DataTable)"/> method.</para>
        /// </summary>
        static public void Correct(DataTable Table)
        {
            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                    Correct(Row);
            }
        }

        /* save to database (INSERT and UPDATE only) */
        /// <summary>
        /// Saves a specified DataRow to the system data table.
        /// <para>NOTE: Returns the Id of the row in the database table</para>
        /// <para>WARNING: Performs INSERT and UPDATE only.</para>
        /// </summary>
        static public object Save(DataRow Row)
        {
            Correct(Row);
            Db.Commit(Row, SysTables.Data, "Id", SysConfig.GuidOids, Store, Sqls);
            return Row["Id"];
        }
        /// <summary>
        /// Saves a specified DataTable to the system data table.
        /// <para>NOTE: On return the Table.Rows contain the Ids of the rows.</para>
        /// <para>WARNING: Performs INSERT and UPDATE only.</para>
        /// </summary>
        static public void Save(DataTable Table)
        {
            Correct(Table);
            Db.Commit(Table, SysTables.Data, "Id", SysConfig.GuidOids, Store, Sqls);
        }
        /// <summary>
        /// Saves a specified <see cref="SysDataItem"/> to the system data table.
        /// <para>NOTE: Returns the Id of the row in the database table</para>
        /// <para>WARNING: Performs INSERT and UPDATE only.</para>
        /// </summary>
        static public object Save(SysDataItem Item)
        {
            DataTable Table = CreateDataTable();
            DataRow Row = Table.NewRow();
            Item.SaveToRow(Row);
            Table.Rows.Add(Row);
            return Save(Row);
        }

        /* delete */
        /// <summary>
        /// Deletes a record from the system data table
        /// </summary>
        static public void Delete(object Id)
        {
            string SqlText = $"delete from {SysTables.Data} where Id = {Sys.IdStr(Id)}";
            Store.ExecSql(SqlText);
        }
        /// <summary>
        /// Deletes a record, under the DataType and DataName, from the system data table.
        /// <para>WARNING: Normally there is just a single row, since there is a unique contraint
        /// regarding DataType and DataName</para>
        /// </summary>
        static public void Delete(string DataType, string DataName)
        {
            string SqlText = $@"
delete from {SysTables.Data} 
where 
        DataType = '{DataType}' 
    and DataName = '{DataName}'
";

            Store.ExecSql(SqlText);
        }
        /// <summary>
        /// Deletes all records from the system data table under the DataType.
        /// </summary>
        static public void Delete(string DataType)
        {
            string SqlText = $@"
delete from {SysTables.Data} 
where 
    DataType = '{DataType}' 
";

            Store.ExecSql(SqlText);
        }
       

        /* load/save to/from DataRow */
        /// <summary>
        /// Loads Source DataRow to the Dest SysDataItem
        /// </summary>
        static public void LoadTo(DataRow Source, SysDataItem Dest)
        {
            Dest.LoadFromRow(Source);
        }
        /// <summary>
        /// Saves Source SysDataTime to the Dest DataRow
        /// </summary>
        static public void LoadTo(SysDataItem Source, DataRow Dest)
        {
            Source.SaveToRow(Dest);
        }

        /* blob loaders */
        /// <summary>
        /// Selects and loads the FielName blof field to Dest
        /// </summary>
        static public void LoadBlobTo(Stream Dest, string FieldName, string DataType, string DataName)
        {
            DataRow Row = Select(DataType, DataName, false);
            if (Row != null)
            {
                Row.SaveToStream(FieldName, Dest);
            }
        }
        /// <summary>
        /// Selects and loads the FielName blof field to Dest
        /// </summary>
        static public void LoadBlobTo(DataRow Dest, string FieldName, string DataType, string DataName)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                LoadBlobTo(MS, FieldName, DataType, DataName);
                Dest.LoadFromStream(FieldName, MS);
            }
        }


        /* helpers */
        /// <summary>
        /// Gets the DataColumn of FieldName from Table.
        /// </summary>
        static public DataColumn GetColumn(DataTable Table, string FieldName)
        {
            if ((FieldName.Length == 1) && ("123456".IndexOf(FieldName[0]) != -1))
                FieldName = "Data" + FieldName;

            if (Table.Columns.Contains(FieldName))
                return Table.Columns[FieldName];

            return null;
        }
        /// <summary>
        /// Returns the value of the FieldName from Row
        /// </summary>
        static public object GetValue(DataRow Row, string FieldName)
        {
            DataColumn Column = GetColumn(Row.Table, FieldName);
            if (Column != null)
            {
                if (!Sys.IsNull(Row[FieldName]))
                    return Row[FieldName];
                else if (Simple.SimpleTypeOf(Column.DataType) == SimpleType.String)
                    return string.Empty;
                else if (Simple.SimpleTypeOf(Column.DataType) == SimpleType.Integer)
                    return -1;
            }
            return null;
        }
        /// <summary>
        /// Sets the Value as the value of the FieldName column of the Row.
        /// </summary>
        static public void SetValue(DataRow Row, string FieldName, object Value)
        {
            DataColumn Column = GetColumn(Row.Table, FieldName);
            if (Column != null)
                Row[FieldName] = Value;
        }

        /* import-export */
        /// <summary>
        /// Imports from FilePath any row with DataType and DataName.
        /// <para>WARNING: The FilePath file must be created with the DataTable.WriteXml method.</para>
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlImportData(string FilePath, string DataType, string DataName)
        {
            if (File.Exists(FilePath))
            {
                DataTable Source = new DataTable();
                Source.ReadXml(FilePath);
                DataTable DestTable = Source.Clone();
                DataRow DestRow;

                foreach (DataRow SourceRow in Source.Rows)
                {
                    if (Sys.IsSameText(DataType, Sys.AsString(SourceRow["DataType"], string.Empty))
                        && Sys.IsSameText(DataName, Sys.AsString(SourceRow["DataName"], string.Empty)))
                    {
                        DestRow = DestTable.NewRow();
                        SourceRow.CopyTo(DestRow);
                        DestTable.Rows.Add(DestRow);
                    }
                }

                TableXmlImportData(DestTable);
            }
        }
        /// <summary>
        /// Imports from FilePath any row with DataType.
        /// <para>WARNING: The FilePath file must be created with the DataTable.WriteXml method.</para>
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlImportData(string FilePath, string DataType)
        {
            if (File.Exists(FilePath))
            {
                DataTable Source = new DataTable();
                Source.ReadXml(FilePath);
                DataTable DestTable = Source.Clone();
                DataRow DestRow;

                foreach (DataRow SourceRow in Source.Rows)
                {
                    if (Sys.IsSameText(DataType, Sys.AsString(SourceRow["DataType"], string.Empty)))
                    {
                        DestRow = DestTable.NewRow();
                        SourceRow.CopyTo(DestRow);
                        DestTable.Rows.Add(DestRow);
                    }
                }

                TableXmlImportData(DestTable);
            }
        }
        /// <summary>
        /// Imports all rows from FilePath  
        /// <para>WARNING: The FilePath file must be created with the DataTable.WriteXml method.</para>
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlImportData(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                DataTable Table = new DataTable();
                Table.ReadXml(FilePath);
                TableXmlImportData(Table);
            }
        }
        /// <summary>
        /// Imports Table.
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlImportData(DataTable Table)
        {
            Save(Table);
        }

        /// <summary>
        /// Exports DataType and DataName rows from system data table to FilePath.
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlExportData(string FilePath, string DataType, string DataName)
        {
            DataRow Row = Select(DataType, DataName, false);
            TableXmlExportData(FilePath, Row.Table);
        }
        /// <summary>
        /// Exports DataType rows from system data table to FilePath.
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlExportData(string FilePath, string DataType)
        {
            TableXmlExportData(FilePath, Select(DataType, false));
        }
        /// <summary>
        /// Exports all rows from system data table to FilePath.
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlExportData(string FilePath)
        {
            TableXmlExportData(FilePath, Select(false));
        }
        /// <summary>
        /// Exports Table to FilePath
        /// <para>NOTE: Import and Export methods use the <see cref="DataTable"/> ReadXml() and WriteXml() methods. </para>
        /// </summary>
        static public void TableXmlExportData(string FilePath, DataTable Table)
        {
            if (Table != null)
            {
                Sys.EnsureDirectories(FilePath);
                Table.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
        }


    }

}

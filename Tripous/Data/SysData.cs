﻿/*--------------------------------------------------------------------------------------        
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
    /// <para>WARNING: All methods of the SystemData class act on the current company.</para>
    /// <para>WARNING: There is unique contraint on DataType, DataName and Company.</para>
    /// </summary>
    public class SysData
    {

        static SqlStore fStore;
        static SqlStatements fSqlStatements = new SqlStatements();

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        static SysData()
        {
            fStore = SqlStores.CreateDefaultSqlStore();
            Db.BuildSql(SysTables.Data, "Id", false, fStore, fSqlStatements, true);
        }


        /// <summary>
        /// Creates and returns a DataTable based on the schema of the system data table.
        /// </summary>
        static public DataTable CreateDataTable()
        {
            DataTable Result = new DataTable(SysTables.Data);
            fStore.GetNativeSchema(string.Empty, SysTables.Data, string.Empty, Result);
            return Result;
        }
        /// <summary>
        /// Returns the field list of the SysData table without the blob fields.
        /// </summary>
        static public string GetSelectListNoBlobs()
        {
            string Result = @"
             Id            
            ,{0}

            ,DataName
            ,DataType 
            ,Title   
            ,ConnectionName     
            ,Notes         

            ,Category1     
            ,Category2     
            ,Category3     
            ,Category4      
";
            Result = string.Format(Result, SysConfig.CompanyFieldName);

            return Result;
        }


        /* select, blob selection is controlled by the NoBlobs flag */
        /// <summary>
        /// Returns a single data row of system data, according to the specified DataType and DataName of the current Company.
        /// <para>It may return null if nothing found.</para>
        /// </summary>
        static public DataRow Select(string DataType, string DataName, bool NoBlobs)
        {
            DataTable Table = CreateDataTable();
            Select(DataType, DataName, Table, NoBlobs);
            return Table.Rows.Count > 0 ? Table.Rows[0] : null;
        }
        /// <summary>
        /// Returns a table with all DataType system data of the current company.
        /// </summary>
        static public DataTable Select(string DataType, bool NoBlobs)
        {
            DataTable Result = CreateDataTable();
            Select(DataType, Result, NoBlobs);
            return Result;
        }
        /// <summary>
        /// Returns a table with all system data of the current company.
        /// </summary>
        static public DataTable Select(bool NoBlobs)
        {
            DataTable Result = CreateDataTable();
            Select(Result, NoBlobs);
            return Result;
        }

        /* select with Sql construction */
        /// <summary>
        /// Returns a table with all DataType and DataName system data of the current company.
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType, DataName and Company</para>
        /// </summary>
        static public void Select(string DataType, string DataName, DataTable Table, bool NoBlobs)
        {
            string SelectList = NoBlobs ? GetSelectListNoBlobs() : " * ";

            string SqlText = string.Format("select {0} from {1} where {2} = {3} and DataType = {4} and DataName = {5}",
                SelectList, SysTables.Data, SysConfig.CompanyFieldName, SysConfig.CompanyIdSql, DataType.QS(), DataName.QS());

            SqlSelect(SqlText, Table);
        }
        /// <summary>
        /// Returns a table with all DataType system data of the current company.
        /// </summary>
        static public void Select(string DataType, DataTable Table, bool NoBlobs)
        {
            string SelectList = NoBlobs ? GetSelectListNoBlobs() : " * ";

            string SqlText = string.Format("select {0} from {1} where {2} = {3} and DataType = {4}",
                SelectList, SysTables.Data, SysConfig.CompanyFieldName, SysConfig.CompanyIdSql, DataType.QS());

            SqlSelect(SqlText, Table);
        }
        /// <summary>
        /// Returns a table with all system data of the current company.
        /// </summary>
        static public void Select(DataTable Table, bool NoBlobs)
        {
            string SelectList = NoBlobs ? GetSelectListNoBlobs() : " * ";

            string SqlText = string.Format("select {0} from {1} where {2} = {3}",
                SelectList, SysTables.Data, SysConfig.CompanyFieldName, SysConfig.CompanyIdSql);

            SqlSelect(SqlText, Table);
        }
        /// <summary>
        /// Returns a table with all DataType system data of the current company.
        /// <para>The passed DataType is used in the WHERE clause with a LIKE statement.</para>
        /// </summary>
        static public DataTable SelectLike(string DataType, bool NoBlobs)
        {
            string SelectList = NoBlobs ? GetSelectListNoBlobs() : " * ";

            string SqlText = string.Format("select {0} from {1} where {2} = {3} and DataType like {4}",
               SelectList, SysTables.Data, SysConfig.CompanyFieldName, SysConfig.CompanyIdSql, (DataType + "%").QS());

            return SqlSelect(SqlText);
        }

        /* Sql select */
        /// <summary>
        /// Selects the SqlText and returns a table.
        /// </summary>
        static public DataTable SqlSelect(string SqlText)
        {
            DataTable Result = CreateDataTable();
            SqlSelect(SqlText, Result);
            return Result;
        }
        /// <summary>
        /// Selects the SqlText and returns a table.
        /// </summary>
        static public void SqlSelect(string SqlText, DataTable Table)
        {
            fStore.SelectTo(Table, SqlText);
        }

        /* misc select */
        /// <summary>
        /// Selects the row specified by Id and loads Item.
        /// <para>Returns true if Id exists and a row is returned, else false.</para>
        /// </summary>
        static public bool Select(object Id, SysDataItem Item)
        {
            bool Result = false;
            string SqlText = string.Format(@"select * from {0} where Id = {1}", SysTables.Data, Sys.IdStr(Id));
            DataRow Row = fStore.SelectResults(SqlText);
            if (Row != null)
            {
                Item.LoadFromRow(Row);
                Result = true;
            }

            return Result;
        }
        /// <summary>
        /// Loads Item by selecting Item.DataType and Item.DataName from the system data table.
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType, DataName and Company</para>
        /// </summary>
        static public object Select(SysDataItem Item)
        {
            return Select(Item.DataType, Item.DataName, Item);
        }
        /// <summary>
        /// Loads Item by selecting DataType and DataName from the system data table.
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType, DataName and Company</para>
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
            return DBNull.Value;
        }
        /// <summary>
        /// Selects and returns the Id of the system data table under the DataType and DataName.
        /// <para>WARNING: Normally this returns a single row, since there is a unique contraint
        /// regarding DataType, DataName and Company</para>
        /// </summary>
        static public object SelectId(string DataType, string DataName)
        {
            DataRow Row = Select(DataType, DataName, false);
            if (Row != null)
                return Row["Id"];
            return null;
        }
        /// <summary>
        /// Returns true if a row exists in the system data table under the DataType and DataName.
        /// </summary>
        static public bool Exists(string DataType, string DataName)
        {
            return !Sys.IsNull(SelectId(DataType, DataName));
        }

        /// <summary>
        /// "Corrects" Row by setting its Id to either -1 or an existing Id based
        /// on the DataType and DataName column values. It also sets the current company Id.
        /// </summary>
        static public void Correct(DataRow Row)
        {
            if (Row != null)
            {
                string DataType = Sys.AsString(Row["DataType"], string.Empty);
                string DataName = Sys.AsString(Row["DataName"], string.Empty);
                Row["Id"] = SelectId(DataType, DataName);
                Row[SysConfig.CompanyFieldName] = SysConfig.CompanyId;
            }
        }
        /// <summary>
        /// "Corrects" each DataRow of Table by setting its Id to either -1 or an existing Id based
        /// on the DataType and DataName column values. It also sets the current company Id.
        /// </summary>
        static public void Correct(DataTable Table)
        {
            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                    Correct(Row);
            }
        }

        /* commit (INSERT and UPDATE only) */
        /// <summary>
        /// Commits Row to the system data table.
        /// <para>NOTE: Returns the Id of the row in the database table</para>
        /// <para>WARNING: Performs INSERT and UPDATE only.</para>
        /// </summary>
        static public object Commit(DataRow Row)
        {
            Correct(Row);
            Db.Commit(Row, SysTables.Data, "Id", SysConfig.GuidOids, fStore, fSqlStatements);
            return Row["Id"];
        }
        /// <summary>
        /// Commits Table to the system data table.
        /// <para>NOTE: On return the Table.Rows contain the Ids of the rows.</para>
        /// <para>WARNING: Performs INSERT and UPDATE only.</para>
        /// </summary>
        static public void Commit(DataTable Table)
        {
            Correct(Table);
            Db.Commit(Table, SysTables.Data, "Id", SysConfig.GuidOids, fStore, fSqlStatements);
        }
        /// <summary>
        /// Commits Item to the system data table.
        /// <para>NOTE: Returns the Id of the row in the database table</para>
        /// <para>WARNING: Performs INSERT and UPDATE only.</para>
        /// </summary>
        static public object Commit(SysDataItem Item)
        {
            DataTable Table = CreateDataTable();
            DataRow Row = Table.NewRow();
            Item.SaveToRow(Row);
            Table.Rows.Add(Row);
            return Commit(Row);
        }

        /* delete */
        /// <summary>
        /// Deletes a record from the system data table
        /// </summary>
        static public void Delete(object Id)
        {
            string SqlText = string.Format("delete from {0} where Id = {1}", SysTables.Data, Sys.IdStr(Id));
            fStore.ExecSql(SqlText);
        }
        /// <summary>
        /// Deletes a record, under the DataType and DataName, from the system data table.
        /// <para>WARNING: Normally there is just a single row, since there is a unique contraint
        /// regarding DataType, DataName and Company</para>
        /// </summary>
        static public void Delete(string DataType, string DataName)
        {
            string SqlText = string.Format("delete from {0} where {1} = {2} and DataType = {3} and DataName = {4}",
                SysTables.Data, SysConfig.CompanyFieldName, SysConfig.CompanyIdSql, DataType.QS(), DataName.QS());
            fStore.ExecSql(SqlText);
        }
        /// <summary>
        /// Deletes all records from the system data table under the DataType and the current company.
        /// </summary>
        static public void Delete(string DataType)
        {
            string SqlText = string.Format("delete from {0} where {1} = {2} and DataType = {3}",
                SysTables.Data, SysConfig.CompanyFieldName, SysConfig.CompanyIdSql, DataType.QS());
            fStore.ExecSql(SqlText);
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
        /// </summary>
        static public void ImportData(string FilePath, string DataType, string DataName)
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

                ImportData(DestTable);
            }
        }
        /// <summary>
        /// Imports from FilePath any row with DataType.
        /// <para>WARNING: The FilePath file must be created with the DataTable.WriteXml method.</para>
        /// </summary>
        static public void ImportData(string FilePath, string DataType)
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

                ImportData(DestTable);
            }
        }
        /// <summary>
        /// Imports all rows from FilePath  
        /// <para>WARNING: The FilePath file must be created with the DataTable.WriteXml method.</para>
        /// </summary>
        static public void ImportData(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                DataTable Table = new DataTable();
                Table.ReadXml(FilePath);
                ImportData(Table);
            }
        }
        /// <summary>
        /// Imports Table.
        /// </summary>
        static public void ImportData(DataTable Table)
        {
            Commit(Table);
        }

        /// <summary>
        /// Exports DataType and DataName rows of the current company from system data table to FilePath.
        /// </summary>
        static public void ExportData(string FilePath, string DataType, string DataName)
        {
            DataRow Row = Select(DataType, DataName, false);
            ExportData(FilePath, Row.Table);
        }
        /// <summary>
        /// Exports DataType rows of the current company from system data table to FilePath.
        /// </summary>
        static public void ExportData(string FilePath, string DataType)
        {
            ExportData(FilePath, Select(DataType, false));
        }
        /// <summary>
        /// Exports all rows of the current company from system data table to FilePath.
        /// </summary>
        static public void ExportData(string FilePath)
        {
            ExportData(FilePath, Select(false));
        }
        /// <summary>
        /// Exports Table to FilePath
        /// </summary>
        static public void ExportData(string FilePath, DataTable Table)
        {
            if (Table != null)
            {
                Sys.EnsureDirectories(FilePath);
                Table.WriteXml(FilePath, XmlWriteMode.WriteSchema);
            }
        }


    }

}

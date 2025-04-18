﻿namespace WebDesk.Controllers
{

#warning TRIPOUS WARNING: Remove the AllowAnonymous attribute from the AjaxController in production

    /// <summary>
    /// Ajax controller.
    /// <para>NOTE: All AJAX calls should be handled by this controller.</para>
    /// </summary>
    [AllowAnonymous] // <<<<<< This is here just for the demos to work. Remove it in production.
    public class AjaxController : ControllerMvc, IViewToStringConverter
    {
        string GetExceptionText(Exception e)
        {
            //string Result = WApp.DevMode ? Sys.ExceptionText(e) : e.Message;
            string Result = e.Message;
            return Result;
        }
        DataTable SelectTable(string SqlText, string ConnectionName)
        {
            SqlStore Store = SqlStores.CreateSqlStore(ConnectionName);

            DataTable Table = Store.Select(SqlText);
            return Table;
        }

        // non-actions -------------------------------------------------------------------------------

        #region IAjaxController
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IViewToStringConverter.ViewToString(string ViewName, object Model, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, Model, PlusViewData);
        }
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IViewToStringConverter.ViewToString(string ViewName, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, PlusViewData);
        }
        #endregion

        // actions -------------------------------------------------------------------------------

        [HttpPost("/Ajax/Execute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                Result = DataStore.AjaxExecute(this, R);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
        }

        #region Translations
        [HttpGet("/App/TranslationList")]
        public async Task<JsonResult> AppGetTranslationList(string CultureCode)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                Dictionary<string, string> StringList = DataStore.GetLanguageResourceStringList(CultureCode);

                Result.SerializePacket(StringList);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
 
        }
        #endregion

        #region Sql
        [HttpPost("/Sql/Select")]
        public async Task<JsonResult> SqlSelect([FromBody] SqlTextItem SqlTextItem)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                DataTable Table = SelectTable(SqlTextItem.SqlText, SqlTextItem.ConnectionName);
                JsonDataTable JTable = new JsonDataTable(Table);

                Result.SerializePacket(JTable);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
        }
        [HttpPost("/Sql/SelectAll")]
        public async Task<JsonResult> SqlSelectAll([FromBody] JsonDataTable JTable)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                string Name, SqlText, ConnectionName;
                JsonDataSet JDataSet = new JsonDataSet();

                DataTable Table;
                DataTable tblSqlList = JTable.ToTable();
                foreach (DataRow Row in tblSqlList.Rows)
                {
                    Name = Row.AsString("Name");
                    ConnectionName = Row.AsString("ConnectionName");
                    SqlText = Row.AsString("SqlText");

                    Table = SelectTable(SqlText, ConnectionName);
                    Table.TableName = Name;
                    JTable = new JsonDataTable(Table, null);
                    JDataSet.Tables.Add(JTable);
                }

                Result.SerializePacket(JDataSet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }



            return Json(Result);
        }
        #endregion

        #region Broker
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Initialize")]
        public JsonResult BrokerInitialize(string BrokerName)
        {
            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonBroker Packet = broker.JsonInitialize();
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
        }

        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Insert")]
        public JsonResult BrokerInsert(string BrokerName)
        {
            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonBroker Packet = broker.JsonInsert();
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return this.Json(Result);
        }
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Edit")]
        public JsonResult BrokerEdit(string BrokerName, string Id)
        {
            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonBroker Packet = broker.JsonEdit(Id);
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return this.Json(Result);
        }
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Delete")]
        public JsonResult BrokerDelete(string BrokerName, string Id)
        {
            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                broker.JsonDelete(Id);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return this.Json(Result);
        }
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Commit")]
        public JsonResult BrokerCommit([FromBody] JsonBroker Packet)
        {
            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(Packet.Name, true, false);
                Packet = broker.JsonCommit(Packet);
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return this.Json(Result);
        }

        /// <summary>
        /// Action. The SqlText could be a SELECT statement or a SelectSql Name from
        /// the SelectList of the broker.
        /// </summary>
        [Route("/Broker/SelectList")]
        public JsonResult BrokerSelectList(string BrokerName, string SqlText, bool UseRowLimit)
        {
            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonDataTable Packet = broker.JsonSelectList(SqlText, UseRowLimit ? -1 : 0);
                Packet.Name = "List";
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return this.Json(Result);
        }
        #endregion

        #region Locator
        [HttpGet("/Locator/GetDef")]
        public async Task<JsonResult> LocatorGetDef(string LocatorName)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();

            LocatorDef Def = LocatorDef.FindDef(LocatorName);
            if (Def == null)
            {
                Result.ErrorText = $"Locator Definition not found: {LocatorName}";
            }
            else
            {
                Result.SerializePacket(Def);
                Result.IsSuccess = true;
            }

            return Json(Result);
        }
        [HttpGet("/Locator/SqlSelect")]
        public async Task<JsonResult> LocatorSqlSelect(string LocatorName, string WhereSql)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();

            LocatorDef Def = LocatorDef.FindDef(LocatorName);
            if (Def == null)
            {
                Result.ErrorText = $"Locator Definition not found: {LocatorName}";
            } 
            else
            {
                string SqlText = Def.SqlText;
                if (!string.IsNullOrWhiteSpace(WhereSql))
                    SqlText = $@"
{SqlText}
where
   {WhereSql}
";
                DataTable Table = SelectTable(SqlText, Def.ConnectionName);
                JsonDataTable JTable = new JsonDataTable(Table);

                Result.SerializePacket(JTable);
                Result.IsSuccess = true;
            }

            return Json(Result);
        }
        #endregion

        #region SysData
        [HttpGet("/SysData/SelectList")]
        public async Task<JsonResult> SysDataSelectList(string DataType, bool NoBlobs)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult(); 
            try
            {
                DataTable Table = DataStore.SysDataSelectList(DataType, NoBlobs);
                JsonDataTable JTable = new JsonDataTable(Table);
                Result.SerializePacket(JTable);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
        }
        [HttpGet("/SysData/SelectItemById")]
        public async Task<JsonResult> SysDataSelectItemById(string Id)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                SysDataItem Item = DataStore.SysDataSelectItemById(Id);
                Result.SerializePacket(Item);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
        }
        [HttpPost("/SysData/SaveItem")]
        public async Task<JsonResult> SysDataSaveItem([FromBody] SysDataItem Item)
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                DataStore.SysDataSaveItem(Item);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Json(Result);
        }

        #endregion

        #region Desk
        [HttpGet("/Desk/GetMainMenu")]
        public async Task<JsonResult> DeskGetMainMenu()
        {
            await Task.CompletedTask;

            HttpPacketResult Result = new HttpPacketResult();

            try
            {
                Command[] MenuItems = DataStore.GetMainMenu();
                Result.SerializePacket(MenuItems);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }
 
            return Json(Result);
        }
        #endregion
 
 
    }
}

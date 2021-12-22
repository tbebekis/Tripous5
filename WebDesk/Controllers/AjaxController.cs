
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous;
using WebLib;
using WebLib.Models;
using WebLib.AspNet;
 
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tripous.Data;

namespace WebDesk.Controllers
{

#warning WARNING: Remove the AllowAnonymous attribute from the AjaxController in production

    /// <summary>
    /// Ajax controller.
    /// <para>NOTE: All AJAX calls should be handled by this controller.</para>
    /// </summary>
    [AllowAnonymous] // <<<<<< This is here just for the demos to work. Remove it in production.
    public class AjaxController : ControllerMvc, IAjaxController
    {
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
        string IAjaxController.ViewToString(string ViewName, object Model, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, Model, PlusViewData);
        }
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IAjaxController.ViewToString(string ViewName, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, PlusViewData);
        }
        #endregion

        // actions -------------------------------------------------------------------------------

        [HttpPost("/Ajax/Execute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;
            HttpActionResult Result = DataStore.AjaxExecute(this, R);
            return Json(Result);
        }
       
        #region Sql
        [HttpPost("/Sql/Select")]
        public async Task<JsonResult> SqlSelect([FromBody] SqlTextItem SqlTextItem)
        {
            await Task.CompletedTask;

            HttpActionResult Result = new HttpActionResult();
            try
            {
                DataTable Table = SelectTable(SqlTextItem.SqlText, SqlTextItem.ConnectionName);
                JsonDataTable JTable = new JsonDataTable(Table);

                Result.SerializePacket(JTable);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return Json(Result);
        }
        [HttpPost("/Sql/SelectAll")]
        public async Task<JsonResult> SqlSelectAll([FromBody] JsonDataTable JTable)
        {
            await Task.CompletedTask;

            HttpActionResult Result = new HttpActionResult();
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
                Result.ErrorText = Sys.ExceptionText(e);
            }



            return Json(Result);
        }
        #endregion

        #region Broker
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Initialize")]
        public JsonResult Initialize(string BrokerName)
        {
            HttpActionResult Result = new HttpActionResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonBroker Packet = broker.JsonInitialize();
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return Json(Result);
        }

        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Insert")]
        public JsonResult Insert(string BrokerName)
        {
            HttpActionResult Result = new HttpActionResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonBroker Packet = broker.JsonInsert();
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return this.Json(Result);
        }
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Edit")]
        public JsonResult Edit(string BrokerName, string Id)
        {
            HttpActionResult Result = new HttpActionResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                JsonBroker Packet = broker.JsonEdit(Id);
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return this.Json(Result);
        }
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Delete")]
        public JsonResult Delete(string BrokerName, string Id)
        {
            HttpActionResult Result = new HttpActionResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(BrokerName, true, false);
                broker.JsonDelete(Id);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return this.Json(Result);
        }
        /// <summary>
        /// Action
        /// </summary>
        [Route("/Broker/Commit")]
        public JsonResult Commit([FromBody] JsonBroker Packet)
        {
            HttpActionResult Result = new HttpActionResult();
            try
            {
                SqlBroker broker = SqlBroker.Create(Packet.Name, true, false);
                Packet = broker.JsonCommit(Packet);
                Result.SerializePacket(Packet);
                Result.IsSuccess = true;
            }
            catch (Exception e)
            {
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return this.Json(Result);
        }

        /// <summary>
        /// Action. The SqlText could be a SELECT statement or a SelectSql Name from
        /// the SelectList of the broker.
        /// </summary>
        [Route("/Broker/SelectList")]
        public JsonResult SelectList(string BrokerName, string SqlText, bool UseRowLimit)
        {
            HttpActionResult Result = new HttpActionResult();
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
                Result.ErrorText = Sys.ExceptionText(e);
            }

            return this.Json(Result);
        }
        #endregion

        #region Locator
        [HttpGet("/Locator/GetDef")]
        public async Task<JsonResult> LocatorGetDef(string LocatorName)
        {
            await Task.CompletedTask;

            HttpActionResult Result = new HttpActionResult();

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

            HttpActionResult Result = new HttpActionResult();

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

            HttpActionResult Result = new HttpActionResult(); 

            DataTable Table = SysData.Select(DataType, NoBlobs);
            JsonDataTable JTable = new JsonDataTable(Table);
            Result.SerializePacket(JTable);
            Result.IsSuccess = true;

            return Json(Result);
        }
        [HttpGet("/SysData/SelectById")]
        public async Task<JsonResult> SysDataSelectById(string Id)
        {
            await Task.CompletedTask;

            Type T = Id.GetType();

            HttpActionResult Result = new HttpActionResult();

            DataTable Table = SysData.SelectById(Id);
            JsonDataTable JTable = new JsonDataTable(Table);
            Result.SerializePacket(JTable);
            Result.IsSuccess = true;

            return Json(Result);
        }
        #endregion

        #region Desk
        [HttpGet("/Desk/GetMainMenu")]
        public async Task<JsonResult> DeskGetMainMenu()
        {
            await Task.CompletedTask;

            HttpActionResult Result = new HttpActionResult();

            Command[] MenuItems = DataStore.GetMainMenu();
            Result.SerializePacket(MenuItems);
            Result.IsSuccess = true;

            return Json(Result);
        }
        #endregion
    }
}


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
    /// <summary>
    /// Ajax controller.
    /// <para>NOTE: All AJAX calls, except of <see cref="BrokerController"/> calls, should be handled by this controller.</para>
    /// </summary>
    public class AjaxController : ControllerMvc, IAjaxController
    {
        DataTable SelectTable(string SqlText, string ConnectionName)
        {
            SqlStore Store = SqlStores.CreateSqlStore(ConnectionName);

            DataTable Table = Store.Select(SqlText);
            return Table;
        }


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

        [Route("/AjaxExecute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;
            HttpActionResult Result = DataStore.AjaxExecute(this, R);
            return Json(Result);
        }

        [Route("/SqlSelect")]
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

        [Route("/SqlSelectAll")]
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

 




    }
}

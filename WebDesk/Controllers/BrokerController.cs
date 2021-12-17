
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
using WebLib.AspNet;

 
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tripous.Data;

namespace WebDesk.Controllers
{
 
    /// <summary>
    /// Broker controller
    /// </summary>
    public class BrokerController : ControllerMvc
    {
        /* actions */
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
                JsonDataTable Packet = broker.JsonSelectList(SqlText, UseRowLimit? -1: 0);
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


 
    }












}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous;
using Tripous.Logging;
using Tripous.Data;


using WebLib.AspNet;
using WebLib.Models;

namespace WebLib
{
 
    static public partial class DataStore
    {


        /// <summary>
        /// Executes a request coming from an ajax call to ajax controller and returns the result.
        /// </summary>
        static public HttpActionResult AjaxExecute(IViewToStringConverter Controller, AjaxRequest Request)
        {
            AjaxResponse Response = AjaxRequest.Process(Request, Controller);

            if (Response == null)
                Sys.Throw($"Ajax Operation not supported: {Request.OperationName}");

            HttpActionResult Result = HttpActionResult.SetPacket(Response.GetPacketObject(), true);
            return Result;
        }

    }
}

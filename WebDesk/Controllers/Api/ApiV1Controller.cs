using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mime;
using System.Text;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
 
using Tripous;
using WebLib;
using WebLib.Models;
using WebLib.AspNet;
 

namespace WebDesk.Controllers.Api
{
 
    [Route("api/v1")]
    public class ApiV1Controller: ControllerApi
    {

        [HttpGet("supportedcultures")] // GET api/v1/supportedcultures
        [AllowAnonymous]
        public ActionResult<HttpActionResult> SupportedCultures()
        {
            Language[] Languages = DataStore.GetLanguages();
            List<string> List = new List<string>();
            foreach (Language Language in Languages)
                List.Add(Language.CultureCode);

            HttpActionResult Result = HttpActionResult.SetEntity(List, true);

            return Result;
        }

        [HttpPost("authenticate")]  
        [AllowAnonymous]
        //public IActionResult Authenticate(string ClientId, string Secret, string CultureCode)         // POST api/v1/authenticate?ClientId=teo&Secret=webdesk&CultureCode=en-US
        public IActionResult Authenticate(JwtAuthModel M)   
        {
            string ClientId = M.ClientId; string Secret = M.Secret; string CultureCode = M.CultureCode;

            if (string.IsNullOrWhiteSpace(CultureCode))
                CultureCode = Lib.DefaultCultureCode;

            Language[] Languages = DataStore.GetLanguages();
            Language Language = Languages.FindByCultureCode(CultureCode); 
            if (Language == null)
                return BadRequest(new { message = "Invalid culture" });


            ItemResponse<Requestor> Response = DataStore.ValidateRequestor(ClientId, Secret);
            Requestor Client = Response.Item;

            if (Response.Succeeded && Client != null)
            {                
                return JwtAuthHelper.GetAuthenticatedResult(Client, WApp.AppSettings.Jwt, CultureCode);
            } 

            return BadRequest(new { message = "Invalid ClientId or Secret." });
        }

        [HttpGet("persons")] // GET api/v1/persons
        public IActionResult Persons()
        {
            var Entity = new[]
            {
                new { Name = "Θεόδωρος", Age = 60 },
                new { Name = "Kostas", Age = 40 }
            };

            HttpActionResult Result = HttpActionResult.SetEntity(Entity, true);
            return Json(Result);
        }
    }
}

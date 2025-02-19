namespace WebDesk.Controllers.Api
{
 
    [Route("api/v1")]
    public class ApiV1Controller: ControllerApi
    {

        [HttpGet("supportedcultures")] // GET api/v1/supportedcultures
        [AllowAnonymous]
        public ActionResult<HttpPacketResult> SupportedCultures()
        {
            Language[] Languages = DataStore.GetLanguages();
            List<string> List = new List<string>();
            foreach (Language Language in Languages)
                List.Add(Language.CultureCode);

            HttpPacketResult Result = HttpPacketResult.SetEntity(List, true);

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

            HttpPacketResult Result = HttpPacketResult.SetEntity(Entity, true);
            return Json(Result);
        }
    }
}

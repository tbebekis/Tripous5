namespace Tripous.Http
{
    /// <summary>
    /// Represents the response of a call to the WebApi
    /// </summary>
    public class HttpClientResult 
    {
        HttpActionResult fActionResult;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpClientResult(string ActionUrl)
        {
            this.ActionUrl = ActionUrl;
        }
 

        /* public */
        /// <summary>
        /// Loads this instance from a specified http response
        /// </summary>
        public virtual async Task LoadFromResponseAsync(HttpResponseMessage Response)
        {
            this.ActionUrl = !string.IsNullOrWhiteSpace(ActionUrl) ? ActionUrl : Response.RequestMessage.RequestUri.ToString();
            this.StatusCode = Response.StatusCode;
            this.ReasonPhrase = Response.ReasonPhrase;
            this.IsSuccess = Response.IsSuccessStatusCode;

            if (Response.IsSuccessStatusCode)
                ResponseJsonText = await Response.Content.ReadAsStringAsync();

            CookieStringList = GetCookieStringList(Response);
        }
        
        /// <summary>
        /// Deserializes the response json text into an object.
        /// </summary>
        public virtual T Deserialize<T>()
        {
            if (!string.IsNullOrWhiteSpace(ResponseJsonText))
                return Json.Deserialize<T>(ResponseJsonText);
            return default(T);
        }
        /// <summary>
        /// Deserializes the response json text into a specified object.
        /// </summary>
        public virtual void Deserialize(object Instance)
        {
            if (Instance != null)
            {
                Json.PopulateObject(Instance, ResponseJsonText);
            }            
        }

        /* static */
        /// <summary>
        /// Returns a <see cref="Cookie"/> instance from a specified cookie text
        /// </summary>
        static public Cookie CreateCookie(string CookieText)
        {
            string[] Parts = CookieText.Split(';', StringSplitOptions.TrimEntries);

            string Part = Parts[0].Trim();

            string Name = Part.Split("=")[0];
            int Index = Part.IndexOf("=");
            string Value = Part.Remove(0, Index + 1).Trim();
            Cookie Cookie = new Cookie(Name, Value);

            for (int i = 1; i < Parts.Length; i++)
            {
                if (Parts[i].StartsWith("path=", StringComparison.OrdinalIgnoreCase))
                    Cookie.Path = Parts[i].Replace("path", "");
                else if (Parts[i].StartsWith("secure", StringComparison.OrdinalIgnoreCase))
                    Cookie.Secure = true;
                else if (Parts[i].StartsWith("httponly", StringComparison.OrdinalIgnoreCase))
                    Cookie.HttpOnly = true;
                else if (Parts[i].StartsWith("domain=", StringComparison.OrdinalIgnoreCase))
                    Cookie.Domain = Parts[i].Replace("domain=", "");
                else if (Parts[i].StartsWith("expires=", StringComparison.OrdinalIgnoreCase))
                {
                    string S = Parts[i].Replace("expires=", "", StringComparison.OrdinalIgnoreCase);
                    if (DateTime.TryParse(S, out DateTime Expires))
                        Cookie.Expires = Expires;
                }
            }

            return Cookie;
        }
        /// <summary>
        /// Returns a list of <see cref="Cookie"/> instances from a specified <see cref="HttpResponseMessage"/>  response, if any, else empty list.
        /// </summary>
        static public List<Cookie> GetCookieList(List<string> CookieStringList)
        {
            List<Cookie> Result = new List<Cookie>();

            foreach (string CookieText in CookieStringList)
            {
                Cookie Cookie = CreateCookie(CookieText);
                Result.Add(Cookie);
            }

            return Result;
        }
        /// <summary>
        /// Returns a list of cookies from a specified <see cref="HttpResponseMessage"/>  response, if any, else empty list.
        /// </summary>
        static public List<string> GetCookieStringList(HttpResponseMessage Response)
        {
            if (Response.Headers.TryGetValues("Set-Cookie", out var CookieStringList))
                return CookieStringList.ToList();
            return new List<string>();
        }
        /// <summary>
        /// Returns a list of <see cref="Cookie"/> instances from a specified <see cref="HttpResponseMessage"/>  response, if any, else empty list.
        /// </summary>
        static public List<Cookie> GetCookieList(HttpResponseMessage Response)
        {
            List<string> CookieStringList = GetCookieStringList(Response);
            List<Cookie> Result = GetCookieList(CookieStringList);
            return Result;
        }


        /* properties */
        /// <summary>
        /// The action url of the call
        /// </summary>
        public string ActionUrl { get; private set; }

        /* response properties */
        /// <summary>
        /// True when the call succeeds network-wise.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// The HTTP status code of the call
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// The reason text sent by the server
        /// </summary>
        public string ReasonPhrase { get; set; }
        /// <summary>
        /// The Packet result of a controller action.
        /// </summary>
        public string ResponseJsonText { get; set; }
        /*
        /// <summary>
        /// The Response object.
        /// </summary>
        [JsonIgnore]
        public HttpResponseMessage Response { get; set; }
        */
        /// <summary>
        /// Deserializes and returns the response json text as a <see cref="HttpActionResult"/>.
        /// </summary>
        [JsonIgnore]
        public HttpActionResult ActionResult { get { return fActionResult ?? (fActionResult = Deserialize<HttpActionResult>()); } }
        /// <summary>
        /// The string list of cookies from the response, if any, else null.
        /// </summary>
        [JsonIgnore]
        public List<string> CookieStringList { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

namespace Tripous
{
    /// <summary>
    /// Represents the response of a call to the WebApi
    /// </summary>
    public class HttpClientResult : EventArgs
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
            {
                ResponseJsonText = await Response.Content.ReadAsStringAsync();
            }
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
        /// <summary>
        /// Deserializes and returns the response json text as a <see cref="HttpActionResult"/>.
        /// </summary>
        [JsonIgnore]
        public HttpActionResult ActionResult { get { return fActionResult ?? (fActionResult = Deserialize<HttpActionResult>()); } }
 
    }
}

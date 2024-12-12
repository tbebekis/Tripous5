using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http; 

namespace Tripous
{

    /// <summary>
    /// To be used with events
    /// </summary>
    public class HttpClientCallInfo // : EventArgs
    {

        /// <summary>
        /// Constructor
        /// </summary>
        private HttpClientCallInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpClientCallInfo(HttpClientCallType CallType, string ActionUrl)
        {
            this.CallType = CallType;
            this.ActionUrl = ActionUrl;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpClientCallInfo(HttpClientCallType CallType, string ActionUrl, object Packet)
            : this(CallType, ActionUrl)
        {
            this.Packet = Packet;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpClientCallInfo(HttpClientCallType CallType, string ActionUrl, Dictionary<string, string> FormData)
            : this(CallType, ActionUrl)
        {
            this.FormData = FormData;
        }


        /* properties */
        /// <summary>
        /// The type of the call
        /// </summary>
        public HttpClientCallType CallType { get; }
        /// <summary>
        /// The relative Url of the action
        /// </summary>
        public string ActionUrl { get; }
        /// <summary>
        /// The packet, if any, else null. 
        /// Used by Post and Put only.
        /// </summary>
        public object Packet { get; }
        /// <summary>
        /// The FormData, if any, else null.
        /// Used by PostForm only.
        /// </summary>
        public Dictionary<string, string> FormData { get; }
        /// <summary>
        /// The Response object.
        /// Valid only in the <see cref="HttpClientBase{T}.CallAfter(HttpClientCallInfo)"/> call, else null.
        /// </summary>
        public HttpResponseMessage Response { get; set; } 
        /// <summary>
        /// The string list of cookies from the response, if any, else null.
        /// </summary>
        public List<string> CookieStringList { get; set; }

        /// <summary>
        /// The client result.
        /// Valid only in the <see cref="HttpClientBase{T}.CallAfter(HttpClientCallInfo)"/> call, else null.
        /// </summary>
        public HttpClientResult ClientResult { get; set; }
        /// <summary>
        /// User defined.
        /// </summary>
        public object Tag { get; set; }
    }
}

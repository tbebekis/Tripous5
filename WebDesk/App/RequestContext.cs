using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Tripous;
using Tripous.Web;

namespace WebDesk
{
    /// <summary>
    /// Represents a context regarding the current HTTP request (current visitor, selected warehouse, etc.)
    /// </summary>
    internal class RequestContext : IRequestContext
    {
        DataStoreSettings fSettings;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public RequestContext(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContext = HttpContextAccessor.HttpContext;

            // TODO: set selected language when Datastore is initialized
            Language = Tripous.Languages.DefaultLanguage;
        }

        /* properties */
        /// <summary>
        /// The http context
        /// </summary>
        public HttpContext HttpContext { get; }
        /// <summary>
        /// The http request
        /// </summary>
        public HttpRequest Request => HttpContext.Request;
        /// <summary>
        /// The query string as a collection of key-value pairs
        /// </summary>
        public IQueryCollection Query => Request.Query;

        /// <summary>
        /// The language of the current request
        /// </summary>
        public virtual Language Language { get; set; }
        /// <summary>
        /// The user or api client of the current request
        /// </summary>
        public virtual Requestor Requestor { get; set; }


        /// <summary>
        /// DataStore settings 
        /// </summary>
        public DataStoreSettings Settings => fSettings ?? (fSettings = Lib.GetSettings());
    }
}

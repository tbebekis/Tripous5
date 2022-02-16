using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using Tripous;

namespace WebLib
{


    /// <summary>
    /// Represents a context regarding the current HTTP request.
    /// </summary>
    public abstract class RequestContext  
    {
        /// <summary>
        /// Field
        /// </summary>
        protected DataStoreSettings fSettings;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public RequestContext(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContext = HttpContextAccessor.HttpContext;
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
        public abstract Language Language { get; set; }
        /// <summary>
        /// The user or api client of the current request
        /// </summary>
        public virtual Requestor Requestor { get; set; }
        /// <summary>
        /// True when the request is authenticated.
        /// </summary>
        public abstract bool IsAuthenticated { get; }

        /// <summary>
        /// DataStore settings 
        /// </summary>
        public virtual DataStoreSettings Settings => fSettings ?? (fSettings = Lib.GetSettings());
    }



}

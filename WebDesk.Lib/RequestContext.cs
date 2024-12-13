﻿namespace WebLib
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

        /* public */
        /// <summary>
        /// Returns the language object of the current request according to <see cref="CultureCode"/>
        /// </summary>
        public virtual Language GetLanguage()
        {
            return Languages.GetByCultureCode(CultureCode);
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
        /// The culture (language) of the current request specified as a culture code (en-US, el-GR)
        /// </summary>
        public abstract string CultureCode { get; set; }
 
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

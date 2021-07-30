using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using Tripous;

namespace WebDesk
{
    /// <summary>
    /// Represents a context regarding the current HTTP request.
    /// <para>NOTE: Whatever information is intended to have the lifetime of the HTTP request should be added in this interface.</para>
    /// </summary>
    public interface IRequestContext
    {
        /// <summary>
        /// The http context
        /// </summary>
        HttpContext HttpContext { get; }
        /// <summary>
        /// The http request
        /// </summary>
        HttpRequest Request { get; }
        /// <summary>
        /// The query string as a collection of key-value pairs
        /// </summary>
        IQueryCollection Query { get; }

        /// <summary>
        /// The language of the current request
        /// </summary>
        Language Language { get; set; }
        /// <summary>
        /// The user or api client of the current request
        /// </summary>
        Requestor Requestor { get; set; }

        /// <summary>
        /// Application settings
        /// </summary>
        DataStoreSettings Settings { get; }

        /// <summary>
        /// True when the request is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }




    /// <summary>
    /// A JWT request context (with JWT authentication)
    /// </summary>
    public interface IJwtRequestContext: IRequestContext
    {
    }




    /// <summary>
    /// A user request context (with cookies authentication)
    /// </summary>
    public interface IUserRequestContext : IRequestContext
    {
        /// <summary>
        /// Sign-in. Authenticates a specified, already validated, Visitor
        /// </summary>
        Task SignInAsync(Requestor V, bool IsPersistent, bool IsImpersonation);
        /// <summary>
        /// Sign-out.
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        /// True when the user has loged-in usin a SuperUserPassword
        /// </summary>
        bool IsImpersonation { get;  }
    }








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

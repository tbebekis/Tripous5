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
        Language Language { get; }
        /// <summary>
        /// The user or api client of the current request
        /// </summary>
        Requestor Requestor { get; set; }

        /// <summary>
        /// Application settings
        /// </summary>
        DataStoreSettings Settings { get; }
    }
}

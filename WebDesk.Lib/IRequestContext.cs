﻿namespace WebLib
{
    /// <summary>
    /// Represents a context regarding the current HTTP request.
    /// <para>NOTE: Whatever information is intended to have the lifetime of the HTTP request should be added in this interface.</para>
    /// </summary>
    public interface IRequestContext
    {

        /// <summary>
        /// Returns the language object of the current request according to <see cref="CultureCode"/>
        /// </summary>
        Language GetLanguage();

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
        /// The culture (language) of the current request specified as a culture code (en-US, el-GR)
        /// </summary>
        string CultureCode { get; set; }
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
}

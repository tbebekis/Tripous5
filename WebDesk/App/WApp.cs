using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Data;

//using Microsoft.Net.Http.Headers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.CookiePolicy;


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Caching.Memory; 

using Tripous;
using WebLib.AspNet;

namespace WebDesk
{
    /// <summary>
    /// Represents this application
    /// </summary>
    static internal partial class WApp
    {
        /* event handlers */
        /// <summary>
        /// The host has fully started.
        /// <para>Perform post-startup activities here</para>
        /// </summary>
        static void OnStarted()
        {
            Sys.LogInfo("Started", "Application");
        }
        /// <summary>
        /// The host is performing a graceful shutdown. Requests may still be processing. Shutdown blocks until this event completes.
        /// <para>Perform on-stopping activities here</para>
        /// </summary>
        static void OnStopping()
        {
            Sys.LogInfo("Stopping", "Application");
        }
        /// <summary>
        /// The host is completing a graceful shutdown. All requests should be processed. Shutdown blocks until this event completes.
        /// <para>Perform post-stopped activities here</para>
        /// </summary>
        static void OnStopped()
        {
            Sys.LogInfo("Stopped", "Application");
        }

        /* public */
        /// <summary>
        /// Returns a service specified by a type argument. If the service is not registered an exception is thrown.
        /// <para>WARNING: "Scoped" services can NOT be resolved from the "root" service provider. </para>
        /// <para>There are two solutions to the "Scoped" services problem:</para>
        /// <para> ● Use <c>HttpContext.RequestServices</c>, a valid solution since we use a "Scoped" service provider to create the service,  </para>
        /// <para> ● or add <c> .UseDefaultServiceProvider(options => options.ValidateScopes = false)</c> in the <c>CreateHostBuilder</c>() of the Program class</para>
        /// <para>see: https://github.com/dotnet/runtime/issues/23354 and https://devblogs.microsoft.com/dotnet/announcing-ef-core-2-0-preview-1/ </para>
        /// </summary>
        static public T GetService<T>()
        {
            return WSys.GetService<T>();
        }

        /// <summary>
        /// Returns the path url of an image, e.g. ~/themes/THEME/Content/images/IMAGE.png
        /// </summary>
        static public string ImageUrl(string FileName)
        {
            string S = $"~/images";
            S = Sys.UrlCombine(S, FileName);
            return S;
        }
 
        /* properties */
        /// <summary>
        /// Returns true when in debug mode, i.e. the DEBUG constant is defined.
        /// </summary>
        static public bool DebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
        /// <summary>
        /// Returns true when HostEnvironment.IsDevelopment() returns true.
        /// </summary>
        static public bool DevMode { get; private set; }
        /// <summary>
        /// Returns the HttpContext
        /// </summary>
        static public HttpContext HttpContext { get { return WSys.HttpContext; } }
        /// <summary>
        /// Returns the HostEnvironment
        /// </summary>
        static public IWebHostEnvironment HostEnvironment { get { return WSys.HostEnvironment; } }
        /// <summary>
        /// The configuration instance for the appsettings.json
        /// </summary>
        static public IConfiguration Configuration { get { return WSys.Configuration; } }
        /// <summary>
        /// Returns an <see cref="IFileProvider"/> pointing to <see cref="IHostEnvironment.ContentRootPath"/>.
        /// </summary>
        static public IFileProvider ContentRootFileProvider { get { return HostEnvironment.ContentRootFileProvider; } }

        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        static public string RootPath { get { return HostEnvironment.ContentRootPath; } }
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        static public string WebRootPath { get { return HostEnvironment.WebRootPath; } }
        /// <summary>
        /// Returns the physical path of the images folder, i.e. C:\MyApp\wwwroot\images
        /// </summary>
        static public string ImagesPath { get { return Path.Combine(WebRootPath, "images"); } }

        /// <summary>
        /// Application settings, coming from appsettings.json
        /// </summary>
        static public AppSettings AppSettings { get; private set; }
    }
}

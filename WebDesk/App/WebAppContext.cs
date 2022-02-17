using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;

using Tripous;
using WebLib;
using WebLib.AspNet;

namespace WebDesk
{
    /// <summary>
    /// Represents the web application. It is passed to the DataStore as a link point between the two parts.
    /// </summary>
    internal class WebAppContext : IWebAppContext
    {
        IWebAppCache fCache;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public WebAppContext()
        {
        } 


        /// <summary>
        /// Returns a service specified by a type argument. If the service is not registered an exception is thrown.
        /// </summary>
        public T GetService<T>()
        {
            return WSys.GetService<T>();
        }


        /// <summary>
        /// Adds a map between two types, from a source type to a destination type. A flag controls whether the mapping is a two-way one.
        /// <para>NOTE: Throws an exception if the ObjectMapper is already configured. </para>
        /// </summary>
        public void AddObjectMap(Type Source, Type Dest, bool TwoWay = false)
        {
            ObjectMapper.AddMap(Source, Dest, TwoWay);
        }
        /// <summary>
        /// Creates and returns a destination object, based on a specified type argument, and maps a specified source object to destination object.
        /// </summary>
        public TDestination Map<TDestination>(object Source) where TDestination : class
        {
            return ObjectMapper.Map<TDestination>(Source);
        }
        /// <summary>
        /// Maps a source to a destination object.
        /// </summary>
        public TDestination MapTo<TSource, TDestination>(TSource Source, TDestination Dest) where TSource : class where TDestination : class
        {
            return ObjectMapper.MapTo<TSource, TDestination>(Source, Dest);
        }

        /// <summary>
        /// Calls all plugins to add commands for the main menu of the application
        /// </summary>
        public void AddPluginMainMenuCommands(List<Command> CommandList)
        {
            foreach (var Plugin in WApp.PluginList)
                Plugin.AddMainMenuCommands(CommandList);
        }

        /// <summary>
        /// Returns the language object of the current request according to <see cref="Culture"/>
        /// </summary>
        public Language GetLanguage()
        {
            return Languages.GetByCultureCode(Culture.Name);
        }
        /// <summary>
        /// Returns the application settings
        /// </summary>
        public DataStoreSettings GetSettings()
        {
            return Lib.GetSettings();
        }
 
        /* properties */
        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        public string RootPath => WApp.RootPath;
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        public string WebRootPath => WApp.WebRootPath;
        /// <summary>
        /// Returns the physical path of the images folder, i.e. C:\MyApp\wwwroot\images
        /// </summary>
        public string ImagesPath => WApp.ImagesPath;

        /// <summary>
        /// Represents an application memory cache.
        /// </summary>
        public IWebAppCache Cache
        {
            get
            {
                if (fCache == null)
                    fCache = new WebAppMemoryCache(GetService<IMemoryCache>());
                return fCache;
            }
        }

        /// <summary>
        /// The <see cref="CultureInfo"/> culture of the current request.
        /// <para>CAUTION: The culture of each HTTP Request is set by a lambda in ConfigureServices().
        /// This property here uses that setting to return its value.
        /// </para>
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                IRequestCultureFeature Feature = Lib.HttpContext.Features.Get<IRequestCultureFeature>();
                return Feature != null ? Feature.RequestCulture.Culture : new CultureInfo("en-US");
            }
        }



        /*
                /// <summary>
                /// The <see cref="Language"/> language of the current request.
                /// <para>CAUTION: The culture of each HTTP Request is set by a lambda in ConfigureServices().
                /// This property here uses that setting to return its value.
                /// </para>
                /// </summary>
                public Language Language => Lib.RequestContext.Language; 
         */


        /// <summary>
        /// Returns true when HostEnvironment.IsDevelopment() returns true.
        /// </summary>
        public bool DevMode => WApp.DevMode;
    }
}

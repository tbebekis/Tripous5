using System;
using System.Globalization;

using Microsoft.AspNetCore.Http; 

using Tripous;
using WebDesk.AspNet;

namespace WebDesk
{
    /// <summary>
    /// Represents the web application
    /// </summary>
    public interface IWebAppContext
    {
        /// <summary>
        /// Returns a service specified by a type argument. If the service is not registered an exception is thrown.
        /// </summary>
        T GetService<T>(); 

        /// <summary>
        /// Adds a map between two types, from a source type to a destination type. A flag controls whether the mapping is a two-way one.
        /// <para>NOTE: Throws an exception if the ObjectMapper is already configured. </para>
        /// </summary>
        void AddObjectMap(Type Source, Type Dest, bool TwoWay = false);
        /// <summary>
        /// Creates and returns a destination object, based on a specified type argument, and maps a specified source object to destination object.
        /// </summary>
        TDestination Map<TDestination>(object Source) where TDestination : class;
        /// <summary>
        /// Maps a source to a destination object.
        /// </summary>
        TDestination MapTo<TSource, TDestination>(TSource Source, TDestination Dest) where TSource : class where TDestination : class;
 
        /* properties */
        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        string RootPath { get; }
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        string WebRootPath { get; }
        /// <summary>
        /// Returns the physical path of the images folder, i.e. C:\MyApp\wwwroot\images
        /// </summary>
        string ImagesPath { get; }
 
        /// <summary>
        /// Represents an application memory cache.
        /// </summary>
        IWebAppCache Cache { get; }
        /// <summary>
        /// Returns the application settings
        /// </summary>
        DataStoreSettings GetSettings();
 
        /// <summary>
        /// The <see cref="CultureInfo"/> culture of the current request.
        /// <para>CAUTION: The culture of each HTTP Request is set by a lambda in ConfigureServices().
        /// This property here uses that setting to return its value.
        /// </para>
        /// </summary>
        CultureInfo Culture { get; }
        /// <summary>
        /// The <see cref="Language"/> language of the current request.
        /// <para>CAUTION: The culture of each HTTP Request is set by a lambda in ConfigureServices().
        /// This property here uses that setting to return its value.
        /// </para>
        /// </summary>
        Language Language { get; }

        /// <summary>
        /// Returns true when HostEnvironment.IsDevelopment() returns true.
        /// </summary>
        bool DevMode { get; }
    }
}

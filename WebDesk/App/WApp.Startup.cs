using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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

using Newtonsoft.Json.Serialization;

using Tripous;
using Tripous.Logging;
using Tripous.Data;

using WebLib;
using WebLib.AspNet;

namespace WebDesk
{
    /// <summary>
    /// Represents this application
    /// </summary>
    static internal partial class WApp
    {
        static List<IPlugin> PluginList = new List<IPlugin>();
        static IDisposable AppSettingsChangeToken;
        static WebAppContext AppContext = new WebAppContext();
 

        /* private */
        /// <summary>
        /// Sets-up the SysConfig
        /// </summary>
        static void InitializeSysConfig()
        {
            Platform.IsWeb = true;

            SysConfig.ApplicationMode = ApplicationMode.Web;

            SysConfig.SolutionName = "WebDesk";
            SysConfig.ApplicationName = SysConfig.AppExeName;
            SysConfig.ApplicationTitle = SysConfig.ApplicationName;

            SysConfig.ObjectStoreExcludedAssemblies.AddRange(new string[] { });
            SysConfig.ObjectStoreAutoInvokeInitializers = false;

            SysConfig.DateFormat = DateTimeFormatType.Date;
            SysConfig.DateTimeFormat = DateTimeFormatType.DateTime24;
            SysConfig.TimeFormat = DateTimeFormatType.Time24;

            SysConfig.AppDataFolder = Path.Combine(SysConfig.AppExeFolder, "Data");

            SysConfig.MainAssembly = typeof(WApp).Assembly;
            SysConfig.SqlConnectionsFileName = "SqlConnections.json";
        }
        /// <summary>
        /// Initializes <see cref="DbProviderFactory"/> classes.
        /// </summary>
        static void InitializeDbProviderFactories()
        {
            // DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
        }

 
        /// <summary>
        /// Loads plugins
        /// </summary>
        static void LoadPlugins()
        {
            if (Directory.Exists(SysConfig.PluginsFolder))
            {
                PluginLoader<IPlugin> PluginLoader = new PluginLoader<IPlugin>();
                PluginLoader.RootFolder = SysConfig.PluginsFolder;
                PluginLoader.Prefix = "ewm_";

                IPlugin[] Plugins = PluginLoader.Execute();
                PluginList.AddRange(Plugins);                
            }
        }
        /// <summary>
        /// Initializes loaded plugins.
        /// </summary>
        static void InitializePlugins()
        {
            foreach (var Item in PluginList)
                Item.Initialize(WApp.AppContext);
        }

        /// <summary>
        /// Loads the active languages from the database table.
        /// <para>It also extracts language flag images from resources and saves it to wwwroot/images, if not already there.</para>
        /// </summary>
        static void LoadLanguages()
        {
            // load active languages
            Language[] A = DataStore.GetLanguages();
 
            foreach (var Item in A)
            {
                Tripous.Languages.Add(Item); 
            }


            // set the default language
            var Settings = DataStore.GetSettings();
            Tripous.Languages.SetDefaultLanguage(Settings.General.CultureCode);
        }
        /// <summary>
        /// A call-back function to be used with <see cref="ObjectMapper.Configure(Action{object})"/> methods.
        /// <para>NOTE: The passed in Configurator object is an AutoMapper.IMapperConfigurationExpression instance, in the current implementantion.</para>
        /// </summary>
        static void AddObjectMaps(object Configurator)
        {
            //ObjectMapper.AddMap(typeof(LanguageItem), typeof(AntyxSoft.Language), TwoWay: true);

            DataStore.AddObjectMaps(Configurator);

            foreach (var EM in PluginList)
                EM.AddObjectMaps(Configurator);
        }

        /// <summary>
        /// Initializes the application
        /// </summary>
        static void InitializeApplication()
        {
            try
            {
                // ● initialize libraries
                InitializeSysConfig();

                Pictures.ImagesPath = WApp.ImagesPath;

                Tripous.Logging.Logger.AddFileListener();

                ObjectStore.Initialize();
                Db.Initialize();
                Lib.Initialize(WApp.AppContext);

                // ● data store
                InitializeDbProviderFactories();                
                DataStore.Initialize(WApp.AppContext);

                // ● plugins
                LoadPlugins();

                // ● languages
                LoadLanguages();

                // ● plugin initialization
                InitializePlugins();

                // ● object maps
                ObjectMapper.Configure(AddObjectMaps);               
 
            }
            catch (Exception ex)
            {
                LogInfo Info = new LogInfo("Application", "Start", "", LogLevel.Fatal, ex, "Fatal error on WApp.Start()");
                Logger.Log(Info);
                throw;
            }
        }

        /* internal */
        /// <summary>
        /// Configure services.
        /// <para>Service Lifetime: </para>
        /// <para>Transient : each time is requested</para>
        /// <para>Scoped    : once per HTTP Request</para>
        /// <para>Singleton : once per application</para>
        /// </summary>
        static public void ConfigureServices(IServiceCollection services)
        {
            DevMode = HostEnvironment.IsDevelopment();

            /*
            Service Lifetime: 
             ● Transient : each time is requested
             ● Scoped    : once per HTTP Request
             ● Singleton : once per application
             */

            // ● AppSettings - bind AppSettings to a private field
            IConfigurationSection AppSettingsSection = Configuration.GetSection(typeof(AppSettings).Name);
            services.Configure<AppSettings>(AppSettingsSection);
            AppSettings = AppSettings ?? new AppSettings();
            AppSettingsSection.Bind(AppSettings);

            SysConfig.PluginsFolder = AppSettings.PluginsFolder;

            // ● custom services 
            services.AddScoped<IJwtRequestContext, JwtRequestContext>();
            services.AddScoped<IUserRequestContext, UserRequestContext>();
 

            // ● HttpContext and ActionContext
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();  // see: https://github.com/aspnet/mvc/issues/3936

            // ● Memory Cache - NOTE: adds the cache as singleton
            services.AddDistributedMemoryCache(); // AddMemoryCache(); AddDistributedMemoryCache

            // ● Session
            services.AddSession(options =>
            {
                options.Cookie.Name = "DevAppSession";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;  // Make the session cookie essential
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
            });


            // ● IHttpClientFactory
            services.AddHttpClient();

            // ● Build a temporary root service provider
            // We set the final root ServiceProvider in Configure()
            //ServiceProvider TempServiceProvider = services.BuildServiceProvider();
            //WSys.RootServiceProvider = TempServiceProvider;

            // ● Application
            InitializeApplication();

            // ● Request Localization
            // https://www.codemag.com/Article/2009081/A-Deep-Dive-into-ASP.NET-Core-Localization
            services.Configure((RequestLocalizationOptions options) => {

                var Provider = new CustomRequestCultureProvider(async (HttpContext) => {
                    await Task.Yield();
                    IRequestContext RequestContext = Lib.IsMvcRequest(HttpContext) ? GetService<IUserRequestContext>() : GetService<IJwtRequestContext>();
                    Language Language = RequestContext.Language; 
                    return new ProviderCultureResult(Language.CultureCode);
                });

                var Cultures = DataStore.GetLanguages().Select(item => item.GetCulture()).ToArray(); // Tripous.Languages.Items.Select(item => item.GetCulture()).ToArray();
                options.DefaultRequestCulture = new RequestCulture(DataStore.EnLanguage.GetCulture());
                options.SupportedCultures = Cultures;
                options.SupportedUICultures = Cultures;
                options.RequestCultureProviders.Insert(0, Provider);
            });


            // ● Security - authentication
            AuthenticationBuilder AuthBuilder = services.AddAuthentication(o => {
                o.DefaultScheme = Lib.CookieAuthScheme;
                o.DefaultAuthenticateScheme = o.DefaultScheme;
                o.DefaultChallengeScheme = o.DefaultScheme;
            });

            // ● Cookie authentication
            AuthBuilder.AddCookie(Lib.CookieAuthScheme, o => {
                CookieAuthHelper.SetCookieConfigurationOptions(o);
                });

            // ● JWT authentication
            if (AppSettings.Jwt.Enabled)
            {
                AuthBuilder.AddJwtBearer(Lib.JwtAuthScheme, o => {
                    JwtAuthHelper.SetJwtBearerConfigurationOptions(o, AppSettings.Jwt);
                });
            }


            /*
            //● authentication
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = WApp.Settings.Azure.GetAuthority();
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,

                    // set both AppIdUri and ClientIs as valid audiences in the access token
                    ValidAudiences = new List<string>
                    {
                        WApp.Settings.Azure.ClientId,
                        WApp.Settings.Azure.GetAppIdUri()
                    }
                };
            });
            */

            services.AddScoped<AuthCookieEvents>();

            services.Configure<CookiePolicyOptions>(o =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // If CheckConsentNeeded is set to true, then the IsEssential should be also set to true, for any Cookie's CookieOptions setting.
                // SEE: https://stackoverflow.com/questions/52456388/net-core-cookie-will-not-be-set
                o.CheckConsentNeeded = context => true;

                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS.
                o.MinimumSameSitePolicy = SameSiteMode.None;

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                o.HttpOnly = HttpOnlyPolicy.Always;

                // Add the SameSite attribute, this will emit the attribute with a value of none.
                o.Secure = CookieSecurePolicy.Always;
            });


            //● Security - authorization
            // SEE: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme
            services.AddAuthorization(o =>
            {
                o.AddPolicy(Lib.PolicyAuthenticated, policy => { policy.RequireAuthenticatedUser(); });
            });

            // ● HSTS
            if (WApp.HostEnvironment.IsDevelopment())
            {
                // services.AddHsts(options => { options.MaxAge = TimeSpan.FromHours(1); });
            }
            else // Production, etc.
            {
                // HSTS
                HSTSSettings HSTS = WApp.AppSettings.HSTS;
                services.AddHsts(options =>
                {
                    options.Preload = HSTS.Preload;
                    options.IncludeSubDomains = HSTS.IncludeSubDomains;
                    options.MaxAge = TimeSpan.FromHours(HSTS.MaxAgeHours >= 1 ? HSTS.MaxAgeHours : 1);
                    if (HSTS.ExcludedHosts != null && HSTS.ExcludedHosts.Count > 0)
                    {
                        foreach (string host in HSTS.ExcludedHosts)
                            options.ExcludedHosts.Add(host);
                    }
                });
            }

            // ● MVC View location expander and Themes support 
            if (WApp.HostEnvironment.IsDevelopment())
            {
                ViewLocationExpander.AddLocation($"/Demos/{{1}}/{{0}}.cshtml");
                ViewLocationExpander.AddLocation($"/Demos/Shared/{{0}}.cshtml");
            }
 
            ViewLocationExpander.AddLocation($"/Views/Ajax/{{0}}.cshtml");
            services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });
 
            

            // ● MVC
            IMvcBuilder MvcBuilder = services.AddControllersWithViews(o => {
                
                o.ModelBinderProviders.Insert(0, new AppModelBinderProvider());

                //if (!WApp.HostEnvironment.IsDevelopment())
                {
                    o.Filters.Add<ActionExceptionFilter>();
                }

                /* No global model validation, use the BaseControllerMvc.ValidateModel() method
                  o.Filters.Add<ModelValidationFilter>();
                 */

                //o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })

            // •    .AddViewOptions(o => { o.ViewEngines.Insert(0, new MyCustomViewEngine()); });

            // •    .ConfigureApplicationPartManager((AppManager) => { })

            // •    .AddApplicationPart(Assmply)

            /* •  
            // the default case for serializing output to JSON is camelCase in Asp.Net Core, so we turn it off here.
            // https://stackoverflow.com/questions/38728200/how-to-turn-off-or-handle-camelcasing-in-json-response-asp-net-core
            // https://github.com/aspnet/Announcements/issues/194
            .AddNewtonsoftJson(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver());
            */
            .AddNewtonsoftJson(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver())
            // or
            //.AddNewtonsoftJson()
            //.AddJsonOptions(opt => { opt.JsonSerializerOptions.PropertyNamingPolicy = null; })
            ;

            

            // ● Razor Runtime Compilation
            // see: https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-compilation
            if (HostEnvironment.IsDevelopment())
            {
                MvcBuilder.AddRazorRuntimeCompilation();
            }

            // ● JS Report - for converting HTML to PDF
            /*
                services.AddJsReport(
                    new LocalReporting()
                    .UseBinary(JsReportBinary.GetBinary())
                    .Configure(cfg => {
                        cfg.AllowLocalFilesAccess = true;
                        return cfg;
                    })
                    .KillRunningJsReportProcesses()
                    .AsUtility()
                    .Create()
                    ); 
             */



            // ● Disable the built-in global model validation for controllers decorated with [ApiController] 
            // The [ApiController] attribute enables some special controller behavior.
            // see: https://docs.microsoft.com/en-us/aspnet/core/web-api#attribute-on-specific-controllers
            // The [ApiController] attribute  performs automatic model state validation and in case of an invalid model state, 
            // responds with a 400 bad request error. 
            // When the controller is decorated with [ApiController] attribute, the framework would automatically register 
            // a ModelStateInvalidFilter which runs on the OnActionExecuting event. 
            // This checks for the model state validity and returns the response accordingly. 
            // SEE ALSO: The custom WebDesk.AspNet.ModelValidationFilter enables the same behavior to all controllers.
            services.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });

        }
        /// <summary>
        /// Configure application
        /// <para>Called from Configure()</para>
        /// </summary>
        static public void Configure(IApplicationBuilder app, IOptionsMonitor<AppSettings> AppSettingsAccessor)
        {


            // ● RootServiceProvider - set the root service provider
            WSys.RootServiceProvider = app.ApplicationServices;

            // ● events
            IHostApplicationLifetime appLifetime = WSys.GetService<IHostApplicationLifetime>();
            appLifetime.ApplicationStarted.Register(WApp.OnStarted);
            appLifetime.ApplicationStopping.Register(WApp.OnStopping);
            appLifetime.ApplicationStopped.Register(WApp.OnStopped);

            // ● AppSettings - initializes application settings and sets-up setttings change notification.
            AppSettings = AppSettingsAccessor.CurrentValue;

            AppSettingsChangeToken = AppSettingsAccessor.OnChange(settings =>
            {
                if (AppSettingsChangeToken != null)
                {
                    try
                    {
                        AppSettingsChangeToken.Dispose();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        AppSettingsChangeToken = null;
                    }
                }
                AppSettings = settings;
            });
 

            //----------------------------------------------------------------------------------------
            // Middlewares
            //----------------------------------------------------------------------------------------
            // ● Session
            app.UseSession();

            // TODO: check middleware order
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#order

            // ● Exceptions
            if (HostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ● Proxy headers - forward proxy headers onto current request
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });


            // ● Miscs
            if (HostEnvironment.IsDevelopment())
            {
                //app.UseBrowserLink();           // see: https://docs.microsoft.com/en-us/aspnet/core/client-side/using-browserlink
            }
            else
            {
                app.UseHsts();                  // see comments in HSTSSettings class
            }


            // ● HTTPS Redirection  
            app.UseHttpsRedirection();

            // ● Static files

            // local function
            void StaticFileResponseProc(StaticFileResponseContext context)
            {
                var DataStoreSettings = DataStore.GetSettings();

                if (!string.IsNullOrEmpty(DataStoreSettings.Http.StaticFilesCacheControl))
                    context.Context.Response.Headers.Append(Microsoft.Net.Http.Headers.HeaderNames.CacheControl, DataStoreSettings.Http.StaticFilesCacheControl);
            }

            // NOTE: We may have multiple calls to app.UseStaticFiles() in order to pass instances of PhysicalFileProvider serving files from other locations. 
            // "You can provide additional instances of UseStaticFiles/UseFileServer with other file providers to serve files from other locations."
            // SEE: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-5.0#serve-files-from-multiple-locations
            // SEE: https://github.com/dotnet/AspNetCore.Docs/issues/15578#issuecomment-551238579

            // common static files 
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = StaticFileResponseProc
            });

            // theme static files
            /*
                        app.UseStaticFiles(new StaticFileOptions
                        {
                            FileProvider = new PhysicalFileProvider(WApp.ThemesPath),
                            RequestPath = new PathString("/" + ViewLocationExpander.ThemesFolder),
                            OnPrepareResponse = StaticFileResponseProc
                        }); 
             */

            // ● Cookie Policy
            app.UseCookiePolicy();

            // ● Routing
            app.UseRouting();

            // ● Request Localization 
            // UseRequestLocalization initializes a RequestLocalizationOptions object. 
            // On every request the list of RequestCultureProvider in the RequestLocalizationOptions is enumerated 
            // and the first provider that can successfully determine the request culture is used.
            // SEE: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization#localization-middleware
            app.UseRequestLocalization();

            // ● Cors
            //app.UseCors();

            // ● Security
            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseResponseCompression();
            // app.UseResponseCaching();


            // ● MVC 
            // https://stackoverflow.com/questions/57846127/what-are-the-differences-between-app-userouting-and-app-useendpoints/61413251#61413251
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

        }

    }
}

namespace WebDesk
{


    /// <summary>
    /// The startup class. It actually delegates all "action" to <see cref="WApp"/> class
    /// <para>NOTE: only those two services can be injected into Startup constructor: <see cref="IConfiguration"/>, <see cref="IWebHostEnvironment"/></para>
    /// </summary>
    public class Startup
    {
        IConfiguration Configuration;
        IWebHostEnvironment HostEnvironment;

        /* construction */
        /// <summary>
        /// Constructor.
        /// <para>NOTE: only those two services can be injected into Startup constructor: <see cref="IConfiguration"/>, <see cref="IWebHostEnvironment"/></para>
        /// <para>see: https://docs.microsoft.com/en-gb/aspnet/core/fundamentals/dependency-injection#services-injected-into-startup</para>
        /// </summary>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnvironment = environment;

            WSys.Configuration = Configuration;
            WSys.HostEnvironment = HostEnvironment;
        }

        /* public */
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            WApp.ConfigureServices(services);
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IOptionsMonitor<AppSettings> SettingsAccessor)
        {
            /*
                         app.Use(async (context, next) =>
                        {
                            var url = context.Request.Path.Value;

                            // Rewrite to index
                            if (url.Contains("Localized"))
                            {
                                // rewrite and continue processing
                                //context.Request.Path = "/home/index";
                            }

                            await next();
                        });
             */
 

            WApp.Configure(app, SettingsAccessor);
        }
    }
}

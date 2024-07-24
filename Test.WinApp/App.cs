 

namespace Test.WinApp
{
    static public partial class App
    {
        static MainForm MainForm;
        static FileLogListener fFileLogListener;
        static SyncedLogListener fSyncedLogListener;

        /* private */
        /// <summary>
        /// Initializes the <see cref="SysConfig"/> static class.
        /// </summary>
        static void InitializeSysConfig()
        {
            Platform.IsDesktop = true;

            SysConfig.ApplicationMode = ApplicationMode.Desktop;

            SysConfig.SolutionName = "Test.WinApp";
            SysConfig.ApplicationName = SysConfig.AppExeName;
            SysConfig.ApplicationTitle = SysConfig.ApplicationName;
            SysConfig.MainAssembly = typeof(App).Assembly;


            SysConfig.ObjectStoreExcludedAssemblies.AddRange(new string[] { });
            SysConfig.ObjectStoreAutoInvokeInitializers = false;

            SysConfig.DateFormat = DateTimeFormatType.Date;
            SysConfig.DateTimeFormat = DateTimeFormatType.DateTime24;
            SysConfig.TimeFormat = DateTimeFormatType.Time24;

            SysConfig.AppDataFolder = Path.Combine(SysConfig.AppExeFolder, "Data"); // Path.GetFullPath(@"..\..\..\Data");

            SysConfig.GuidOids = true;
            SysConfig.VariablesPrefix = ":@";
            SysConfig.CompanyFieldName = "CompanyId";
            SysConfig.SqlConnectionsFileName = "SqlConnections.json";
            SysConfig.EnKey1 = "tripous";
            SysConfig.SelectSqlRowsLimit = 400;
            SysConfig.MachineIdRequired = false;

            SysConfig.UsersEnabled = false;
        }
        /// <summary>
        /// Registers DbProviderFactory classes
        /// </summary>
        static void RegisterDbProviderFactories()
        {
            DbProviderFactories.RegisterFactory("System.Data.SQLite", System.Data.SQLite.SQLiteFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Loads database configuration settings.
        /// </summary>
        static void LoadConnectionStrings()
        {
            SysConfig.SqlConnectionsFolder = typeof(App).Assembly.GetFolder();
            SqlConnectionInfoList ConnectionInfoList = new SqlConnectionInfoList();
            Db.Connections = ConnectionInfoList.SqlConnections;
        }
        /// <summary>
        /// Creates any non-existing creatable database.
        /// </summary>
        static void CreateDatabases()
        {
            SqlProvider Provider;
            string ConnectionString;

            SqlConnectionInfo DefaultConnectionInfo = Db.DefaultConnectionInfo;

            Provider = DefaultConnectionInfo.GetSqlProvider();
            ConnectionString = DefaultConnectionInfo.ConnectionString;

            if (!Provider.DatabaseExists(ConnectionString) && Provider.CanCreateDatabases)
            {
                Provider.CreateDatabase(ConnectionString);
            }

            foreach (var ConInfo in Db.Connections)
            {
                if (ConInfo != DefaultConnectionInfo)
                {
                    Provider = ConInfo.GetSqlProvider();
                    ConnectionString = ConInfo.ConnectionString;

                    if (!Provider.DatabaseExists(ConnectionString) && Provider.CanCreateDatabases)
                    {
                        Provider.CreateDatabase(ConnectionString);
                    }
                }
            }

        }
        static void RegisterSchemas()
        {
            Schema AppSchema = Schemas.GetApplicationSchema();
            SchemaVersion Version; // = AppSchema.Add(Version: 1);


            Type[] Types = typeof(App).Assembly.GetTypesSafe();
            MethodInfo[] Methods;
            RegisterSchemaFuncAttribute Attr;

            foreach (Type Type in Types)
            {
               Methods = Type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (MethodBase Method in Methods)
                {
                    Attr = Attribute.GetCustomAttribute(Method, typeof(RegisterSchemaFuncAttribute)) as RegisterSchemaFuncAttribute;
                    if (Attr != null)
                    {
                        Version = AppSchema.FindOrAdd(Attr.Version);

                        // static void RegisterSchema(Schema Schema, SchemaVersion Version)
                        Method.Invoke(null, new object[] { AppSchema, Version });  
                    }
                }
 
            }
        }
        /// <summary>
        /// Creates database tables etc. based on the registered schemas
        /// </summary>
        static void ExecuteSchemas()
        {
            Schemas.Execute();
        }
 
        static void RegisterBrokers()
        {
            Type[] Types = typeof(App).Assembly.GetTypesSafe();
            MethodInfo[] Methods;
            RegisterBrokersFuncAttribute Attr;

            foreach (Type Type in Types)
            {
                Methods = Type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (MethodBase Method in Methods)
                {
                    Attr = Attribute.GetCustomAttribute(Method, typeof(RegisterBrokersFuncAttribute)) as RegisterBrokersFuncAttribute;
                    if (Attr != null)
                    {
                        // static void RegisterBrokers()
                        Method.Invoke(null, new object[] {  });
                    }
                }

            }
        }

        /* public */
        static public void Initialize(MainForm MainForm)
        {
            InitializeSysConfig();

            App.MainForm = MainForm;

            fFileLogListener = new FileLogListener(Path.Combine(SysConfig.AppExeFolder, "Logs"));
            fSyncedLogListener = new SyncedLogListener();
            fSyncedLogListener.EntryEvent += SyncedLogListener_EntryEvent; 

            ObjectStore.Initialize();
            Db.Initialize();

            RegisterDbProviderFactories();
            LoadConnectionStrings();
            CreateDatabases();

            RegisterSchemas();
            ExecuteSchemas();

            SqlStore = SqlStores.CreateDefaultSqlStore();

            RegisterBrokers();
        }

        static void SyncedLogListener_EntryEvent(object sender, LogEntryArgs e)
        {
            LogBox.AppendLine(e.Entry.AsJson());
        }

 

        static public SqlConnectionInfoList ConnectionInfoList { get; private set; }

        /// <summary>
        /// The Default store
        /// </summary>
        static public SqlStore SqlStore { get; private set; }
    }
}

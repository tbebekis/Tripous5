namespace WebLib
{

    /// <summary>
    /// Represents the database
    /// </summary>
    static public partial class DataStore
    {
 
        static int? fDefaultCacheTimeoutMinutes;

        static int DefaultCacheTimeoutMinutes
        {
            get
            {
                if (!fDefaultCacheTimeoutMinutes.HasValue)
                {
                    var Settings = GetSettings();
                    int v = Settings.General.DefaultCacheTimeoutMinutes;
                    fDefaultCacheTimeoutMinutes = v < 15 ? 15 : v;
                }

                return fDefaultCacheTimeoutMinutes.Value;
            }
        }

        static void EnsureLanguageResources(Language Language)
        {
            if (Language.Resources.IsEmpty)
            {
                Dictionary<string, string> ResourceStringList = StrRes.GetStringListAsDictionary(Language.Code);
                Language.Resources.LoadFrom(ResourceStringList);
            }
        }
        static SqlStoreLogListener fSqlStoreLogListener;


        /// <summary>
        /// Registers DbProviderFactory classes
        /// </summary>
        static void RegisterDbProviderFactories()
        {  
            DbProviderFactories.RegisterFactory("System.Data.SQLite", System.Data.SQLite.SQLiteFactory.Instance);
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("FirebirdSql.Data.FirebirdClient", FirebirdSql.Data.FirebirdClient.FirebirdClientFactory.Instance); 
            DbProviderFactories.RegisterFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
        }

        /// <summary>
        /// Loads database configuration settings.
        /// </summary>
        static void LoadConnectionStrings()
        {
            SysConfig.SqlConnectionsFolder = typeof(DataStore).Assembly.GetFolder();
            SqlConnectionInfoList ConnectionInfoList = new SqlConnectionInfoList();

            /*
                        // normalize databas path
                        SqlConnectionInfo DefaultConnectionInfo = ConnectionInfoList.SqlConnections.FirstOrDefault(item => Sys.IsSameText(item.Name, "Default"));
                        string CS = DefaultConnectionInfo.ConnectionString;
                        ConnectionStringBuilder CSB = new ConnectionStringBuilder(CS);
                        string Database = CSB.Database;
                        Database = Path.GetFileName(Database);
                        string Folder = typeof(DataStore).Assembly.GetFolder();
                        Database = Path.Combine(Folder, Database);
                        CSB["Data Source"] = Database;
                        DefaultConnectionInfo.ConnectionString = CSB.ConnectionString; 
             */

            Db.Connections = ConnectionInfoList.SqlConnections;

            //TestDefs();
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
      
        /// <summary>
        /// Registers and executes the system schema tables.
        /// </summary>
        static void ExecuteSystemSchema()
        {
            SysTables.AddSchemaAll();
            Schemas.Execute();
        }


        static void TestDefs()
        {
            DataTableDef Table = new DataTableDef();
            Table.Name = "Country";
            Table.AddId();
            Table.AddString("Code", 40, true, null, "''");
            Table.AddString("Name", 96, false).Unique = true;
            Table.AddString("CustomerId", 40, true).SetForeignKey("Customer.Id");
            Table.AddField("Date", DataFieldType.DateTime, false); 

            string DefText = Table.GetDefText();

            SqlProvider Provider = Db.DefaultConnectionInfo.GetSqlProvider();
            DefText = Provider.ReplaceDataTypePlaceholders(DefText);


            string JsonText = Json.ToJson(Table);
            Table = Json.FromJson<DataTableDef>(JsonText);
            Table.Check();
        }
        static void Test()
        {
            SqlBroker Broker = SqlBroker.CreateSingleTableBroker(SysTables.StrRes);
            Broker.Insert();
            Broker.Row["LanguageCode"] = "el";
            Broker.Row["EntryKey"] = "Paparia";
            Broker.Row["EntryValue"] = "Παπάρια";
            Broker.Commit();
        }

        /// <summary>
        /// Initializer
        /// </summary>
        static public void Initialize(IWebAppContext App)
        {
            if (DataStore.App == null)
            {
                DataStore.App = App;                
            }           
        }
        static public void InitializeDatabases()
        {
            RegisterDbProviderFactories();
            LoadConnectionStrings();
            CreateDatabases();

            SqlStore = SqlStores.CreateDefaultSqlStore();

            ExecuteSystemSchema();
            ExecuteSchemas();
 
            fSqlStoreLogListener = new SqlStoreLogListener();
        }
        static public void RegisterDescriptors()
        {
            AjaxRequest.Register(new AjaxRequestDefaultHandler());

            RegisterBrokers();
            RegisterLocators();

            RegisterViews();

            EntityDescriptors.Load(typeof(DataStore).Assembly);
        }
        static public void AddTestData()
        {
            // add Traders
            string SqlText = "select count(Id) as Result from Trader";
            int Count = SqlStore.IntegerResult(SqlText, 0);
            if (Count == 0)
            {
                string[] Traders = { "Jules Verne", "Robert Heinlein", "Isaac Asimov", "William Gibson", "Arthur Clarke" };
                SqlBroker Broker = SqlBroker.Create("Trader", true, false);

                foreach (string TraderName in Traders)
                {
                    Broker.Insert();
                    Broker.Row["Name"] = TraderName;
                    Broker.Commit();
                }

            }
        }

        /// <summary>
        /// Called by the system. 
        /// <para>Instructs plugin to add any object to object mappings may have by calling either:</para>
        /// <para><c>App.AddObjectMap(Type Source, Type Dest, bool TwoWay = false)</c></para>
        /// <para>or the passed in Configurator object which in the current implementantion is an AutoMapper.IMapperConfigurationExpression instance </para>
        /// </summary>
        static public void AddObjectMaps(object Configurator)
        {
            //App.AddObjectMap(typeof(Visitor), typeof(DeVisitor), true);
            //App.AddObjectMap(typeof(AppUser), typeof(DeAppUser), true);
        }

        /* public */
        /// <summary>
        /// Returns the list of languages this web-site supports.
        /// <para>NOTE: Throws an exception if languages not defined.</para>
        /// </summary>
        static public Language[] GetLanguages()
        {
 
            //-----------------------------------------------
            Language[] GetLanguagesInternal()
            {
                List<Language> LanguageList = new List<Language>();

                string Sql = SSelectLanguages;
                DataTable Table = SqlStore.Select(Sql);               
                Language Item;
                
                foreach (DataRow Row in Table.Rows)
                {
                    Item = new Language();
                    LanguageList.Add(Item);

                    Item.Id = Row.AsString("Id");
                    Item.Name = Row.AsString("Name");
                    Item.Code = Row.AsString("Code");
                    Item.CultureCode = Row.AsString("CultureCode");
                    Item.FlagImage = Row.AsString("FlagImage");
                }

                Languages.SetLanguages(LanguageList);

                return LanguageList.ToArray();
            }
            //-----------------------------------------------

            // This method is called even when the initialization is not complete and no ServiceProvider is available
            // so we need extra precaution when call it.

            Language[] Result = null;

            if (WSys.HttpContext == null)
            {
                Result = GetLanguagesInternal();
            }
            else
            {
                Result = App.Cache.Get<Language[]>(CacheKeys.LanguageList, () => {
                    int TimeoutMinutes = DefaultCacheTimeoutMinutes;
                    return (TimeoutMinutes, GetLanguagesInternal());
                });
            }


            if (Result.Length == 0)
                Lib.Error($"Languages not defined.");

            return Result;
        }
        /// <summary>
        /// Returns the general resource strings (not translatable table data) of a language specified by a culture code, (e.g. el-GR, en-US, etc), as dictionary.
        /// <para>NOTE: Throws exceptions if no languages defined or the specified language not exists.</para>
        /// </summary>
        static public Dictionary<string, string> GetLanguageResourceStringList(string CultureCode)
        {
            Language Language = Languages.GetByCultureCode(CultureCode);

            EnsureLanguageResources(Language);

            Dictionary<string, string> Result = Language.Resources.GetResourceStringListDictionary();
            return Result;
        }
 
        /// <summary>
        /// Returns the data-store settings
        /// <para>WARNING: do NOT use the cache with settings.</para>
        /// </summary>
        static public DataStoreSettings GetSettings()
        {
            // TODO: load app settings
            return new DataStoreSettings();
        }

        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// </summary>
        static public Requestor GetRequestor(string Id)
        {
            Requestor Result = null;

            string SqlText = $@"
select
    *
from
    AppUser
where 
    Id = '{Id}'";

            DataRow Row = SqlStore.SelectResults(SqlText);
            if (Row != null)
            {
                Result = new Requestor();
                Result.Id = Row.AsString("Id");
                Result.UserId = Row.AsString("UserId");
                Result.Name = Row.AsString("Name");
                Result.Email = Row.AsString("Email");
                Result.IsBlocked = Row.AsInteger("IsBlocked") > 0;
                Result.Level = (UserLevel)Row.AsInteger("Level"); 
            }

            return Result;
        }
        /// <summary>
        /// Validates the specified credentials and returns a Visitor on success, else null.
        /// </summary>
        static public ItemResponse<Requestor> ValidateRequestor(string UserId, string Password)
        {
            ItemResponse<Requestor> Result = new ItemResponse<Requestor>();

            if (string.IsNullOrWhiteSpace(UserId))
            {
                Result.AddError(Localize("Requestor.NotRegistered"));
            }
            else
            {
                string SqlText = $@"
select
    *
from
    AppUser
where 
    UserId = '{UserId}'";


                DataRow Row = SqlStore.SelectResults(SqlText);

                if (Row == null)
                {
                    Result.AddError(Localize("Requestor.NotRegistered"));
                }
                else if (string.IsNullOrWhiteSpace(Row.AsString("Password")) || string.IsNullOrWhiteSpace(Row.AsString("PasswordSalt")))
                {
                    Result.AddError(Localize("Requestor.NotRegistered"));
                }
                else if (!ValidatePassword(Password, Row.AsString("PasswordSalt"), Row.AsString("Password")))
                {
                    Result.AddError(Localize("Requestor.InvalidPassword"));
                }
                else if (Row.AsInteger("IsActivated") <= 0)
                {
                    Result.AddError(Localize("Requestor.NotActivated"));
                }
                else if (Row.AsInteger("IsBlocked") > 0)
                {
                    Result.AddError(Localize("Requestor.NotValidRequestor"));
                }
                else
                {
                    Result.Item = GetRequestor(Row.AsString("Id"));
                }
            }
 

            if (Result.Item == null && (Result.Errors == null || Result.Errors.Count == 0))
                Result.AddError("Unknown Error.");

            return Result;
        }
 
        static public Command[] GetMainMenu()
        {
            List<Command> Result = new List<Command>();

            // System
            Command BarItem = new Command() { TitleKey = "System", Type = Tripous.RequestType.Ui, Id = "{E7DACAFA-CA45-4238-84F9-D87A121A0CC2}" };
            Result.Add(BarItem);
            BarItem.AddUi(new Command() { Name = "Ui.SysData.Table", TitleKey = "Tables", IsSingleInstance = true, Id = "{5F61B877-031F-4FA4-954E-80BB27363E6D}" });
            BarItem.AddUi(new Command() { Name = "Ui.SysData.Broker", TitleKey = "Brokers", IsSingleInstance = true, Id = "{67014D07-BB5A-46F5-AF4E-222A6FE31F9C}" });
            BarItem.AddUi(new Command() { Name = "Ui.SysData.Locator", TitleKey = "Locators", IsSingleInstance = true, Id = "{A6F876C1-32B9-46FD-A4BF-3CF16DD7376E}" });
            BarItem.AddUi(new Command() { Name = "Ui.SysData.CodeProvider", TitleKey = "Code Providers", IsSingleInstance = true, Id = "{D0242FD5-5FF3-499D-BE12-00F326C00FEF}" });

            // Admin
            BarItem = new Command() { TitleKey = "Admin", Type = Tripous.RequestType.Ui, Id = "{AB6BCF40-D5D6-488F-8FBA-A366AE0EDAE0}" };
            Result.Add(BarItem);
            BarItem.AddUi(new Command() { Name = "Ui.Data.Trader", TitleKey = "Traders", IsSingleInstance = true, Id = "{0337F29D-7D5C-422A-89AE-98307B1BE951}" });
 
            App.AddPluginMainMenuCommands(Result);

            return Result.ToArray();

            //return GetMainMenuDemo();
        }

        /* miscs */
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the culture of the current request, e.g. el-GR
        /// </summary>
        static public string Localize(string Key)
        {
            return Localize(App.Culture.Name, Key);
        }
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and a culture code, e.g. el-GR
        /// </summary>
        static public string Localize(string CultureCode, string Key)
        { 
            Language Language = Languages.GetByCultureCode(CultureCode);  
            return Localize(Language, Key);
        }
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and a culture code, e.g. el-GR
        /// </summary>
        static public string Localize(Language Language, string Key)
        {
            EnsureLanguageResources(Language);
            string Result = Language.Resources.Find(Key);
            return !string.IsNullOrWhiteSpace(Result) ? Result : Key;
        }

        /* properties */
        /// <summary>
        /// Returns true when initialized
        /// </summary>
        static public bool Initialized => App != null && SqlStore != null;
        /// <summary>
        /// Represents the web application
        /// </summary>
        static public IWebAppContext App { get; private set; }
        /// <summary>
        /// Returns the default sql store
        /// </summary>
        static public SqlStore SqlStore { get; private set; }
    }
}

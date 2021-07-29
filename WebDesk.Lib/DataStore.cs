using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous;
using Tripous.Logging;
using Tripous.Data;
using Tripous.Web;

namespace WebDesk
{


    /// <summary>
    /// Represents the database
    /// </summary>
    static public partial class DataStore
    {
        /// <summary>
        /// English language
        /// </summary>
        static public Language EnLanguage = new Language() { Id = "ENU", Name = "English", Code = "en", CultureCode = "en-US", FlagImage = "gb.png" };
        /// <summary>
        /// Greek language
        /// </summary>
        static public Language GrLanguage = new Language() { Id = "ELL", Name = "Greek", Code = "el", CultureCode = "el-GR", FlagImage = "gr.png" };
 
        static Language[] StoreLanguages;


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

        /// <summary>
        /// Registers DbProviderFactory classes
        /// </summary>
        static void RegisterDbProviderFactories()
        {
            DbProviderFactories.RegisterFactory("System.Data.SQLite", System.Data.SQLite.SQLiteFactory.Instance);
        }

        /// <summary>
        /// Loads database configuration settings.
        /// </summary>
        static void ConnectDatabases()
        {
            SysConfig.SqlConnectionsFolder = typeof(DataStore).Assembly.GetFolder();
            SqlConnectionInfoList ConnectionInfoList = new SqlConnectionInfoList();

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

            Db.Connections = ConnectionInfoList.SqlConnections;
        }
        /// <summary>
        /// Creates any non-existing creatable database.
        /// </summary>
        static void CreateDatabases()
        {
            SqlConnectionInfo DefaultConnectionInfo = Db.DefaultConnectionInfo;
            SqlProvider Provider = DefaultConnectionInfo.GetSqlProvider();
            string ConnectionString = DefaultConnectionInfo.ConnectionString;

            if (!Provider.DatabaseExists(ConnectionString) && Provider.CanCreateDatabases)
            {
                Provider.CreateDatabase(ConnectionString);
            }
        }
        /// <summary>
        /// Creates database tables etc. based on the registered schemas
        /// </summary>
        static void ExecuteSchemas()
        {
            Schemas.Execute();
        }



        /// <summary>
        /// Initializer
        /// </summary>
        static public void Initialize(IWebAppContext App)
        {
            if (DataStore.App == null)
            {
                DataStore.App = App;

                Logger.Add(new DataLogListener());

                RegisterDbProviderFactories();
                RegisterSchemas();

                ConnectDatabases();
                CreateDatabases();
                ExecuteSchemas();

                SqlStore = SqlStores.CreateDefaultSqlStore();

                EntityDescriptors.Load(typeof(DataStore).Assembly);
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
        /// Returns the list of languages this web-site supports
        /// </summary>
        static public Language[] GetLanguages()
        {
            if (StoreLanguages == null)
            {
                string Sql = SSelectLanguages;
                DataTable Table = SqlStore.Select(Sql);
                List<Language> LanguageList = new List<Language>();
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

                StoreLanguages = LanguageList.ToArray();

                // TODO: load string resources
                // Lang.Resources.LoadFrom(GetResourceStringList(Lang.Id));
            }

            return StoreLanguages;
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
            if (StoreLanguages == null)
                Lib.Error($"Cannot Localize(). No Languages");

            Language Lang = StoreLanguages.FirstOrDefault(item => item.CultureCode.IsSameText(CultureCode));
            if (Lang == null)
                Lib.Error($"Language not found: {CultureCode}");

            string Result = Lang.Resources.Find(Key);

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

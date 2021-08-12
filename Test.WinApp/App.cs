using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Reflection;

using Tripous;
using Tripous.Logging;
using Tripous.Data;

namespace Test.WinApp
{
    static public partial class App
    {
        static MainForm MainForm;

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
        static void ConnectDatabases()
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

        /* public */
        static public void Initialize(MainForm MainForm)
        {
            InitializeSysConfig();

            App.MainForm = MainForm;

            Logger.Add(new LogLineListener(MainForm.AppendLine));
            Logger.Add(new LogFileListener(Path.Combine(SysConfig.AppExeFolder, "Logs")));

            ObjectStore.Initialize();
            Db.Initialize();

            RegisterDbProviderFactories();
            ConnectDatabases();
            CreateDatabases();

            RegisterSchemas();
            ExecuteSchemas();

            SqlStore = SqlStores.CreateDefaultSqlStore();
 
        }

        /* log */
        static public void LogClear()
        {
            MainForm.Clear();
        }
        static public void LogAppend(string Text)
        {
            MainForm.Append(Text);
        }
        static public void LogAppendLine(string Text)
        {
            MainForm.AppendLine(Text);
        }
        static public void Log(string Text = null)
        {
            MainForm.Log(Text);
        }

        static public SqlConnectionInfoList ConnectionInfoList { get; private set; }

        /// <summary>
        /// The Default store
        /// </summary>
        static public SqlStore SqlStore { get; private set; }
    }
}

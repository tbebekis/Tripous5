/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Tripous
{

    /// <summary>
    /// The tripous system configuration and system settings
    /// </summary>
    static public class SysConfig
    {

        /* private */
        static string fApplicationName;
        static string fApplicationTitle;
        static string fSqlConnectionsFolder;
        static string fMainSettingsFolder;
        static string fSolutionName;
        static string fAppRootDataFolder;
        static string fAppDataFolder;
        static string fAppBackUpFolder;
        static string fTempFolder;
 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static SysConfig()
        { 
        }


        /* properties */
        /// <summary>
        /// The mode of the application (Desktop, Web, Service)
        /// </summary>
        static public ApplicationMode ApplicationMode { get; set; } = Tripous.ApplicationMode.Desktop;
        /// <summary>
        /// Gets the assembly of the main executable.
        /// <para> The user has to manually set the main assembly in Compact Framework. Otherwise those properties that use
        /// the main assembly in order to infer various paths will throw exceptions.</para> 
        /// </summary>
        static public Assembly MainAssembly { get; set; } = typeof(SysConfig).Assembly;

        /// <summary>
        /// Gets or sets the CompanyName of the software company that created this application
        /// </summary>
        static public string CompanyName { get; set; } = "Tripous";

        /// <summary>
        /// The application solution name. Defaults to SysConfig.MainAssembly.GetFileName()
        /// <para>NOTE: When a solution consists of more than one executables, those
        /// executables have to reside in the same folder, and all executables have to return
        /// the same Sys.AppRootDataFolder, where setting files are placed.</para>
        /// <para>This property is used in constructing that common Sys.AppRootDataFolder
        /// for all executables of the same solution.</para>
        /// </summary>
        static public string SolutionName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(fSolutionName))
                    return fSolutionName;

                return SysConfig.MainAssembly.GetFileName();
            }
            set { fSolutionName = value; }
        }
        /// <summary>
        /// Gets or sets the ApplicationName of this application. This ApplicationName is used
        /// when sending xml tcp commands.
        /// <para>Defaults to Sys.AppExeName</para>
        /// </summary>
        static public string ApplicationName
        {
            get { return string.IsNullOrEmpty(fApplicationName) ? SysConfig.AppExeName : fApplicationName; }
            set { fApplicationName = value; }
        }
        /// <summary>
        /// Gets or sets the application title. It is a string used for display purposes.
        /// <para>Defaults to SysConfig.ApplicationName.</para>
        /// </summary>
        static public string ApplicationTitle
        {
            get { return string.IsNullOrEmpty(fApplicationTitle) ? ApplicationName : fApplicationTitle; }
            set { fApplicationTitle = value; }
        }

        /// <summary>
        /// Gets the full path for a "special directory" that applications use to store their data.
        /// <para>For desktop Windows that directory is normally the C:\ProgramData\</para>
        /// <para>Net CF: Data in that special directory must remain intact even after a hard reset of the PDA.
        /// So that directory must be in the "right" storage device. Such devices are the additional memory cards
        /// and the Flash File Store directory of the Pocket PCs. If not such a safe directory exists, the method
        /// returns the directory of the main executable assembly.
        /// </para>
        /// </summary>
        static public string MachineRootDataFolder
        {
            get
            {
                string Result = string.Empty;

                // Vista = C:\ProgramData
                // XP = C:\Document Settings\All Users\Application Data
                Result = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                if (!Directory.Exists(Result))
                    Result = AppExeFolder;

                return Result;

            }

        }
        /// <summary>
        /// Gets or sets a directory the application uses to create various sub-directories in order to store data.
        /// <para>NOTE: If AppRootDataFolder is NOT defined the it uses a certain logic in order to return a value, as following:</para>
        /// <para>It uses the MachineRootDataFolder property to get a root directory. </para>
        /// <para>If MachineRootDataFolder equals AppExeFolder then it just returns that directory. </para>
        /// <para>(For Net CF this is the case where no "safe directory" exists, such as an additional memory card.) </para>
        /// <para>Otherwise, it just adds a CompanyName directory, if CompanyName is not empty, and then it adds the AppExeName.</para>
        /// <para></para>
        /// <para>For desktop Windows it may return: C:\ProgramData\CompanyName\AppExeName\ </para>
        /// </summary>
        static public string AppRootDataFolder
        {

            get
            {
                if (!string.IsNullOrWhiteSpace(fAppRootDataFolder))
                    return fAppRootDataFolder;

                if (!Platform.IsWeb)
                {
                    string S = MachineRootDataFolder;
                    if (S.IsSameText(AppExeFolder))
                        return AppExeFolder;

                    if (string.IsNullOrEmpty(SysConfig.CompanyName))
                        return Path.Combine(MachineRootDataFolder, SysConfig.SolutionName);
                    else
                        return Path.Combine(MachineRootDataFolder, SysConfig.CompanyName, SysConfig.SolutionName);
                }
                else
                {
                    return Path.Combine(AppExeFolder, "App_Data");
                }

            }
            set { fAppRootDataFolder = value; }
        }
        /// <summary>
        /// The data folder of this application.
        /// <para></para>
        /// <para>By default, for desktop Windows it may return: C:\ProgramData\CompanyName\AppExeName\Data\ </para>
        /// </summary>
        static public string AppDataFolder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fAppDataFolder))
                {
                    fAppDataFolder = Platform.IsWeb? AppRootDataFolder : Path.Combine(AppRootDataFolder, "Data");
                }

                return fAppDataFolder;
            }
            set
            {
                fAppDataFolder = value;
            }         
        }
 
        /// <summary>
        /// The backup folder of this application.
        /// <para></para>
        /// <para>By default, gor desktop Windows it may return: C:\ProgramData\CompanyName\BackUp\ </para>
        /// </summary>
        static public string AppBackUpFolder 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fAppDataFolder))
                {
                    fAppBackUpFolder = Path.Combine(AppRootDataFolder, "BackUp");
                }

                return fAppBackUpFolder;
            }
            set
            {
                fAppBackUpFolder = value;
            }
        }
        /// <summary>
        /// Gets the directory where the main assembly resides.
        /// <para>The returned string includes a trailing path separator.</para>
        /// </summary>
        static public string AppExeFolder { get { return SysConfig.MainAssembly.GetFolder(); } }
        /// <summary>
        /// Gets or sets the full path to a temporary folder
        /// </summary>
        static public string TempFolder
        {
            get
            {
                string Result = fTempFolder;
                if (string.IsNullOrEmpty(Result))
                {
                    Result = Path.GetTempPath();
                    if (!string.IsNullOrEmpty(SysConfig.CompanyName))
                        Result = Path.Combine(Result, SysConfig.CompanyName);
                    Result = Path.Combine(Result, SysConfig.ApplicationName);
                }
 
                if (!Directory.Exists(Result))
                    Directory.CreateDirectory(Result);

                return Result;
            }
            set { fTempFolder = value; }
        }

        /// <summary>
        /// Gets the full path to the main executable assembly.
        /// </summary>
        static public string AppExePath { get { return SysConfig.MainAssembly.GetFullPath(); } }
        /// <summary>
        /// Gets the filename to the main executable assembly without path and extension.
        /// </summary>
        static public string AppExeName { get { return SysConfig.MainAssembly.GetFileName(); } }
 
        /// <summary>
        /// The profiles filename (not path) where database connection information is kept.
        /// <para>Defaults to SqlConnections.json</para>
        /// </summary>
        static public string SqlConnectionsFileName { get; set; } = "SqlConnections.json";
        /// <summary>
        /// The folder where the SqlConnections.json is found
        /// </summary>
        static public string SqlConnectionsFolder
        {
            get { return string.IsNullOrWhiteSpace(fSqlConnectionsFolder) ? SysConfig.AppExeFolder : fSqlConnectionsFolder; }
            set { fSqlConnectionsFolder = value; }
        }
        /// <summary>
        /// The full path to the SqlConnections.json file
        /// </summary>
        static public string SqlConnectionsFilePath { get { return Path.Combine(SysConfig.SqlConnectionsFolder, SysConfig.SqlConnectionsFileName); } }

        /// <summary>
        /// The main settings json file name
        /// <para>Defaults to MainSettings.json</para>
        /// </summary>
        static public string MainSettingsFileName { get; set; } = "MainSettings.json";
        /// <summary>
        /// The folder where the MainSettings.json file is found
        /// </summary>
        static public string MainSettingsFolder
        {
            get { return string.IsNullOrWhiteSpace(fMainSettingsFolder) ? SysConfig.AppRootDataFolder : fMainSettingsFolder; }
            set { fMainSettingsFolder = value; }
        }
        /// <summary>
        /// The full path to the MainSettings.json file  
        /// </summary>
        static public string MainSettingsFilePath { get { return Path.Combine(SysConfig.MainSettingsFolder, SysConfig.MainSettingsFileName); } }


        /// <summary>
        /// Contains the starting part of assembly names.
        /// <para>Assemblies found in this list, are excluded by the ObjectStore registration searching.</para>
        /// <para>Defaults to System, Microsoft, mscorlib and vshost. </para>
        /// </summary>
        static public List<string> ObjectStoreExcludedAssemblies { get; private set; } = new List<string>(new string[] { "System", "Microsoft", "mscorlib", "vshost" });
        /// <summary>
        /// When true, then those static methods marked by the InitializerAttribute
        /// are executed immediately as they discovered by ObjectStore.
        /// <para>When false, the default, an explicit call to ObjectStore.InvokeInitializers is required.</para>
        /// <para>Defalult to false.</para>
        /// </summary>
        static public bool ObjectStoreAutoInvokeInitializers { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the how a Date value should be formatted as a string 
        /// <para>Defaults to DateTimeFormatType.Date </para>
        /// </summary>
        static public DateTimeFormatType DateFormat { get; set; } = DateTimeFormatType.Date;
        /// <summary>
        /// Gets or sets the how a DateTime value should be formatted as a string 
        /// <para>Defaults to DateTimeFormatType.DateTime24 </para>
        /// </summary>
        static public DateTimeFormatType DateTimeFormat { get; set; } = DateTimeFormatType.DateTime24;
        /// <summary>
        /// Gets or sets the how a Time value should be formatted as a string 
        /// <para>Defaults to DateTimeFormatType.Time24 </para>
        /// </summary>
        static public DateTimeFormatType TimeFormat { get; set; } = DateTimeFormatType.Time24;

        /* miscs */
        /// <summary>
        /// When true, then user accounts are enabled for this application.
        /// Otherwise the UserNameDefault and PasswordDefault is used
        /// for all users.
        /// <para>Defaults to true.</para>
        /// </summary>
        static public bool UsersEnabled { get; set; }
        /// <summary>
        /// When is set indicates that the Oids are Guid strings.  
        /// <para>Defaults to true.</para>
        /// </summary>
        static public bool GuidOids { get; set; } = true;
        /// <summary>
        /// An encryption key.
        /// <para>Defaults to tripous</para>
        /// </summary>
        static public string EnKey1 { get; set; } = "tripous";
        /// <summary>
        /// A limit to apply when selecting from databases in order to display in browser forms or views.
        /// <para>Defaults to 400</para>
        /// </summary>
        static public int SelectSqlRowsLimit { get; set; } = 400;
        /// <summary>
        /// When true, a MachineId entry should be created 
        /// into the application ini.
        /// <para>Defaults to false.</para>
        /// </summary>
        static public bool MachineIdRequired { get; set; }
        /// <summary>
        /// The script language used by the scripting system.
        /// <para>Defaults to ScriptNet</para>
        /// </summary>
        static public string ScriptLanguage { get; set; } = "";
        /// <summary>
        /// Gets the variables sql prefix. Defaults to :@
        /// </summary>
        static public string VariablesPrefix { get; set; } = ":@";
        
        /// <summary>
        /// The field name of the company field, used in various tables. 
        /// <para>Defaults to CompanyId</para>
        /// </summary>
        static public string CompanyFieldName { get; set; } = "CompanyId";
        /// <summary>
        /// The Id of the current company, if any, else null.
        /// </summary>
        static public object CompanyId { get; set; } 
        /// <summary>
        /// ReadOnly. Returns the value of the CompanyId as a string for constructing Sql statements.
        /// </summary>
        static public string CompanyIdSql
        {
            get
            {

                if (CompanyId == null)
                {
                    if (GuidOids)
                        return Sys.StandardCompanyGuid.QS();
                    return "-1";
                }

                Type T = CompanyId.GetType();

                if ((T == typeof(System.String)) || (T == typeof(System.Guid)))
                    return CompanyId.ToString().QS();
                else
                    return CompanyId.ToString();
            }

        }
        /// <summary>
        /// ReadOnly. Returns the value of the CompanyId
        /// </summary>
        static public object CompanyIdValue
        {
            get
            {
                if (CompanyId == null)
                { 
                    return GuidOids? (object)Sys.StandardCompanyGuid: -1;
                }
                else
                {
                    return CompanyId;
                }
            }
        }

        /// <summary>
        /// The folder where the external modules reside
        /// </summary>
        static public string ExternalModulesFolder { get; set; }

        /// <summary>
        /// The name of the default database connection
        /// </summary>
        static public string DefaultConnection { get; set; } = Sys.DEFAULT;

        /* read-only */
        /// <summary>
        /// Gets the url slash, ie /
        /// </summary>
        static public string UrlSlash { get { return '/'.ToString(); } }

 

        /// <summary>
        /// Gets the default SimpleType data type for Id fields, based on the GuidOids setting in the Variables
        /// </summary>
        static public SimpleType OidDataType { get { return GuidOids ? SimpleType.String : SimpleType.Integer; } }
        /// <summary>
        /// Gets the size of a field for  the default SimpleType data type for Id fields
        /// </summary>
        static public int OidSize { get { return OidDataType == SimpleType.String ? 40 : 0; } }
        /// <summary>
        /// A string to be used for primary keys when formating CREATE TABLE statements
        /// </summary>
        static public string PrimaryKeyStr { get { return SysConfig.GuidOids ? "@NVARCHAR(40)    @NOT_NULL primary key" : "@PRIMARY_KEY"; } }
        /// <summary>
        ///  A string to be used for foreign keys when formating CREATE TABLE statements
        /// </summary>
        static public string ForeignKeyStr { get { return SysConfig.GuidOids ? "@NVARCHAR(40)" : "integer"; } }
    }

}
/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Helper config class with the system table names
    /// </summary>
    static public class SysTables
    {
        const string SIni = "Ini";
        const string SLog = "Log";
        const string SLang = "Lang";
        const string SStrRes = "StrRes";
        const string SData = "Data";
        const string SSmtpProvider = "SmtpProvider";

        /// <summary>
        /// Default prefix for system table names
        /// </summary>
        public const string DefaultPrefix = "SYS_";

        static Dictionary<string, string> Names = new Dictionary<string, string>();
 
        static string RemovePrefix(string Name)
        {
            if (Name.StartsWith(Prefix))
                Name = Name.Remove(0, Prefix.Length);
            return Name;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static SysTables()
        {
            Ini = "INI";
            Lang = "LANG";
            Log = "LOG";
            Data = "DATA";            
            StrRes = "STR_RES";
            SmtpProvider = "SMTP_PROVIDER";
        }

        /* system schema */
        /// <summary>
        /// Adds the database schema of a system table to version #1 of system <see cref="Schema"/>.
        /// </summary>
        static public void AddSchemaLog()
        {

            Schema Schema = Schemas.GetSystemSchema();
            SchemaVersion schema = Schema.FindOrAdd(Version: 1);

            /* SYS_LOG */
            string SqlText = $@" 
create table {SysTables.Log}  (
   Id                     {SysConfig.PrimaryKeyStr()}
  ,LogDate                @DATE            @NULL
  ,LogTime                @NVARCHAR(12)    @NULL
  ,UserName               @NVARCHAR(96)    @NULL
  ,Host                   @NVARCHAR(64)    @NULL
  ,LogLevel               @NVARCHAR(24)    @NULL     
  ,LogSource              @NVARCHAR(96)    @NULL
  ,ScopeId                @NVARCHAR(96)    @NULL
  ,EventId                @NVARCHAR(96)    @NULL
  ,Data                   @NBLOB_TEXT      @NULL
)
";

            schema.AddTable(SqlText);

            SqlText = $"create index IDX_{SysTables.Log}_00 on {SysTables.Log}(LogDate) ";
            schema.AddStatementAfter(SqlText);
        }
        /// <summary>
        /// Adds the database schema of a system table to version #1 of system <see cref="Schema"/>.
        /// </summary>
        static public void AddSchemaLang()
        {

            Schema Schema = Schemas.GetSystemSchema();
            SchemaVersion schema = Schema.FindOrAdd(Version: 1);

            /*  SYS_LANG  */
            string SqlText = $@"
create table {SysTables.Lang} (
    Id                      {SysConfig.PrimaryKeyStr()}
   ,Name                    @NVARCHAR(40)        @NOT_NULL    
   ,Code                    @NVARCHAR(40)        @NOT_NULL    
   ,CultureCode             @NVARCHAR(40)        @NOT_NULL                  
   ,FlagImage               @NVARCHAR(40)        @NULL
   ,IsActive                integer default 1    @NOT_NULL
   ,DisplayOrder            integer default 0    @NOT_NULL                     
 )
";

            schema.AddTable(SqlText);

            SqlText = $@"insert into {SysTables.Lang} (Id, Name, Code, CultureCode, FlagImage) values ('{Sys.EnId}', 'English', 'en', 'en-US', 'gb.png') ";
            schema.AddStatementAfter(SqlText);

            SqlText = $@"insert into {SysTables.Lang} (Id, Name, Code, CultureCode, FlagImage) values ('{Sys.GrId}', 'Greek', 'gr', 'el-GR', 'gr.png') ";
            schema.AddStatementAfter(SqlText);
        }
        /// <summary>
        /// Adds the database schema of a system table to version #1 of system <see cref="Schema"/>.
        /// </summary>
        static public void AddSchemaStrRes()
        {

            Schema Schema = Schemas.GetSystemSchema();
            SchemaVersion schema = Schema.FindOrAdd(Version: 1);

            /*  SYS_STR_RES  */
            string SqlText = $@"
create table {SysTables.StrRes} (
    Id                      {SysConfig.PrimaryKeyStr()} 
   ,LanguageId              {SysConfig.ForeignKeyStr()} @NULL                 
   ,TableName               @NVARCHAR(96)        @NULL           
   ,TableId                 {SysConfig.ForeignKeyStr()} @NULL           
   ,EntryKey                @NVARCHAR(96)        @NOT_NULL		 
   ,EntryValue              @NBLOB_TEXT          @NOT_NULL                         
 )
";

            schema.AddTable(SqlText);
        }
        /// <summary>
        /// Adds the database schema of a system table to version #1 of system <see cref="Schema"/>.
        /// </summary>
        static public void AddSchemaData()
        {

            Schema Schema = Schemas.GetSystemSchema();
            SchemaVersion schema = Schema.FindOrAdd(Version: 1);

            //string CompanyDataType = SysConfig.GuidOids ? "@NVARCHAR(40) default '' " : "integer default -1 ";

            /* SYS_DATA */
            string SqlText = $@"
create table {SysTables.Data}  (                                                                                      
    Id                  {SysConfig.PrimaryKeyStr()}
    
    ,DataType           @NVARCHAR(96)   @NOT_NULL
    ,DataName           @NVARCHAR(96)   @NOT_NULL    

    ,TitleKey           @NVARCHAR(96)   @NOT_NULL
    ,Notes              @NVARCHAR(255)  @NULL

    ,Owner              @NVARCHAR(96)   @NULL

    ,Tag1               @NVARCHAR(96)   @NULL
    ,Tag2               @NVARCHAR(96)   @NULL
    ,Tag3               @NVARCHAR(96)   @NULL
    ,Tag4               @NVARCHAR(96)   @NULL

    ,Data1              @BLOB_TEXT      @NULL
    ,Data2              @BLOB_TEXT      @NULL
    ,Data3              @BLOB_TEXT      @NULL
    ,Data4              @BLOB_TEXT      @NULL

    ,constraint UC_{SysTables.Data}_00 unique (DataType, DataName)
)
";

            schema.AddTable(SqlText);
        }
        /// <summary>
        /// Returns the full SELECT statement for the system Data table.
        /// <para>Blob selection is controlled by the NoBlobs flag</para>
        /// </summary>
        static public string GetSystemDataSelectStatement(bool NoBlobs)
        {
            string NoBlobsSql = $@"
select
     Id            

    ,DataType 
    ,DataName

    ,TitleKey  
    ,Notes    

    ,Owner

    ,Tag1             
    ,Tag2             
    ,Tag3             
    ,Tag4        
from
    {SysTables.Data} 
";



            string FullSql = $@"
select
    * 
from
    {SysTables.Data} 
";


            return NoBlobs ? NoBlobsSql : FullSql;

        }
        /// <summary>
        /// Adds the database schema of a system table to version #1 of system <see cref="Schema"/>.
        /// </summary>
        static public void AddSchemaSmtpProvider()
        {

            Schema Schema = Schemas.GetSystemSchema();
            SchemaVersion schema = Schema.FindOrAdd(Version: 1);

            /* SYS_SMTP_PROVIDER */
            string SqlText = $@"
create table {SysTables.SmtpProvider} (
     Id						{SysConfig.PrimaryKeyStr()}
    ,Host                   @NVARCHAR(96)        @NOT_NULL  
    ,Port                   integer default 25   @NOT_NULL  
    ,UserName               @NVARCHAR(96)        @NULL 
    ,SenderAddress          @NVARCHAR(96)        @NULL 
    ,Psw                    @NVARCHAR(255)       @NULL
    ,EnableSsl              integer default 0    @NOT_NULL
    ,MessagesPerMinute      integer default -1   @NOT_NULL                    
    
    ,constraint UK_{SysTables.SmtpProvider}_00 unique (Host)
)
";
            schema.AddTable(SqlText);
        }
        /// <summary>
        /// Adds the database schema of all system tables to version #1 of system <see cref="Schema"/>.
        /// </summary>
        static public void AddSchemaAll()
        {
            AddSchemaLog();
            AddSchemaLang();
            AddSchemaStrRes();
            AddSchemaData();
            AddSchemaSmtpProvider();
        }



        /* system table names */
        /// <summary>
        /// Gets the name of SYS_INI system table
        /// <para>Defaults to SYS_INI</para>
        /// </summary>
        static public string Ini
        {
            get { return Prefix + Names[SIni]; }
            set { Names[SIni] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_LOG system table
        /// <para>Defaults to SYS_LOG</para>
        /// </summary>
        static public string Log
        {
            get { return Prefix + Names[SLog]; }
            set { Names[SLog] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_LANG system table
        /// <para>Defaults to SYS_LANG</para>
        /// </summary>
        static public string Lang
        {
            get { return Prefix + Names[SLang]; }
            set { Names[SLang] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_STR_RES system table
        /// <para>Defaults to SYS_STR_RES</para>
        /// </summary>
        static public string StrRes
        {
            get { return Prefix + Names[SStrRes]; }
            set { Names[SStrRes] = RemovePrefix(value); }
        }
        /// <summary>
        /// Gets the name of SYS_DATA system table
        /// <para>Defaults to SYS_DATA</para>
        /// </summary>
        static public string Data
        {
            get { return Prefix + Names[SData]; }
            set { Names[SData] = RemovePrefix(value); }
        } 
        /// <summary>
        /// Gets the name of SYS_SMTP_PROVIDER system table
        /// <para>Defaults to SYS_SMTP_PROVIDER</para>
        /// </summary>
        static public string SmtpProvider
        {
            get { return Prefix + Names[SSmtpProvider]; }
            set { Names[SSmtpProvider] = RemovePrefix(value); }
        }

        /* miscs */
        /// <summary>
        /// Gets or sets the string used as a prefix
        /// when constructing system table nick-names
        /// </summary>
        static public string Prefix { get; set; } = DefaultPrefix;
        /// <summary>
        /// Returns a dictionary with system table names where Keys are the property names of this class.
        /// </summary>
        static public Dictionary<string, string> GetDictionary()
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            foreach (var Entry in Names)
            {
                Result[Entry.Key] = Entry.Value;
            }

            return Result;
        }
    }
}

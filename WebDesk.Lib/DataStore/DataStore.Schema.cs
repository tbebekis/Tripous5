using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;

using WebDesk.Models;

namespace WebDesk
{
    static public partial class DataStore
    {
        static string CompanyDataType = SysConfig.GuidOids ? "@NVARCHAR(40)    @NOT_NULL" : "integer default -1 @NOT_NULL";

        const string EnumLookUpSql = @"
create table {0} (
   Id                     {1}                  @NOT_NULL primary key
  ,Name                   @NVARCHAR(96)        @NOT_NULL      

  ,constraint UK_{0}_00 unique (Name)           
)
";

        const string EnumLookUpWithErpIdSql = @"
create table {0} (
   Id                     {1}                  @NOT_NULL primary key
  ,Name                   @NVARCHAR(96)        @NOT_NULL     
  ,ErpId                  @NVARCHAR(96)        @NULL  

  ,constraint UK_{0}_00 unique (Name)           
)
";

        /// <summary>
        /// Creates an "enum" (integer look-up) table
        /// </summary>
        static public void AddEnum(SchemaVersion schema, string TableName, bool HasErpId = false)
        {
            string S = HasErpId ? EnumLookUpWithErpIdSql : EnumLookUpSql;

            S = string.Format(S, TableName, "integer");

            schema.AddTable(S);
        }
        /// <summary>
        /// Creates an "enum" (integer look-up) table and INSERTs the EnumType values to it.
        /// </summary>
        static public void AddEnum(SchemaVersion schema, string TableName, Type EnumType, bool HasErpId = false)
        {
            AddEnum(schema, TableName, HasErpId);

            string SqlText = "insert into {0} (Id, Name) values ({1}, '{2}')";
            string S2;

            Array A = Enum.GetValues(EnumType);
            for (int i = 0; i < A.Length; i++)
            {
                S2 = Sys.SplitCamelCase(A.GetValue(i).ToString());
                S2 = string.Format(SqlText, TableName, (int)A.GetValue(i), S2);
                schema.AddStatementAfter(S2);
            }

        }
        /// <summary>
        /// Creates look-up table
        /// </summary>
        static public void AddLookUp(SchemaVersion schema, string TableName, bool HasErpId = false)
        {
            string S = HasErpId ? EnumLookUpWithErpIdSql : EnumLookUpSql;

            S = string.Format(S, TableName, "@NVARCHAR(40)");

            schema.AddTable(S);
        }


        static void RegisterSystemSchema_01()
        {
            string SqlText;

            Schema Schema = Schemas.GetSystemSchema();             
            SchemaVersion schema = Schema.Add(Version: 1);

            /* Company */
            SqlText = $@"
create table {SysTables.Company}  (
   Id                   {SysConfig.PrimaryKeyStr()}    
  ,Name                 @NVARCHAR(96)    @NOT_NULL

  ,constraint UC_{SysTables.Company}_00 unique (Name)
)
";

            schema.AddTable(SqlText);

            if (SysConfig.GuidOids)
            {
                SqlText = $"insert into {SysTables.Company} (Id, Name) values ({SysConfig.CompanyIdSql}, 'Default') ";
                schema.AddStatementAfter(SqlText);
            }

            /*  SYS_LANG  */
            SqlText = $@"
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


            /* SYS_LOG */
            SqlText = $@" 
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


            /* SYS_DATA */
            SqlText = $@"
create table {SysTables.Data}  (                                                                                      
   Id                  {SysConfig.PrimaryKeyStr()}
  ,@COMPANY_ID         {CompanyDataType}

  ,DataName            @NVARCHAR(96)   @NOT_NULL
  ,Title               @NVARCHAR(96)   @NOT_NULL
  ,DataType            @NVARCHAR(96)   @NOT_NULL
  ,StoreName           @NVARCHAR(64)   @NOT_NULL
  ,Notes               @NVARCHAR(255)  @NULL

  ,Category1           @NVARCHAR(64)   @NULL
  ,Category2           @NVARCHAR(64)   @NULL

  ,Data1               @BLOB           @NULL
  ,Data2               @BLOB           @NULL
  ,Data3               @BLOB           @NULL
  ,Data4               @BLOB           @NULL

  ,constraint UC_{SysTables.Data}_00 unique (@COMPANY_ID, DataType, DataName)
)
";

            schema.AddTable(SqlText);


            /*  SYS_STR_RES  */
            SqlText = $@"
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


            /* SYS_SMTP_PROVIDER */
            SqlText = $@"
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
        static void RegisterSchema_01()
        {

            string SqlText;
            string TableName; 

            Schema Schema = Schemas.GetApplicationSchema();
            SchemaVersion schema = Schema.Add(Version: 1);
 

            /* AppUser */
            TableName = "AppUser";
            SqlText = $@"
create table {TableName} (
     Id						{SysConfig.PrimaryKeyStr()}
    ,UserId                 @NVARCHAR(96)        @NOT_NULL 
    ,Email                  @NVARCHAR(96)        @NOT_NULL 
    ,Name                   @NVARCHAR(96)        @NULL
    ,Level                  integer default 0    @NOT_NULL    
    ,IsBlocked              integer default 0    @NOT_NULL    
 
    ,LastAccessDT           @DATE_TIME          @NULL 
    ,LastLoginDT            @DATE_TIME          @NULL
    ,RegistrationDT         @DATE_TIME          @NULL

    ,IsActivated            integer default 0   @NOT_NULL
    ,ActivationToken        @NVARCHAR(96)       @NULL

    ,Password               @NVARCHAR(96)       @NULL
    ,PasswordSalt           @NVARCHAR(20)       @NULL
    ,PassRecoveryDT         @DATE_TIME          @NULL
    ,PassRecoveryToken      @NVARCHAR(96)       @NULL    

    ,constraint UK_{TableName}_00 unique (UserId)
)   
";
            schema.AddTable(SqlText);

            string Id = Sys.GenId(true);
            int UserLevel = (int)Tripous.UserLevel.Admin;
            string PasswordSalt = GenerateRandomText(8);
            string Password = "webdesk";
            Password = GeneratePasswordHash(Password, PasswordSalt);

            SqlText = $@"
insert into {TableName} (
    Id,
    UserId,
    Email,
    Name, 
    Level,
    IsActivated, 
    Password,
    PasswordSalt
) values (
    '{Id}', 
    'teo', 
    'tbebekis@antyxsoft.com',
    'Theo Bebekis',
    {UserLevel},
    1,
    '{Password}',
    '{PasswordSalt}'  
 ) 
";
            // __ID__
            //SqlText = SqlText.Replace("__ID__", Id);


            schema.AddStatementAfter(SqlText);



 

            /* Product */
            TableName = "Product";
            SqlText = $@"
create table {TableName} (
     Id						{SysConfig.PrimaryKeyStr()}
    ,Name                   @NVARCHAR(96)               @NOT_NULL
    ,Price                  @DECIMAL default 0          @NOT_NULL  
)   
";
            schema.AddTable(SqlText);

            /* Trader */
            TableName = "Trader";
            SqlText = $@"
create table {TableName} (
     Id						{SysConfig.PrimaryKeyStr()}
    ,Name                   @NVARCHAR(96)        @NULL
)   
";
            schema.AddTable(SqlText);


            /* TradeType */
            TableName = "TradeType";
            AddEnum(schema, TableName, typeof(TradeType));


            /* Trade */
            TableName = "Trade";
            SqlText = $@"
create table {TableName} (
     Id						{SysConfig.PrimaryKeyStr()}
    ,TradeTypeId            integer                     @NOT_NULL    
    ,TradeDate              @DATE_TIME                  @NOT_NULL 
    ,TraderId               {SysConfig.ForeignKeyStr()}   @NOT_NULL    
    ,TotalAmount            @DECIMAL                    @NOT_NULL    

    ,constraint FK_{TableName}_00 foreign key (TradeTypeId) references TradeType (Id)
    ,constraint FK_{TableName}_01 foreign key (TraderId) references Trader (Id)
)   
";
            schema.AddTable(SqlText);

            /* TradeLine */
            TableName = "TradeLine";
            SqlText = $@"
create table {TableName} (
     Id						{SysConfig.PrimaryKeyStr()}
    ,TradeId                {SysConfig.ForeignKeyStr()}   @NOT_NULL    
    ,ProductId              {SysConfig.ForeignKeyStr()}   @NOT_NULL    
    ,Qty                    @DECIMAL                    @NOT_NULL    
    ,Price                  @DECIMAL                    @NOT_NULL    
    ,LineAmount             @DECIMAL                    @NOT_NULL    

    ,constraint FK_{TableName}_00 foreign key (TradeId) references Trade (Id)
    ,constraint FK_{TableName}_01 foreign key (ProductId) references Product (Id)   
)   
";
            schema.AddTable(SqlText);

        }


        /// <summary>
        /// Register database schemas
        /// </summary>
        static void RegisterSchemas()
        {
            RegisterSystemSchema_01();

            RegisterSchema_01();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Tripous;
using Tripous.Data;

using WebDesk.Models;

namespace WebDesk
{
    static public partial class DataStore
    {
        static DataView vTables;


        const string EnumLookUpSql = @"
create table {0} (
   Id                     {1}                  @NOT_NULL primary key
  ,Name                   @NVARCHAR(96)        @NOT_NULL      

  ,constraint UK_{0}_00 unique (Name)           
)
";

        /// <summary>
        /// Creates look-up table
        /// </summary>
        static public void AddLookUp(SchemaVersion schema, string TableName)
        {
            string S = string.Format(EnumLookUpSql, TableName, "@NVARCHAR(40)");
            schema.AddTable(S);
        }
        /// <summary>
        /// Creates an "enum" (integer look-up) table
        /// </summary>
        static public void AddEnum(SchemaVersion schema, string TableName)
        {
            string S = string.Format(EnumLookUpSql, TableName, "integer");
            schema.AddTable(S);
        }
        /// <summary>
        /// Creates an "enum" (integer look-up) table and INSERTs the EnumType values to it.
        /// </summary>
        static public void AddEnum(SchemaVersion schema, string TableName, Type EnumType)
        {
            AddEnum(schema, TableName);

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

        static bool TableExists(string TableName)
        {
            return vTables.Find(TableName) != -1;                
        }


        static void RegisterSchema_01()
        {

            string SqlText;
            string TableName; 

            Schema Schema = Schemas.GetApplicationSchema();
            SchemaVersion schema = Schema.FindOrAdd(Version: 1);
 

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
            //RegisterSystemSchema_01();

            //RegisterSchema_01();
        }

        static void Execute_AppUser()
        {
            string TableName = "AppUser";
            if (TableExists(TableName))
                return;       

            DataTableDef Table = new DataTableDef() { Name = TableName };

            Table.AddPrimaryKey();
            Table.AddStringField("UserId", 96, true);
            Table.AddStringField("Email", 96, true);
            Table.AddStringField("Name", 96, false);
            Table.AddIntegerField("Level", true, null, "0");
            Table.AddIntegerField("IsBlocked", true, null, "0");

            Table.AddDateTimeField("LastAccessDT");
            Table.AddDateTimeField("LastLoginDT");
            Table.AddDateTimeField("RegistrationDT");

            Table.AddIntegerField("IsActivated", true, null, "0");
            Table.AddStringField("ActivationToken", 96, false);

            Table.AddStringField("Password", 96, false);
            Table.AddStringField("PasswordSalt", 20, false);
            Table.AddDateTimeField("PassRecoveryDT");
            Table.AddStringField("PassRecoveryToken", 96, false);

            string SqlText = Table.GetDefText();

            SchemaVersion SV = new SchemaVersion();

            SV.AddTable(SqlText);

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
    'admin', 
    'tbebekis@antyxsoft.com',
    'WebDesk Admin',
    {UserLevel},
    1,
    '{Password}',
    '{PasswordSalt}'  
 ) 
";

            SV.AddStatementAfter(SqlText);

            SV.Execute();

            SysDataItem SDI = Table.ToSysDataItem(SSysDataOwnerName);
            SysData.Save(SDI);
        }

        /// <summary>
        /// Creates database tables etc. based on the registered schemas
        /// </summary>
        static void ExecuteSchemas()
        {

            string SqlText = SysTables.GetSystemDataSelectStatement(NoBlobs: true);
            SqlText += $@"
where
    DataType = 'Table'
    and Owner = 'App'
";
            DataTable Table = SqlStore.Select(SqlText);
            Table.DefaultView.Sort = "DataName";
            
            vTables = Table.DefaultView;

            Execute_AppUser();
           // Schemas.Execute();
        }
    }
}

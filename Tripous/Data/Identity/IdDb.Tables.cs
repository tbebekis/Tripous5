using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Tripous.Identity
{
 


    /// <summary>
    /// Helper
    /// </summary>
    static public partial class IdDb
    {
        private class TableInfo
        {

            void ConstructParameterizedSqlStatements()
            {
                PropertyInfo[] Props = EntityType.GetProperties();

                /* field lists preparation */
                string SqlText;

                StringBuilder InsertFields = new StringBuilder();                // insert field list
                StringBuilder InsertParams = new StringBuilder();               // insert params field list
                StringBuilder UpdateFields = new StringBuilder();

                // prepare field and param lists
                InsertFields.AppendLine();
                InsertParams.AppendLine();
                UpdateFields.AppendLine();

                foreach (var Prop in Props)
                {
                    InsertFields.AppendLine($"  {Prop.Name}, ");
                    InsertParams.AppendLine($"  {":" + Prop.Name}, ");

                    if (Prop.Name == "Id")
                    {
                        UpdateFields.AppendLine($"  {Prop.Name} = {":" + Prop.Name}, ");
                    }
                }

                // remove last comma and space
                TrimLastCommaAndSpace(InsertFields);
                TrimLastCommaAndSpace(InsertParams);
                TrimLastCommaAndSpace(UpdateFields);

                string PrimaryKeyWhere = " where Id = :Id";

                /* INSERT */
                SqlText = "insert into {0} ( {1}" + Environment.NewLine + " ) values ( {2}" + Environment.NewLine + " ) ";
                InsertSql = string.Format(SqlText, this.TableName, InsertFields.ToString(), InsertParams.ToString());

                /* UPDATE */
                SqlText = "update {0} " + Environment.NewLine + "set {1} " + Environment.NewLine + PrimaryKeyWhere;
                UpdateSql = string.Format(SqlText, this.TableName, UpdateFields.ToString());

                /* DELETE */
                SqlText = "delete from {0} " + Environment.NewLine + PrimaryKeyWhere;
                DeleteSql = string.Format(SqlText, this.TableName);

                /* SELECT */
                SqlText = "select * from {0} " + Environment.NewLine + PrimaryKeyWhere;
                SelectRowSql = string.Format(SqlText, this.TableName);
            }
            
            public TableInfo(Type EntityType, string TableName)
            {
                this.EntityType = EntityType;
                this.TableName = TableName;

                ConstructParameterizedSqlStatements();
            }

            public override string ToString()
            {
                return this.TableName;
            }

            public Type EntityType { get; }
            public string TableName { get; }
 

            public string InsertSql { get; private set; }
            public string UpdateSql { get; private set; }
            public string DeleteSql { get; private set; }
            public string SelectRowSql { get; private set; }
        }

        /// <summary>
        /// Constant
        /// </summary>
        public const string ID = "__ID__";

        /// <summary>
        /// Constant
        /// </summary>
        public const string UserTableName = "RBAC_User";
        /// <summary>
        /// Constant
        /// </summary>
        public const string RoleTableName = "RBAC_Role";
        /// <summary>
        /// Constant
        /// </summary>
        public const string PermissionTableName = "RBAC_Permission";
        /// <summary>
        /// Constant
        /// </summary>
        public const string UserRolesTableName = "RBAC_UserRoles";
        /// <summary>
        /// Constant
        /// </summary>
        public const string RolePermissionsTableName = "RBAC_RolePermissions";
        /// <summary>
        /// Constant
        /// </summary>
        public const string UserLoginTableName = "RBAC_UserLogin";
        /// <summary>
        /// Constant
        /// </summary>
        public const string UserClaimsTableName = "RBAC_UserClaims";
        /// <summary>
        /// Constant
        /// </summary>
        public const string UserTokensTableName = "RBAC_UserTokens";
        /// <summary>
        /// Constant
        /// </summary>
        public const string RoleClaimsTableName = "RBAC_RoleClaims";

        /* create table */
        static readonly string UserCreateTableSql = string.Format(@"
create table {0} (
     Id                     nvarchar(40) not null primary key    
    ,Name                   nvarchar(96)
    ,UserName               nvarchar(96) not null
    ,NormalizedUserName     nvarchar(256) not null
    ,Password               nvarchar(512) not null
    ,PasswordSalt           nvarchar(96)
    ,Email                  nvarchar(96)
    ,NormalizedEmail        nvarchar(96)
    ,Phone                  nvarchar(32)
    ,EmailConfirmed         int not null default 0
    ,PhoneConfirmed         int not null default 0

    ,AccessFailedCount      int not null default 0
    ,LockoutEnabled         int not null default 0
    ,LockoutEndUtc          nvarchar(40)
    ,TwoFactorEnabled       int not null default 0

    ,constraint UC_{0} unique (UserName)
)
", UserTableName);
        static readonly string RoleCreateTableSql = string.Format(@"
create table {0} (
     Id         nvarchar(40) not null primary key    
    ,Name       nvarchar(96)
    ,NormalizedName     nvarchar(256) not null

    ,constraint UC_{0} unique (Name)
)
", RoleTableName);
 
        static readonly string PermissionCreateTableSql = string.Format(@"
create table {0} (
     Id         nvarchar(40) not null primary key    
    ,Name       nvarchar(96)

    ,constraint UC_{0} unique (Name)
)
", PermissionTableName);

        static readonly string UserRolesCreateTableSql = string.Format(@"
create table {0} (
     Id         nvarchar(40) not null primary key    
    ,UserId     nvarchar(40) not null
    ,RoleId     nvarchar(40) not null

    ,constraint UC_{0} unique (UserId, RoleId)
    ,constraint FK_{0}_0 foreign key (UserId) references {1}(Id)
    ,constraint FK_{0}_1 foreign key (RoleId) references {2}(Id)
)
", UserRolesTableName, UserTableName, RoleTableName);
        static readonly string RolePermissionsCreateTableSql = string.Format(@"
create table {0} (
     Id             nvarchar(40) not null primary key        
    ,RoleId         nvarchar(40) not null
    ,PermissionId   nvarchar(40) not null

    ,constraint UC_{0} unique (RoleId, PermissionId)
    ,constraint FK_{0}_0 foreign key (RoleId) references {1}(Id)
    ,constraint FK_{0}_1 foreign key (PermissionId) references {2}(Id)
)
", RolePermissionsTableName, RoleTableName, PermissionTableName);
        static readonly string UserLoginCreateTableSql = string.Format(@"
create table {0} (
     Id             nvarchar(40) not null primary key    
    ,UserId         nvarchar(40) not null
    ,Provider       nvarchar(96)
    ,ProviderKey    nvarchar(128)
    ,ProviderDisplayName nvarchar(128)

    ,constraint UC_{0} unique (UserId, Provider)
)
", UserLoginTableName);
        static readonly string UserClaimsCreateTableSql = string.Format(@"
create table {0} (
     Id         nvarchar(40) not null primary key    
    ,UserId     nvarchar(40) not null
    ,Type       nvarchar(96)
    ,Value      nvarchar(255)

    ,constraint UC_{0} unique (UserId, Type, Value)
)
", UserClaimsTableName);
        static readonly string UserTokenCreateTableSql = string.Format(@"
create table {0} (
     Id             nvarchar(40) not null primary key    
    ,UserId         nvarchar(40) not null
    ,Provider       nvarchar(96)
    ,Name           nvarchar(128)
    ,Data           text

    ,constraint UC_{0} unique (UserId, Provider, Name)
)
", UserTokensTableName);   // nvarchar(4000)
        static readonly string RoleClaimsCreateTableSql = string.Format(@"
create table {0} (
     Id         nvarchar(40) not null primary key    
    ,RoleId     nvarchar(40) not null
    ,Type       nvarchar(96)
    ,Value      nvarchar(255)

    ,constraint UC_{0} unique (RoleId, Type, Value)
)
", RoleClaimsTableName);


        static readonly Dictionary<string, string> CreateTableDic = new Dictionary<string, string>() {
            { UserTableName, UserCreateTableSql },
            { RoleTableName, RoleCreateTableSql },
            { PermissionTableName, PermissionCreateTableSql },

            { UserRolesTableName, UserRolesCreateTableSql },
            { RolePermissionsTableName, RolePermissionsCreateTableSql },

            { UserLoginTableName, UserLoginCreateTableSql },
            { UserClaimsTableName, UserClaimsCreateTableSql },
            { UserTokensTableName, UserTokenCreateTableSql },
            { RoleClaimsTableName, RoleClaimsCreateTableSql },
        };

        /* initialization */
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type UserType = typeof(User);
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type RoleType = typeof(Role);
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type PermissionType = typeof(Permission);
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type UserClaimType = typeof(UserClaim);
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type UserTokenType = typeof(UserToken);
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type ExternalUserType = typeof(UserLogin);
        /// <summary>
        /// Type constant
        /// </summary>
        static public readonly Type RoleClaimType = typeof(RoleClaim);


        static TableInfo Find(Type EntityType)
        {
            return Tables.FirstOrDefault(item => item.EntityType == EntityType);
        }
        static void PrepareTables()
        {
            string SqlText;

            Tables.Add(new TableInfo(UserType, UserTableName));
            Tables.Add(new TableInfo(RoleType, RoleTableName));
            Tables.Add(new TableInfo(PermissionType, PermissionTableName));
            Tables.Add(new TableInfo(UserClaimType, UserClaimsTableName));
            Tables.Add(new TableInfo(UserTokenType, UserTokensTableName));
            Tables.Add(new TableInfo(ExternalUserType, UserLoginTableName));
            Tables.Add(new TableInfo(RoleClaimType, RoleClaimsTableName));

            TableInfo TI = Find(typeof(User));
            bool CreateTables = false;
            try
            {
                Store.Select(TI.SelectRowSql, "NOT_EXISTED_ID");                
            }
            catch 
            {
                CreateTables = true;
            }

            if (CreateTables)
            {
                foreach (var Entry in CreateTableDic)
                {
                    SqlText = Entry.Value;
                    Store.ExecSql(SqlText);
                }
            }
        }




        /* properties */
        static List<TableInfo> Tables = new List<TableInfo>();
    }
}

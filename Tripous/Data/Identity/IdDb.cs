using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Security.Claims;

using Tripous.Data;
 

namespace Tripous.Identity
{
    
    /// <summary>
    /// Represents the identity sub-system
    /// </summary>
    static public partial class IdDb
    {
        /* privae */
        static SqlStore Store;       

        static void TrimLastCommaAndSpace(StringBuilder SB)
        {
            if (SB.Length > 0)
            {
                string S = SB.ToString().TrimEnd();
                S = S.TrimEnd(',');
                SB.Clear();
                SB.Append(S);
            }
        }


        /* public */
        /// <summary>
        /// Initializes this class
        /// </summary>
        static public void Initialize(SqlConnectionInfo ConnectionInfo)
        {
            if (Store == null)
            {
                Store = SqlStores.CreateSqlStore(ConnectionInfo);  
                PrepareTables();
            }
        }

        /* INSERT-UPDATE-DELETE-SELECT by Id */
        /// <summary>
        /// Inserts an identity entity to the database
        /// </summary>
        static public void Insert(IdentityEntity Entity)
        {
            Entity.BeforeSave(true);

            if (string.IsNullOrWhiteSpace(Entity.Id))
            {
                Entity.Id = Sys.GenId();
            }
 
            Dictionary<string, object> Params = Entity.CreateSqlCommandParams();
            TableInfo Info = Find(Entity.GetType());
            Store.ExecSql(Info.InsertSql, Params); 
        }
        /// <summary>
        /// Updates an identity entity to the database
        /// </summary>
        static public void Update(IdentityEntity Entity)
        {
            Entity.BeforeSave(false);

            Dictionary<string, object> Params = Entity.CreateSqlCommandParams();
            TableInfo Info = Find(Entity.GetType());
            Store.ExecSql(Info.UpdateSql, Params);
        }
        /// <summary>
        /// Deletes an identity entity from the database
        /// </summary>
        static public void Delete(IdentityEntity Entity)
        {
            if (!string.IsNullOrWhiteSpace(Entity.Id))
                Delete(Entity.GetType(), Entity.Id);
        }
        /// <summary>
        /// Deletes an identity entity from the database
        /// </summary>
        static public void Delete(Type EntityType, string Id)
        {
            TableInfo Info = Find(EntityType);
            Store.ExecSql(Info.DeleteSql, Id);
        }


        /// <summary>
        /// Finds and returns an entity by Id, if any, else null.
        /// </summary>
        static public IdentityEntity FindById(Type EntityType, string Id)
        {
            IdentityEntity Result = null;
            TableInfo Info = Find(EntityType);
            DataRow Row = Store.SelectResults(Info.SelectRowSql, Id);
            if (Row != null)
            {
                Result = Activator.CreateInstance(EntityType, new object[] { Row }) as IdentityEntity;
            }

            return Result;
        }
        /// <summary>
        /// Checks for the uniqueness of a specified field in a table.
        /// </summary>
        static public void CheckUniqueStringField(string TableName, string FieldName, string Value, string ExcludeId = null)
        {
            string SqlText = string.Format($"select Id from {TableName} where {FieldName} = :FieldValue");
            DataTable Table = Store.Select(SqlText, Value);

            if (Table.Rows.Count > 0)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    if (!Row.IsNull(0) && string.Compare(ExcludeId, Row[0].ToString(), StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        throw new ApplicationException($"There is already an entry in the table with the same value. Table: {TableName}, Field: {FieldName}, Value: {Value}");
                    }
                }
            }
        }


        /* user */
        /// <summary>
        /// Finds and returns a user by a specified normalized name, if any, else null.
        /// </summary>
        static public User FindByNormalizedUserName(string NormalizedUserName)
        {
            string SqlText = $"select * from {UserTableName} where NormalizedUserName = :NormalizedUserName";

            User Result = null;
 
            DataRow Row = Store.SelectResults(SqlText, NormalizedUserName);
            if (Row != null)
            {
                Result = new User(Row);
            }

            return Result;

        }
        /// <summary>
        /// Finds and returns a user by a specified normalized email, if any, else null.
        /// </summary>
        static public User FindByNormalizedEmail(string NormalizedEmail)
        {
            string SqlText = $"select * from {UserTableName} where NormalizedEmail = :NormalizedEmail";

            User Result = null;

            DataRow Row = Store.SelectResults(SqlText, NormalizedEmail);
            if (Row != null)
            {
                Result = new User(Row);
            }

            return Result;
        }
 

        /* user password */ 
        /// <summary>
        /// Returns true if a user has password
        /// </summary>
        static public bool HasPassword(string UserId)
        {
            return !string.IsNullOrWhiteSpace(GetPassword(UserId));
        }
        /// <summary>
        /// Saves the hashed password of a user to the database
        /// </summary>
        static public void SaveHashedPassword(string UserId, string Password)
        {
            string SqlText = $"update {UserTableName} set Password = :Password where Id = :Id"; 
        }
        /// <summary>
        /// Makes a hash out of a plain text password of a user and saves the hashed password to the database
        /// </summary>
        static public string SavePassword(User User, string PlainTextPassword)
        {
            return SavePassword(User.Id, PlainTextPassword);
        }
        /// <summary>
        /// Makes a hash out of a plain text password of a user and saves the hashed password to the database
        /// </summary>
        static public string SavePassword(string UserId, string PlainTextPassword)
        {
            string Salt = IdLib.PasswordHasher.GenerateSalt();
            string Password = IdLib.PasswordHasher.Hash(PlainTextPassword, Salt);  

            string SqlText = $"update {UserTableName} set Password = :Password, PasswordSalt = :PasswordSalt where Id = :Id";

            Store.ExecSql(SqlText, Password, Salt, UserId);

            return Password;
        }
        /// <summary>
        /// Returns the hashed password of a user from the database
        /// </summary>
        static public string GetPassword(User User)
        {
            return GetPassword(User.Id);
        }
        /// <summary>
        /// Returns the hashed password of a user from the database
        /// </summary>
        static public string GetPassword(string UserId)
        {
            string SqlText = $"select Password from {UserTableName} where Id = :Id";
            string Default = "";
            object Result = Store.SelectResult(SqlText, Default, UserId);
            return Result.ToString();
        }
        /// <summary>
        /// Returns true when a plain text password and a hashed password are equal.
        /// </summary>
        static public bool VerifyPassword(User User, string Password, string PlainTextPassword)
        {
            return VerifyPassword(User.Id, Password, PlainTextPassword);
        }
        /// <summary>
        /// Returns true when a plain text password and a hashed password are equal.
        /// </summary>
        static public bool VerifyPassword(string UserId, string Password, string PlainTextPassword)
        {
            string SqlText = $"select PasswordSalt from {UserTableName} where Id = :Id";
            string Default = "";
            object Res = Store.SelectResult(SqlText, Default, UserId);

            string Salt = Res.ToString();
            return IdLib.PasswordHasher.Verify(PlainTextPassword, Salt, Password); 
        }

        /* user claim */
        /// <summary>
        /// Returns the list of claims of a user
        /// </summary>
        static public List<UserClaim> GetUserClaims(string UserId)
        {
            List<UserClaim> Result = new List<UserClaim>();

            string SqlText = $"select * from {UserClaimsTableName} where UserId = :UserId";
            DataTable Table = Store.Select(SqlText, UserId);
            foreach (DataRow Row in Table.Rows)
            {
                Result.Add(new UserClaim(Row));
            }
            return Result;
        }
        /// <summary>
        /// Adds a list of claims to the claims of a user
        /// </summary>
        static public void AddUserClaims(string UserId, IEnumerable<Claim> claims)
        {
            UserClaim claim;
            foreach (Claim Source in claims)
            {
                claim = new UserClaim(Source);
                claim.UserId = UserId;
                Insert(claim);
            }
        }
        /// <summary>
        /// Returns a list of users that all have a specified claim
        /// </summary>
        static public List<User> GetUsersForClaim(Claim claim)
        {
            List<User> Result = new List<User>();

            string SqlText = $@"
select * 
from 
    {UserTableName}
        inner join {UserTableName} on {UserTableName}.Id = {UserClaimsTableName}.UserId 
where
        {UserClaimsTableName}.Type = :Type
    and {UserClaimsTableName}.Value = :Value
";

            DataTable Table = Store.Select(SqlText, claim.Type, claim.Value);
            foreach (DataRow Row in Table.Rows)
            {
                Result.Add(new User(Row));
            }

            return Result;
        }

        /* user token */
        /// <summary>
        /// Returns a user token
        /// </summary>
        static public UserToken GetUserToken(string UserId, string Provider, string TokenName)
        {
            string SqlText = $@"
select
    *
from
    {UserTokensTableName}
where
        UserId = :UserId
    and Provider = :Provider
    and Name = :Name
";

            UserToken Result = new UserToken() { UserId = UserId };
            DataRow Row = Store.SelectResults(SqlText, UserId, Provider, TokenName);
            if (Row != null)
                Result.Assign(Row);
            return Result;
        }

        /* user external account */
        /// <summary>
        /// Returns the logins (accounts a user may have in external providers, e.g. facebook, google) of a user
        /// </summary>
        static public List<UserLogin> GetUserLogins(string UserId)
        {
            List<UserLogin> Result = new List<UserLogin>();

            string SqlText = $"select * from {UserLoginTableName} where UserId = :UserId";
            DataTable Table = Store.Select(SqlText, UserId);
            foreach (DataRow Row in Table.Rows)
            {
                Result.Add(new UserLogin(Row));
            }
            return Result;
        }
        /// <summary>
        /// Finds and returns a user having a login in provider, by a specified provider and key (the key uniquelly identifies a user in the external identity provider).
        /// If nothing is found, null is returned.
        /// </summary>
        static public User FindUserByUserLoginProvider(string Provider, string ProviderKey)
        { 

            string SqlText = $@"
select * 
from 
    {UserTableName}
        inner join {UserTableName} on {UserTableName}.Id = {UserLoginTableName}.UserId 
where
        {UserLoginTableName}.Provider = :Provider
    and {UserLoginTableName}.ProviderKey = :ProviderKey
";

 
            DataRow Row = Store.SelectResults(SqlText, Provider, ProviderKey);
            return Row != null ? new User(Row) : null;
        }




        /* role */
        /// <summary>
        /// Returns all roles.
        /// </summary>
        static public List<Role> GetRoles()
        {
            List<Role> Result = new List<Role>();
            string SqlText = $"select * from {RoleTableName}";
            DataTable Table = Store.Select(SqlText);

            foreach (DataRow Row in Table.Rows)
                Result.Add(new Role(Row));       
 
            return Result;
        }
        /// <summary>
        /// Finds an returns a role by a normalized role name
        /// </summary>
        static public Role FindByNormalizedRoleName(string NormalizedRoleName)
        {
            string SqlText = $"select * from {RoleTableName} where NormalizedName = :NormalizedName";

            Role Result = null;

            DataRow Row = Store.SelectResults(SqlText, NormalizedRoleName);
            if (Row != null)
            {
                Result = new Role(Row);
            }

            return Result;
        }

        /* role claim */
        /// <summary>
        /// Returns the claims of a role
        /// </summary>
        static public List<RoleClaim> GetRoleClaims(string RoleId)
        {
            List<RoleClaim> Result = new List<RoleClaim>();

            string SqlText = $"select * from {RoleClaimsTableName} where RoleId = :RoleId";
            DataTable Table = Store.Select(SqlText, RoleId);
            foreach (DataRow Row in Table.Rows)
            {
                Result.Add(new RoleClaim(Row));
            }
            return Result;
        }
        /// <summary>
        /// Returns the claims of a role
        /// </summary>
        static public void AddRoleClaims(string RoleId, IEnumerable<Claim> claims)
        {
            RoleClaim claim;
            foreach (Claim Source in claims)
            {
                claim = new RoleClaim(Source);
                claim.RoleId = RoleId;
                Insert(claim);
            }
        }


        /* miscs */
        /// <summary>
        /// Returns the table name of a database table associated to a <see cref="Type"/> of an identity entity class
        /// </summary>
        static public string GetTableName(Type T)
        {
            TableInfo Info = Find(T);
            return Info != null ? Info.TableName : string.Empty;
        }
        


    }
}

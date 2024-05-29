using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Tripous.Data;

namespace Tripous.Logging
{

    /// <summary>
    /// A log listener that writes log info a database table
    /// </summary>
    public class SqlStoreLogListener : LogListener
    {
 
        const string CreateTableSql = @"
create table {0}  (
   Id                     {1}
  ,Stamp                  @DATETIME        @NULL 
  ,LogDate                @NVARCHAR(12)    @NULL
  ,LogTime                @NVARCHAR(12)    @NULL
  ,UserName               @NVARCHAR(96)    @NULL
  ,HostName               @NVARCHAR(64)    @NULL
  ,Level                  @NVARCHAR(24)    @NULL     
  ,Source                 @NVARCHAR(96)    @NULL
  ,Scope                  @NVARCHAR(96)    @NULL
  ,EventId                @NVARCHAR(96)    @NULL
  ,Data                   @NBLOB_TEXT      @NULL
)
";
        const string InsertSql = @"
insert into {0} (
   Id      
  ,Stamp 
  ,LogDate     
  ,LogTime     
  ,UserName    
  ,HostName        
  ,Level    
  ,Source   
  ,Scope    
  ,EventId     
  ,Data        
) values (
   :Id    
  ,:Stamp
  ,:LogDate     
  ,:LogTime     
  ,:UserName    
  ,:HostName        
  ,:Level    
  ,:Source   
  ,:Scope     
  ,:EventId     
  ,:Data     
)
";

        static object syncLock = new LockObject();

        string ConnectionName;
        SqlStore Store;
        Dictionary<string, object> DataDic = new Dictionary<string, object>()
        {
            { "Id", null },
            { "Stamp", null },
            { "LogDate", null },
            { "LogTime", null },
            { "UserName", null },
            { "HostName", null },
            { "Level", null },
            { "Source", null },
            { "Scope", null },
            { "EventId", null },
            { "Data", null },
        };

        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlStoreLogListener(string ConnectionName = "")
        {
            this.ConnectionName = !string.IsNullOrWhiteSpace(ConnectionName) ? ConnectionName : SysConfig.DefaultConnection;
        }

        /* public */
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        public override void ProcessLog(LogEntry Entry)
        {
            lock (syncLock)
            {
                string SqlText;

                if (Store == null)
                {
                    Store = SqlStores.CreateSqlStore(ConnectionName);
                }

                if (!Store.TableExists(SysTables.Log))
                {
                    SqlText = string.Format(CreateTableSql, SysTables.Log, "@NVARCHAR(40)    @NOT_NULL primary key");
                    Store.CreateTable(SqlText);
                }

                DataDic["Id"] = Entry.Id;
                DataDic["Stamp"] = Entry.TimeStamp;
                DataDic["LogDate"] = Entry.Date;
                DataDic["LogTime"] = Entry.Time;
                DataDic["UserName"] = !string.IsNullOrWhiteSpace(Entry.User) ? Entry.User : string.Empty;
                DataDic["HostName"] = !string.IsNullOrWhiteSpace(Entry.Host) ? Entry.Host : string.Empty;
                DataDic["Level"] = Entry.LevelText;
                DataDic["Source"] = !string.IsNullOrWhiteSpace(Entry.Source) ? Entry.Source : string.Empty;
                DataDic["Scope"] = !string.IsNullOrWhiteSpace(Entry.ScopeId) ? Entry.ScopeId : string.Empty;
                DataDic["EventId"] = !string.IsNullOrWhiteSpace(Entry.EventId) ? Entry.EventId : string.Empty;
                DataDic["Data"] = Entry.AsJson();

                SqlText = string.Format(InsertSql, SysTables.Log);
                Store.ExecSql(SqlText, DataDic);

            }
        }
    }
}

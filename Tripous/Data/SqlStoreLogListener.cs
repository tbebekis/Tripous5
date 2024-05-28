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
    public class SqlStoreLogListener : ILogListener
    {
        const string CreateTableSql = @"
create table {0}  (
   Id                     {1}
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
        const string InsertSql = @"
insert into {0} (
   Id          
  ,LogDate     
  ,LogTime     
  ,UserName    
  ,Host        
  ,LogLevel    
  ,LogSource   
  ,ScopeId     
  ,EventId     
  ,Data        
) values (
   :Id          
  ,:LogDate     
  ,:LogTime     
  ,:UserName    
  ,:Host        
  ,:LogLevel    
  ,:LogSource   
  ,:ScopeId     
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
            { "LogDate", null },
            { "LogTime", null },
            { "UserName", null },
            { "Host", null },
            { "LogLevel", null },
            { "LogSource", null },
            { "ScopeId", null },
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
        public void ProcessLog(LogEntry Info)
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

                DataDic["Id"] = Sys.GenId();
                DataDic["LogDate"] = Info.TimeStamp.Date;
                DataDic["LogTime"] = Info.Time;
                DataDic["UserName"] = !string.IsNullOrWhiteSpace(Info.User) ? Info.User : string.Empty;
                DataDic["Host"] = !string.IsNullOrWhiteSpace(Info.Host) ? Info.Host : string.Empty;
                DataDic["LogLevel"] = Info.Level.ToString();
                DataDic["LogSource"] = !string.IsNullOrWhiteSpace(Info.Source) ? Info.Source : string.Empty;
                DataDic["ScopeId"] = !string.IsNullOrWhiteSpace(Info.ScopeId) ? Info.ScopeId : string.Empty;
                DataDic["EventId"] = !string.IsNullOrWhiteSpace(Info.EventId) ? Info.EventId : string.Empty;

                StringBuilder SB = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Info.Text))
                    SB.AppendLine(Info.Text);

                if (Info.Properties != null && Info.Properties.Count > 0)
                {
                    SB.AppendLine();
                    SB.AppendLine("Properties");
                    SB.AppendLine(Json.Serialize(Info.Properties));
                }

                if (!string.IsNullOrWhiteSpace(Info.ExceptionData))
                {
                    SB.AppendLine();
                    SB.AppendLine(Info.ExceptionData);
                }

                DataDic["Data"] = SB.ToString();

                SqlText = string.Format(InsertSql, SysTables.Log);
                Store.ExecSql(SqlText, DataDic);

            }
        }
    }
}

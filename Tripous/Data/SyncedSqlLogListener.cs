namespace Tripous.Data
{
    public class SyncedSqlLogListener: SqlLogListener
    {
        SynchronizationContext fSyncContext = AsyncOperationManager.SynchronizationContext; 

        void OnEntryEvent(SqlLogEntry Entry)
        {
            if (EntryEvent != null)
            {
                SqlLogEntryArgs Args = new SqlLogEntryArgs(Entry);
                EntryEvent(this, Args);
            }
        }

        public override void ProcessLog(SqlLogEntry Entry)
        {
            fSyncContext.Post(e => OnEntryEvent(e as SqlLogEntry), Entry);
        }


        /// <summary>
        /// Occurs when a new <see cref="SqlLogEntry"/ is available.>
        /// </summary>
        public event EventHandler<SqlLogEntryArgs> EntryEvent;
    }
}

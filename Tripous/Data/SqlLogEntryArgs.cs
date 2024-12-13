namespace Tripous.Data
{
 
    public class SqlLogEntryArgs : EventArgs
    {
        public SqlLogEntryArgs(SqlLogEntry Entry)
        {
            this.Entry = Entry;
        }

        public SqlLogEntry Entry { get; }
    }
}

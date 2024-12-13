namespace Tripous.Data
{
    public abstract class SqlLogListener
    {
        public SqlLogListener()
        {
            Register();
        }

        public abstract void ProcessLog(SqlLogEntry Info);
        public void Register()
        {
            SqlMonitor.Add(this);
        }
        public void Unregister()
        {
            SqlMonitor.Remove(this);
        }

    }
}

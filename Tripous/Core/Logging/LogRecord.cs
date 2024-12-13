namespace Tripous.Logging
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LogRecord
    {
        public LogRecord(LogEntry Entry)
        {
            Id = Entry.Id;
            TimeStamp = Entry.TimeStamp;
            Date = Entry.Date;
            Time = Entry.Time;
            User = Entry.User;
            Host = Entry.Host;
            Level = Entry.LevelText;
            Source = Entry.Source;
            Scope = Entry.ScopeId;
            EventId = Entry.EventId;
            Message = Entry.Text;

            if (Entry.Properties != null && Entry.Properties.Count > 0)
                Properties = Entry.GetPropertiesAsSingleLine();

            if (!string.IsNullOrWhiteSpace(Entry.ExceptionData))
                Stack = Entry.ExceptionData;
        }


        public string Id { get; }
        public DateTime TimeStamp { get; }
        public string Date { get; }
        public string Time { get; }
        public string User { get; }
        public string Host { get; }
        public string Level { get; }
        public string Source { get; }
        public string Scope { get; }
        public string EventId { get; }
        public string Message { get; }
        public string Properties { get; }
        public string Stack { get; }
 
    }
}

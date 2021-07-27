/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tripous.Logging
{

    /// <summary>
    /// Represents a log message 
    /// </summary>
    public class LogInfo
    {
 
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LogInfo(string Source, string ScopeId, string EventId, LogLevel Level, Exception Exception, string Text, params object[] Params)
        {
            this.Source = !string.IsNullOrWhiteSpace(Source) ? Source : (Exception != null? Exception.GetType().FullName: string.Empty);
            this.ScopeId = !string.IsNullOrWhiteSpace(ScopeId) ? ScopeId : string.Empty;
            this.EventId = !string.IsNullOrWhiteSpace(EventId) ? EventId : "0";
            this.Level = Level;
            this.Exception = Exception;
            this.TextTemplate = !string.IsNullOrWhiteSpace(Text) ? Text : string.Empty;
            this.Properties = Logger.FormatParams(ref Text, Params);
            if (this.Properties == null)
                this.Properties = new Dictionary<string, object>();
            this.Text = !string.IsNullOrWhiteSpace(Text) ? Text : (Exception == null ? string.Empty: Exception.Message);
            this.ExceptionData = Exception == null ? string.Empty : ExceptionEx.GetExceptionText(Exception);
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this log info message.
        /// </summary>
        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();

            SB.AppendLine(" ================================================================");
            SB.AppendLine("Date           : " + this.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            SB.AppendLine("NetUserName    : " + this.User);
            SB.AppendLine("Computer       : " + this.Host);
            SB.AppendLine(" ----------------------------------------------------------------");
            SB.AppendLine("Level          : " + Level.ToString());
            SB.AppendLine("Source         : " + Source);
            SB.AppendLine("Scope          : " + ScopeId);
            SB.AppendLine("Message text   : " + Text);
            if (!string.IsNullOrWhiteSpace(ExceptionData))
            {
                SB.AppendLine();
                SB.AppendLine(ExceptionData);
            }

            return SB.ToString();
        }
        /// <summary>
        /// Saves this log info to the folder. The file name is constructed using the date and time information.
        /// </summary>
        public void SaveToFile(string Folder = "")
        {
            Folder = !string.IsNullOrWhiteSpace(Folder) ? Folder : Logger.LogFolder;

            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            string FileName = Path.Combine(Folder, this.TimeStamp.ToFileName(true) + ".txt");
            File.WriteAllText(FileName, this.ToString());
        }

        /* properties */
        /// <summary>
        /// Returns the UTC date-time this info created
        /// </summary>
        public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;
        /// <summary>
        /// Returns the UTC date this info created.
        /// </summary>
        public string Date { get { return this.TimeStamp.ToString("yyyy-MM-dd"); } }
        /// <summary>
        /// Returns the UTC time this info created.
        /// </summary>
        public string Time { get { return this.TimeStamp.ToString("HH:mm:ss.fff"); } }
        /// <summary>
        /// The username of the current user of this application or the local computer
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// The name of the local computer
        /// </summary>
        public string Host { get { return Sys.HostName; } }

        /// <summary>
        /// The log level
        /// </summary>
        public LogLevel Level { get; private set; }
        /// <summary>
        /// The event Id
        /// </summary>
        public string EventId { get; private set; }
        /// <summary>
        /// The text before formatting params into it.
        /// </summary>
        public string TextTemplate { get; private set; }
        /// <summary>
        /// The log message
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// The scope if any
        /// </summary>
        public string ScopeId { get; private set; }
        /// <summary>
        /// The source of this log, if any
        /// </summary>
        public string Source { get; private set; }
        /// <summary>
        /// A dictionary with params passed when the log message was formatted. For use by structured log listeners
        /// </summary>
        public IDictionary<string, object> Properties { get; private set; }
        /// <summary>
        /// The exception if this is a log regarding an exception
        /// </summary>
        public Exception Exception { get; private set; }
        /// <summary>
        /// The exception data, if this is a log regarding an exception
        /// </summary>
        public string ExceptionData { get; private set; }


    }

}

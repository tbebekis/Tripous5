/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Text;
using System.Threading;

using Tripous.Logging;

namespace Tripous.Data
{




    /// <summary>
    /// Monitors sql statement execution. 
    /// <para>It gathers information from an sql statement and sends that information to registered <see cref="SqlLogListener"/>s for further processing.</para>
    /// </summary>
    static public class SqlMonitor
    {
 

        static object syncLock = new LockObject();
        static List<SqlLogListener> fListeners = new List<SqlLogListener>();
        static int fActive = 1;  // Active initially

        /* fields */

        /// <summary>
        /// Field
        /// </summary>
        static readonly int lineLength = 85;
        /// <summary>
        /// Field
        /// </summary>
        static readonly int paramLength = 23;

  

        /* private */
        /// <summary>
        /// Adds spaces to the passed Line until <see cref="lineLength"/>
        /// </summary>
        static string CompleteLength(string Line)
        {
            Line += " ";
            
            return lineLength > Line.Length? Line.PadRight(lineLength - Line.Length, ' '): Line;
        }
        /// <summary>
        /// Formats the passed Line
        /// </summary>
        static string FormatLine(string Line, params object[] Args)
        {
            return CompleteLength(string.Format(Line, Args));
        }
        /// <summary>
        /// Returns the value of the passed Parameter as a string
        /// </summary>
        static string ParamValueToString(DbParameter Parameter)
        {
            if (Sys.IsNull(Parameter.Value))
                return "[NULL]";

            if (Parameter.Value.GetType().IsValueType)
                return Parameter.Value.ToString();

            if (Parameter.Value.GetType() == typeof(System.String))
                return Parameter.Value as string;

            return "[BLOB]";
        }
        /// <summary>
        /// Appends a line to the StringBuilder using the Parameter as information source.
        /// </summary>
        static void AddParamTo(StringBuilder SB, DbParameter Parameter)
        {
            SB.AppendLine(CompleteLength(string.Format(" {0}: {1} - [{2}]",
                                        Parameter.ParameterName.PadRight(paramLength, ' '),
                                        ParamValueToString(Parameter),
                                        Parameter.DbType.ToString()
                            )));
        }

        /* public */
        /// <summary>
        /// Adds a SqlMonitor listener
        /// </summary>
        static internal void Add(SqlLogListener Listener)
        {
            lock (syncLock)
            {
                if (!fListeners.Contains(Listener))
                    fListeners.Add(Listener);
            }
        }
        /// <summary>
        /// Removes a SqlMonitor listener
        /// </summary>
        static internal void Remove(SqlLogListener Listener)
        {
            lock (syncLock)
            {
                if (fListeners.Contains(Listener))
                    fListeners.Remove(Listener);
            }
        }

        /// <summary>
        /// Helper. Infers an EventId from an SQL statement.
        /// </summary>
        static public string GetEventId(string SqlText)
        {
            string EventId = "EXECUTE";

            SqlText = SqlText.Trim();

            if (SqlText.StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase)) EventId = "SELECT";
            else if (SqlText.StartsWith("INSERT", StringComparison.InvariantCultureIgnoreCase)) EventId = "INSERT";
            else if (SqlText.StartsWith("UPDATE", StringComparison.InvariantCultureIgnoreCase)) EventId = "UPDATE";
            else if (SqlText.StartsWith("DELETE", StringComparison.InvariantCultureIgnoreCase)) EventId = "DELETE";

            return EventId;
        }
        /// <summary>
        /// Helper. Infers an EventId from an SQL statement.
        /// </summary>
        static public string GetEventId(DbCommand Cmd)
        {
            return GetEventId(Cmd.CommandText.Trim());
        }
        
        /// <summary>
        /// Returns a text representation of <see cref="DbCommand"/> parameters, along with their values, if any.
        /// </summary>
        static public string GetParamsText(DbCommand Cmd)
        {
            StringBuilder SB = new StringBuilder();

            /* params */
            if (Cmd.Parameters.Count > 0)
            {
                foreach (DbParameter Parameter in Cmd.Parameters)
                    AddParamTo(SB, Parameter);
            }

            string ParamsText = SB.ToString();
            return ParamsText;
        }
        /// <summary>
        /// Returns a string representation of a <see cref="DbCommand"/>
        /// </summary>
        static public string CommandToText(DateTime StartTimeUtc, DbCommand Cmd, string Source, string Scope)
        {
            DateTime EndTimeUtc = DateTime.UtcNow;
            TimeSpan ElapsedTime = EndTimeUtc - StartTimeUtc;

            string SqlText = Cmd.CommandText.Trim();
            string EventId = GetEventId(SqlText);
            string ParamsText = GetParamsText(Cmd);

            StringBuilder SB = new StringBuilder();
            SB.AppendLine($"Start Time Utc: {StartTimeUtc.ToString("HH:mm:ss")}");
            SB.AppendLine($"Elapsed Time: {ElapsedTime.ToString(@"dd\.hh\:mm\:ss")}");
            SB.AppendLine($"Source: {Source}");
            SB.AppendLine($"Scope: {Scope}");
            SB.AppendLine($"EventId: {EventId}");
            SB.AppendLine($"SqlText");
            SB.AppendLine(SqlText);
            if (!string.IsNullOrWhiteSpace(ParamsText))
            {
                SB.AppendLine("Parameters");
                SB.AppendLine(ParamsText);
            }

            return SB.ToString();
        }


        /// <summary>
        /// Log method. Creates a <see cref="SqlLogEntry"/> instance and passes it to the registered <see cref="SqlLogListener"/>s.
        /// </summary>
        static public void LogSql(DateTime StartTimeUtc, string SqlText, string Source, string Scope, string EventId, string ParamsText)
        {
 
            lock (syncLock)
            {
                if (fListeners.Count == 0 || !Active)
                    return;

                if ((fListeners.Count > 0) && !string.IsNullOrWhiteSpace(SqlText))
                {
                       
                    SqlLogEntry Entry = new SqlLogEntry(StartTimeUtc, SqlText,  Source, Scope, EventId, ParamsText);

                    foreach (var Listener in fListeners)
                    {
                        try
                        {
                            Listener.ProcessLog(Entry);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Log method. Creates a <see cref="SqlLogEntry"/> instance and passes it to the registered <see cref="SqlLogListener"/>s.
        /// </summary>
        static public void LogSql(DateTime StartTimeUtc, string SqlText, string Source, string Scope, string ParamsText)
        {
            LogSql(StartTimeUtc, SqlText, Source, Scope, GetEventId(SqlText), ParamsText);
        }
        /// <summary>
        /// Log method. Creates a <see cref="SqlLogEntry"/> instance and passes it to the registered <see cref="SqlLogListener"/>s.
        /// </summary>
        static public void LogSql(DateTime StartTimeUtc, string SqlText, string Source, string ParamsText)
        {
            LogSql(StartTimeUtc, SqlText, Source, string.Empty, GetEventId(SqlText), ParamsText);
        }
        /// <summary>
        /// Log method. Creates a <see cref="SqlLogEntry"/> instance and passes it to the registered <see cref="SqlLogListener"/>s.
        /// </summary>
        static public void LogSql(DateTime StartTimeUtc, string SqlText, string ParamsText)
        {
            LogSql(StartTimeUtc, SqlText, string.Empty, string.Empty, GetEventId(SqlText), ParamsText);
        }



        /// <summary>
        /// The monitor is considered active when two conditions are met:
        /// 1. Active property is set to true
        /// 2. There are registered listeners
        /// </summary>
        static public bool Active
        {
            get
            {
                lock (syncLock)
                {
                    return (fActive == 0) && (fListeners.Count > 0);
                }
            }
            set
            {
                lock (syncLock)
                {
                    if (!value)
                        fActive++;
                    else
                    {
                        fActive--;
                        if (fActive < 0)
                            fActive = 0;
                    }
                }
            }
        }
    }

}

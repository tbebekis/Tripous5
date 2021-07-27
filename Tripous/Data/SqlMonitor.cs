/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;

using Tripous.Logging;

namespace Tripous.Data
{


    /// <summary>
    /// Monitors sql statement execution. It gathers information from an sql statement and sends
    /// that information to the Logger.
    /// </summary>
    static public class SqlMonitor
    {

        const string cStartLine = "/*==============================================================";
        const string cEndLine = "--------------------------------------------------------------*/";
        const string cLine = "----------------------------------------------------------------";

        static object syncLock = new LockObject();
        static List<ILogListener> fListeners = new List<ILogListener>();

        /* fields */
        /// <summary>
        /// Field
        /// </summary>
        static int fCounter;
        /// <summary>
        /// Field
        /// </summary>
        static readonly int lineLength = 85;
        /// <summary>
        /// Field
        /// </summary>
        static readonly int paramLength = 23;

        /* private properties */
        static int Counter { get { lock (syncLock) return fCounter; } }
        static int ListenerCount { get { lock (syncLock) return fListeners.Count; } }

        /* private */
        /// <summary>
        /// Adds spaces to the passed Line until <see cref="lineLength"/>
        /// </summary>
        static string CompleteLength(string Line)
        {
            Line += " ";
            return Line.PadRight(lineLength - Line.Length, ' ');
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
        static public void Add(ILogListener Listener)
        {
            lock (syncLock)
            {
                fListeners.Add(Listener);
            }
        }
        /// <summary>
        /// Removes a SqlMonitor listener
        /// </summary>
        static public void Remove(ILogListener Listener)
        {
            lock (syncLock)
            {
                fListeners.Remove(Listener);
            }
        }

        /// <summary>
        /// Prepares a text using the passed DbCommand and Datetime as sources.
        /// </summary>
        static public string CommandToText(DateTime StartTime, DbCommand Cmd)
        {
            try
            {
                if ((Cmd != null) && !string.IsNullOrWhiteSpace(Cmd.CommandText))
                {

                    DateTime EndTime = DateTime.Now;
                    TimeSpan ElapsedTime = DateTime.Now - StartTime;

                    StringBuilder SB = new StringBuilder();

                    /* header */

                    SB.AppendLine(cStartLine);
                    SB.AppendLine(string.Format(" Started at: {0} - Elapsed time: {1}  ",
                        StartTime.ToString("HH:mm:ss"),
                        ElapsedTime.ToString(@"dd\.hh\:mm\:ss")));

                    /* params */
                    if (Cmd.Parameters.Count > 0)
                    {
                        SB.AppendLine(cLine);
                        foreach (DbParameter Parameter in Cmd.Parameters)
                        {
                            AddParamTo(SB, Parameter);
                        }
                    }

                    SB.AppendLine(cEndLine);



                    /* sql */
                    string SqlText = Cmd.CommandText.Trim();
                    SB.Append(SqlText);



                    return SB.ToString();
                }
            }
            catch
            {
            }


            return string.Empty;
        }
        /// <summary>
        /// Logs information regarding the passed DbCommand and DateTime
        /// </summary>
        static public void LogSql(DateTime StartTime, DbCommand Cmd)
        {
            Interlocked.Increment(ref fCounter);

            lock (syncLock)
            {
                if ((fListeners.Count > 0) && (Cmd != null))
                {
                    try
                    {
                        string SqlText = string.IsNullOrWhiteSpace(Cmd.CommandText) ? string.Empty : Cmd.CommandText.Trim();
                        if (!string.IsNullOrWhiteSpace(SqlText))
                        {
                            string Source = typeof(SqlMonitor).FullName;
                            string EventId = "EXECUTE";

                            if (SqlText.StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase)) EventId = "SELECT";
                            else if (SqlText.StartsWith("INSERT", StringComparison.InvariantCultureIgnoreCase)) EventId = "INSERT";
                            else if (SqlText.StartsWith("UPDATE", StringComparison.InvariantCultureIgnoreCase)) EventId = "UPDATE";
                            else if (SqlText.StartsWith("DELETE", StringComparison.InvariantCultureIgnoreCase)) EventId = "DELETE";

                            string CommandText = CommandToText(StartTime, Cmd);
                            StringBuilder SB = new StringBuilder();
                            SB.AppendLine(string.Format("[{0}]", Counter));
                            SB.AppendLine(CommandText);
                            
                            LogInfo LogInfo = new LogInfo(Source, "", EventId, LogLevel.Trace, null, SB.ToString());

                            foreach (var Listener in fListeners)
                            {
                                try
                                {
                                    Listener.ProcessLog(LogInfo);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

        }


    }

}

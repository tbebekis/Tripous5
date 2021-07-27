/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Tripous.Logging
{

    /// <summary>
    /// Represents an object that handles log information.
    /// <para>
    /// A code can instruct Logger to log a bit of information by calling
    /// one of the various Log() methods. Then the Logger notifies
    /// all ILogListener objects subscribed to it
    /// by calling the ILogListener.ProcessLog() of each one
    /// passing an LogInfo object that represents the log information.  
    /// </para><para>
    /// The Logger doesn't actually logs the information in any medium.
    /// Instead it relies in that some one of its ILogListener subscribers
    /// is capable of doing that.
    /// </para><para>
    /// Any thread can call any of the Log() methods of the Logger.
    /// </para><para>
    /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
    /// Thus Listeners should synchronize the ProcessLog() call. Controls need to check if InvokeRequired.
    /// </para>
    /// </summary>
    static public class Logger
    {
        static object[] EmptyParams = new object[] { };

        static object syncLock = new LockObject();
        static int fActive = 0;
        static LogLevel fLevel = LogLevel.Info;
        static List<ILogListener> listeners = new List<ILogListener>();
        static string fLogFolder;

        static bool CanCallListener(ILogListener Listener)
        {
            if (Sys.HasProperty(Listener, "IsDisposed"))
            {
                object V = Sys.GetProperty(Listener, "IsDisposed");
                if (V != null)
                    return !Convert.ToBoolean(V);
            }

            return true;
        }

        /* public  */
        /// <summary>
        /// Passes the log information to each listener.
        /// </summary>
        static public void Log(LogInfo Info)
        {
            lock (syncLock)
            {
                if (Active && fLevel != LogLevel.None && (int)Info.Level >= (int)fLevel)
                {
                    foreach (ILogListener Item in listeners)
                    {
                        if (!CanCallListener(Item))
                            continue;

                        Threads.Run((info) => 
                        {
                            try
                            {
                                Item.ProcessLog(info);
                            }
#if DEBUG
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                            }
#else
                            catch
                            {
                            }
#endif
                        },                            
                        Info);
                         
                    }
                }
 
            }
        }
        /// <summary>
        /// Passes the log information to each listener.
        /// <para>NOTE: For how params are formatted into the specified text see <see cref="FormatParams" /> </para>
        /// </summary>
        static public void Log(string Source, string ScopeId, string EventId, LogLevel Level, Exception Exception, string Text, params object[] Params)
        {       
            LogInfo Info = new LogInfo(Source, ScopeId, EventId, Level, Exception, Text, Params);
            Log(Info);
        }

        /// <summary>
        /// Logs a trace level message
        /// </summary>
        static public void Trace(string Source, string ScopeId, string EventId, string Text)
        {
            Exception Ex = null;
            Log(Source, ScopeId, EventId, LogLevel.Trace, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a trace level message
        /// </summary>
        static public void Trace(string Source, string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            Log(Source, ScopeId, EventId, LogLevel.Trace, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a trace level message
        /// </summary>
        static public void Trace(string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            Log(Source, ScopeId, EventId, LogLevel.Trace, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a trace level message
        /// </summary>
        static public void Trace(string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            string EventId = "0";
            Log(Source, ScopeId, EventId, LogLevel.Trace, Ex, Text, EmptyParams);
        }

        /// <summary>
        /// Logs a debug level message
        /// </summary>
        static public void Debug(string Source, string ScopeId, string EventId, string Text)
        {
            Exception Ex = null;
            Log(Source, ScopeId, EventId, LogLevel.Debug, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a debug level message
        /// </summary>
        static public void Debug(string Source, string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            Log(Source, ScopeId, EventId, LogLevel.Debug, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a debug level message
        /// </summary>
        static public void Debug(string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            Log(Source, ScopeId, EventId, LogLevel.Debug, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a debug level message
        /// </summary>
        static public void Debug(string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            string EventId = "0";
            Log(Source, ScopeId, EventId, LogLevel.Debug, Ex, Text, EmptyParams);
        }

        /// <summary>
        /// Logs an info level message
        /// </summary>
        static public void Info(string Source, string ScopeId, string EventId, string Text)
        {
            Exception Ex = null;
            Log(Source, ScopeId, EventId, LogLevel.Info, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an info level message
        /// </summary>
        static public void Info(string Source, string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            Log(Source, ScopeId, EventId, LogLevel.Info, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an info level message
        /// </summary>
        static public void Info(string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            Log(Source, ScopeId, EventId, LogLevel.Info, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an info level message
        /// </summary>
        static public void Info(string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            string EventId = "0";
            Log(Source, ScopeId, EventId, LogLevel.Info, Ex, Text, EmptyParams);
        }

        /// <summary>
        /// Logs a warn level message
        /// </summary>
        static public void Warn(string Source, string ScopeId, string EventId, string Text)
        {
            Exception Ex = null;
            Log(Source, ScopeId, EventId, LogLevel.Warn, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a warn level message
        /// </summary>
        static public void Warn(string Source, string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            Log(Source, ScopeId, EventId, LogLevel.Warn, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a warn level message
        /// </summary>
        static public void Warn(string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            Log(Source, ScopeId, EventId, LogLevel.Warn, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs a warn level message
        /// </summary>
        static public void Warn(string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            string EventId = "0";
            Log(Source, ScopeId, EventId, LogLevel.Warn, Ex, Text, EmptyParams);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string Source, string ScopeId, string EventId, Exception Ex)
        {
            string Text = string.Empty;
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string Source, string EventId, Exception Ex)
        {
            string Text = string.Empty;
            string ScopeId = "";
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string EventId, Exception Ex)
        {
            string Text = string.Empty;
            string ScopeId = "";
            string Source = "";
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(Exception Ex)
        {
            string Text = string.Empty;
            string ScopeId = "";
            string Source = "";
            string EventId = "0";
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string Source, string ScopeId, string EventId, string Text)
        {
            Exception Ex = null;
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string Source, string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string EventId, string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        static public void Error(string Text)
        {
            Exception Ex = null;
            string ScopeId = "";
            string Source = "";
            string EventId = "0";
            Log(Source, ScopeId, EventId, LogLevel.Error, Ex, Text, EmptyParams);
        }

        /// <summary>
        /// Formats the specified params into a placeholder containing text. It also returns a dictionary of the specified params for use by structured log listeners.
        /// <example>The specified text may contain placeholders. Placeholders are replaced based in their order, NOT in their content. 
        /// The content of a placeholder becomes a key in the returned dictionary.
        /// <code>
        ///     string Text = "Customer {CustomerId} order {OrderId} is completed.";
        ///     var ParamsDictionary = FormatParams(ref Text, new { CustomerId = 123, OrderId = 456 });
        /// </code>
        /// </example>
        /// </summary>
        static public Dictionary<string, object> FormatParams(ref string Text, params object[] Params)
        {
            Dictionary<string, object> Result = null;

            if (Params != null)
            {
                string Pattern = @"(?<=\{)[^}]*(?=\})";
                MatchCollection Matches = Regex.Matches(Text, Pattern);

                if (Params.Length == Matches.Count)
                {
                    Result = new Dictionary<string, object>();
                    Match Match;
                    object Value;

                    for (int i = Params.Length - 1; i >= 0; i--)
                    {
                        Match = Matches[i];
                        Value = Params[i];
                        Result[Match.Value] = Value;
                        Text = Text.Substring(0, Match.Index - 1) + (Value == null ? string.Empty : Value.ToString()) + Text.Substring(Match.Index + Match.Length + 1);
                    } 
                }
            }

            return Result;
        }


        /// <summary>
        /// Adds a listener
        /// </summary>
        static public void Add(ILogListener Listener)
        {
            lock (syncLock)
            {
                try
                {
                    if (!listeners.Contains(Listener))
                    {
                        listeners.Add(Listener);
                    }
                }
                catch
                {
                }
            }

        }
        /// <summary>
        /// Removes a listener
        /// </summary>
        static public void Remove(ILogListener Listener)
        {
            lock (syncLock)
            {
                try
                {
                    if (listeners.Contains(Listener))
                    {
                        listeners.Remove(Listener);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Adds the <see cref="LogFileListener"/> to the listeners.
        /// </summary>
        static public void AddFileListener(string Folder = "", int MaxSizeInMB = 5)
        {
            var L = new LogFileListener(Folder, MaxSizeInMB);
            Add(L);
        }

        /* properties */
        /// <summary>
        /// When false no logs are recorded. Defaults to true.
        /// </summary>
        static public bool Active
        {
            get
            {
                lock (syncLock)
                {
                    return fActive == 0;
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
        /// <summary>
        /// The level of the accepted log. For a log info to be recorded its log level must be greater or equal to this level. 
        /// See <see cref="LogLevel" /> enum for the numeric values of each level.
        /// <para>Defaults to Info.</para>
        /// </summary>
        static public LogLevel Level { get { lock (syncLock) return fLevel; } set { lock (syncLock) fLevel = value; } }
        /// <summary>
        /// Returns the path to the folder where file logs are saved
        /// </summary>
        static public string LogFolder
        {
            get
            {
                lock (syncLock)
                {
                    if (string.IsNullOrWhiteSpace(fLogFolder))
                    {
                        fLogFolder = Path.Combine(SysConfig.AppRootDataFolder, "Logs");
                    }

                    if (!Directory.Exists(fLogFolder))
                        Directory.CreateDirectory(fLogFolder);

                    return fLogFolder;
                }

            }
            set { fLogFolder = value; }

        }
    }



}

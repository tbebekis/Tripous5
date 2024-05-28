/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Tripous.Logging
{

    /// <summary>
    /// Represents a named source in a log.
    /// <para>The name could be the full name of a class, an action url or any other suitable string and it marks all the log messages produces by this source.</para>
    /// </summary>
    public class LogSource
    {
        

        /// <summary>
        /// Represents a named scope in a log
        /// </summary>
        class LogScope : Disposable
        {
            LogSource fLogSource;

            /// <summary>
            /// Disposes managed or un-managed resources, according to the Managed passed in flag.
            /// </summary>
            protected override void Dispose(bool Managed)
            {
                if (Managed)
                {
                    fLogSource.fScopes.RemoveAt(fLogSource.fScopes.Count - 1);
                }
            }

            /* construction */
            /// <summary>
            /// Constructor
            /// </summary>
            public LogScope(LogSource LogSource, string Id = "", object ScopeParams = null)
            {
                fLogSource = LogSource;

                if (string.IsNullOrWhiteSpace(Id))
                    Id = Guid.NewGuid().ToString("D").ToUpper(); // Guid without brackets

                this.Id = Id;
                 
                if (ScopeParams != null)
                {
                    this.Properties = new Dictionary<string, object>();

                    PropertyInfo[] Properties = ScopeParams.GetType().GetProperties();

                    string Name;
                    object Value;
                    foreach (PropertyInfo Prop in Properties)
                    {
                        Name = Prop.Name;
                        Value = Prop.GetValue(ScopeParams);

                        this.Properties[Name] = Value;  
                    }
                }

            }

            /* properties */
            /// <summary>
            /// A string that names the scope.
            /// </summary>
            public string Id { get; private set; }
            /// <summary>
            /// A dictionary of the passed scope params, if any, else null.
            /// </summary>
            public Dictionary<string, object> Properties { get; private set; }
        }


        /* private */
        object syncLock = new LockObject();
        bool fActive = true;
        /// <summary>
        /// The list of scopes. Scopes can be nested. The last (default) scope can not be disposed. 
        /// That is there is always an available scope.
        /// </summary>
        List<LogScope> fScopes = new List<LogScope>();
        /// <summary>
        /// The current scope
        /// </summary>
        LogScope Scope { get { return fScopes[fScopes.Count - 1]; } }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LogSource(string Name = "")
        {
            this.Name = Name;
            BeginScope();
        }

        /* public */ 
        /// <summary>
        /// Logs a message
        /// </summary>
        public void Log(string EventId, LogLevel Level, Exception Ex, string Text, params object[] Params)
        {
            lock (syncLock)
            {
                if (fActive)
                {
                    LogEntry Info = new LogInfo(this.Name, Scope.Id, EventId, Level, Ex, Text, Params);

                    if (Scope.Properties != null)
                    {
                        foreach (var Entry in Scope.Properties)
                        {
                            Info.Properties[Entry.Key] = Entry.Value;
                        }
                    }

                    Logger.Log(Info);
                }
            }
 
        }
        /// <summary>
        /// Logs a message
        /// </summary>
        public void Log(LogLevel Level, Exception Ex, string Text, params object[] Params)
        {  
            string EventId = "0";
            Log(EventId, Level, Ex, Text, Params);
        }
        /// <summary>
        /// Logs a message
        /// </summary>
        public void Log(string EventId, LogLevel Level, string Text, params object[] Params)
        {
            Exception Ex = null;
            Log(EventId, Level, Ex, Text, Params);
        }
        /// <summary>
        /// Logs a message
        /// </summary>
        public void Log(LogLevel Level, string Text, params object[] Params)
        {
            string EventId = "0";
            Exception Ex = null;
            Log(EventId, Level, Ex, Text, Params);
        }

        /// <summary>
        /// Logs a trace level message
        /// </summary>
        public void Trace(string EventId, string Text)
        {
            Log(EventId, LogLevel.Trace, Text, new object[] { });
        }
        /// <summary>
        /// Logs a trace level message
        /// </summary>
        public void Trace(string Text)
        {
            string EventId = "0";
            Trace(EventId, Text);
        }

        /// <summary>
        /// Logs a debug level message
        /// </summary>
        public void Debug(string EventId, string Text)
        {
            Log(EventId, LogLevel.Debug, Text);
        }
        /// <summary>
        /// Logs a debug level message
        /// </summary>
        public void Debug(string Text)
        {
            string EventId = "0";
            Debug(EventId, Text);
        }

        /// <summary>
        /// Logs an info level message
        /// </summary>
        public void Info(string EventId, string Text)
        {
            Log(EventId, LogLevel.Info, Text);
        }
        /// <summary>
        /// Logs an info level message
        /// </summary>
        public void Info(string Text)
        {
            string EventId = "0";
            Info(EventId, Text);
        }

        /// <summary>
        /// Logs a warn level message
        /// </summary>
        public void Warn(string EventId, string Text)
        {
            Log(EventId, LogLevel.Warning, Text);
        }
        /// <summary>
        /// Logs a warn levelmessage
        /// </summary>
        public void Warn(string Text)
        {
            string EventId = "0";
            Warn(EventId, Text);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        public void Error(string EventId, Exception Ex)
        {
            string Text = Ex.Message;
            Log(EventId, LogLevel.Error, Ex, Text);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        public void Error(Exception Ex)
        {
            string EventId = "0";
            string Text = Ex.Message;
            Log(EventId, LogLevel.Error, Ex, Text);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        public void Error(string EventId, string Text)
        {
            Log(EventId, LogLevel.Error, Text);
        }
        /// <summary>
        /// Logs an error message
        /// </summary>
        public void Error(string Text)
        {
            string EventId = "0";
            Log(EventId, LogLevel.Error, Text);
        }

        /// <summary>
        /// Begins a new log scope. A log scope is just a label under which next log messages are recorded. Log scopes can be nested.
        /// <para>The Id of a scope is the label of the scope.</para>
        /// <para>Scoped params is an object, possibly of an anonymous type. The properties of that object are attached to each log message in the scope. </para>
        /// </summary>
        public IDisposable BeginScope(string Id = "", object ScopeParams = null)
        {
            lock (syncLock)
            {
                LogScope scope = new LogScope(this, Id, ScopeParams);
                fScopes.Add(scope);
                return scope;
            }
        }


        /* properties */
        /// <summary>
        /// The name of this log source.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// When false no logs are recorded. Defaults to true.
        /// </summary>
        public bool Active { get { lock (syncLock) return fActive; } set { lock (syncLock) fActive = value; } }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Newtonsoft.Json;

using Tripous;
using Tripous.Web;
using Tripous.Logging;

namespace WebDesk 
{

    /// <summary>
    /// Represents this library
    /// </summary>
    static public partial class Lib
    {
 
        static readonly TaskFactory fTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);


        /// <summary>
        /// Initializer
        /// </summary>
        static public void Initialize(IWebAppContext App)
        {
            if (Lib.App == null)
            {
                Lib.App = App;
            }
        }

        /// <summary>
        /// Throws an Exception
        /// </summary>
        static public void Error(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                Text = "Unknown error";

            throw new WebAppException(Text);
        }
        /// <summary>
        /// Throws an Exception
        /// </summary>
        static public void Error(string Text, params object[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
                Text = string.Format(Text, Args);
            throw new WebAppException(Text);
        }

        /// <summary>
        /// Writes a specified text to a file and saves the file to the Log folder, i.e. AppData\Log.
        /// <para>The file is saved under a name containing a specified prefix and the current timestamp.</para>
        /// </summary>
        static public void LogToFile(string Text, string FileNamePrefix = "LOGFILE")
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                string Folder = Logger.LogFolder;

                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);

                if (string.IsNullOrWhiteSpace(FileNamePrefix))
                    FileNamePrefix = "LOGFILE";

                string FileName = $"{FileNamePrefix}-{DateTime.Now.ToFileName(true)}.txt";

                string FilePath = Path.Combine(Folder, FileName);
                File.WriteAllText(FilePath, Text);
            }
        }
 
        /* async as sync */
        /// <summary>
        /// Executes an async method as a synchronous one
        /// </summary>
        static public TResult RunSync<TResult>(Func<Task<TResult>> Func)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            return fTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return Func();

            }).Unwrap().GetAwaiter().GetResult();
        }
        /// <summary>
        /// Executes an async method as a synchronous one
        /// </summary>
        static public void RunSync(Func<Task> Func)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            fTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return Func();

            }).Unwrap().GetAwaiter().GetResult();
        }

        /* error list and success list */
        /// <summary>
        /// Returns a <see cref="List{T}"/>    found under a specified key in session variables.
        /// </summary> 
        static List<string> GetSessionStringList(string Key)
        {
            List<string> List = null;
            if (Session.ContainsKey(Key))
                List = Session.Get<List<string>>(Key);

            if (List == null)
                List = new List<string>();

            return List;
        }
        /// <summary>
        /// Adds a message to SuccessList
        /// <para>NOTE: SuccessList and ErrorList messages are displayed to the user until lists are Pop()-ed.</para>
        /// </summary>
        static public void AddToSuccessList(string Message)
        {
            List<string> List = GetSessionStringList("SuccessList");
            List.Add(Message);
            Session.Set<List<string>>("SuccessList", List);
        }
        /// <summary>
        /// Adds a message to ErrorList
        /// <para>NOTE: SuccessList and ErrorList messages are displayed to the user until lists are Pop()-ed.</para>
        /// </summary>
        static public void AddToErrorList(string Message)
        {
            List<string> List = GetSessionStringList("ErrorList");
            List.Add(Message);
            Session.Set<List<string>>("ErrorList", List);
        }
        /// <summary>
        /// Adds a list of messages to ErrorList
        /// <para>NOTE: SuccessList and ErrorList messages are displayed to the user until lists are Pop()-ed.</para>
        /// </summary>
        static public void AddToErrorList(List<string> MessageList)
        {
            List<string> List = GetSessionStringList("ErrorList");
            List.AddRange(MessageList.ToArray());
            Session.Set<List<string>>("ErrorList", List);
        }

        /// <summary>
        /// Removes and returns the SuccessList.
        /// <para>NOTE: SuccessList and ErrorList messages are displayed to the user until lists are Pop()-ed.</para>
        /// </summary>
        static public List<string> PopSuccessList()
        {
            return Session.Pop<List<string>>("SuccessList");
        }
        /// <summary>
        /// Removes and returns the ErrorList.
        /// <para>NOTE: SuccessList and ErrorList messages are displayed to the user until lists are Pop()-ed.</para>
        /// </summary>
        static public List<string> PopErrorList()
        {
            return Session.Pop<List<string>>("ErrorList");
        }

        /* miscs */ 
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the culture of the current request, e.g. el-GR
        /// </summary>
        static public string GS(string Key, CultureInfo Culture = null)
        {
            return Res.GS(Key, Culture);
        }
        /// <summary>
        /// GSL = GetString with maxLen.
        /// Returns the Value value as string. 
        /// <para>The string is returned truncated if exceeds MaxLen. </para>
        /// <para>If the Value is null or empty, Value is returned.</para>
        /// </summary>
        static public string GSL(string Value, int MaxLen, bool ReturnEmptyIfNull = true)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return ReturnEmptyIfNull ? "" : Value;

            if (Value.Length > MaxLen)
                Value = Value.Remove(MaxLen - 1);

            return Value;
        }
        /// <summary>
        /// Formats a decimal value as a currency value. No Currency Symbol prefix or suffix.
        /// </summary>
        static public string FormatMoney(decimal Value, bool OnZeroReturnEmpty = false, string Format = "")
        {
            if (string.IsNullOrWhiteSpace(Format))
                Format = MoneyFormat;

            return Value <= 0 && OnZeroReturnEmpty ? string.Empty : Value.ToString(Format);
        }

        /// <summary>
        /// Creates and returns a list of <see cref="SelectListItem"/> items
        /// </summary>
        static public List<SelectListItem> CreateSelectList() { return new List<SelectListItem>(); }

        /// <summary>
        /// Returns a service specified by a type argument. If the service is not registered an exception is thrown.
        /// </summary>
        static public T GetService<T>()
        {
            return App.GetService<T>();
        }
        /// <summary>
        /// Returns the application settings
        /// </summary>
        /// <returns></returns>
        static public DataStoreSettings GetSettings()
        {
            return new DataStoreSettings();
        }
        /// <summary>
        /// Creates and returns a <see cref="HttpClient"/> using a <see cref="IHttpClientFactory"/>
        /// </summary>
        static public HttpClient CreateHttpClient()
        {
            IHttpClientFactory Factory = App.GetService<IHttpClientFactory>();
            return Factory.CreateClient();
        }
 
        /// <summary>
        /// Creates and returns a new ViewDataDictionary
        /// </summary>
        static public ViewDataDictionary GetViewDataDictionary(ViewDataDictionary Source = null)
        {
            if (Source == null)
            {
                IModelMetadataProvider ModelMetadataProvider = GetService<IModelMetadataProvider>();
                return new ViewDataDictionary(ModelMetadataProvider, new ModelStateDictionary());
            }

            return new ViewDataDictionary(Source);
        }
 
        /* properties */
        /// <summary>
        /// Represents the web application
        /// </summary>
        static public IWebAppContext App { get; private set; }
        /// <summary>
        /// Returns the request context
        /// </summary>
        static public IRequestContext RequestContext => App.GetService<IRequestContext>();
        /// <summary>
        /// Returns the <see cref="HttpContext"/>
        /// </summary>
        static public HttpContext HttpContext => RequestContext.HttpContext;
        /// <summary>
        /// The http request
        /// </summary>
        static public HttpRequest Request => HttpContext.Request;
        /// <summary>
        /// The query string as a collection of key-value pairs
        /// </summary>
        static public IQueryCollection Query => Request.Query;

        /// <summary>
        /// Format string for formatting money values
        /// </summary>
        static public string MoneyFormat => GetSettings().General.MoneyFormat;

 
        /// <summary>
        /// Returns true when HostEnvironment.IsDevelopment() returns true.
        /// </summary>
        static public bool DevMode => App.DevMode;
    }

}

namespace Tripous.Http
{
 
 
    /// <summary>
    /// A base "typed" <see cref="HttpClient"/>.
    /// <para>A typed <see cref="HttpClient"/> is a class that accepts a <see cref="HttpClient"/> instance in its constructor, 
    /// and uses that instance in order to call a HTTP service.</para>
    /// <para><strong>SEE:</strong> https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines</para>
    /// <para>To create the <see cref="HttpClient"/> instance for the constructor of this class, you may simply use the <see cref="HttpClient"/> constructor
    /// or use the IHttpClientFactory (requires the Microsoft.Extensions.Http NuGet package). </para>
    /// <para>Links: 
    /// <list type="bullet">
    /// <item>https://thomaslevesque.com/tag/httpmessagehandler/ </item>
    /// <item>https://blogs.msdn.microsoft.com/henrikn/2012/08/07/httpclient-httpclienthandler-and-webrequesthandler-explained/ </item>
    /// <item>https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests </item>
    /// </list>
    /// </para>
    /// <para>Do <strong>NOT</strong> use <c>using HttpClient client = new HttpClient();</c> because this causes a number of problems.  </para>
    /// Links:
    /// <list type="bullet">
    /// <item>https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/</item>
    /// <item>https://byterot.blogspot.com/2016/07/singleton-httpclient-dns.html</item>
    /// </list>
    /// </summary>
    public abstract class HttpClientBase<T>  where T: HttpClientResult
    {
        /* protected */
        /// <summary>
        /// The inner HttpClient
        /// </summary>
        protected HttpClient Client;

        /// <summary>
        /// Triggers the Error event.
        /// </summary>
        protected virtual void OnError(T ClientResult)
        {
            Error?.Invoke(this, ClientResult);
        }
        /// <summary>
        /// Triggers the ResultLoaded event
        /// </summary>
        protected virtual void OnResultLoaded(T ClientResult)
        {
            ResultLoaded?.Invoke(this, ClientResult);
        }

        /// <summary>
        /// Authenticates with the Api in order to get the access token. On succes it assignes the AccessToken property.
        /// </summary>
        protected abstract void Authenticate();

        /// <summary>
        /// Prepares the authentication headers
        /// </summary>
        protected abstract void PrepareAuthenticationHeaders(HttpClientCallInfo CallInfo);
        /// <summary>
        /// Prepares the accept and the authentication header
        /// </summary>
        protected virtual void PrepareHeaders(HttpClientCallInfo CallInfo)
        {
            PrepareHeadersBefore(CallInfo);

            if (Client.DefaultRequestHeaders.Accept == null || !Client.DefaultRequestHeaders.Accept.Any(m => m.MediaType == MediaTypeNames.Application.Json))
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            PrepareAuthenticationHeaders(CallInfo);
            PrepareHeadersAfter(CallInfo);
        }
        /// <summary>
        /// Called by the <see cref="PrepareHeaders"/>
        /// </summary>
        protected virtual void PrepareHeadersBefore(HttpClientCallInfo CallInfo)
        {
        }
        /// <summary>
        /// Called by the <see cref="PrepareHeaders"/>
        /// </summary>
        protected virtual void PrepareHeadersAfter(HttpClientCallInfo CallInfo)
        {
        }

        /// <summary>
        /// Called by action methods
        /// </summary>
        protected virtual void CallBefore(HttpClientCallInfo CallInfo)
        {
        }
        /// <summary>
        /// Called by action methods
        /// </summary>
        protected virtual void CallAfter(HttpClientCallInfo CallInfo)
        {
        }

        /// <summary>
        /// Creates and returns an Api Client Result
        /// </summary>
        protected virtual T CreateResult(string ActionUrl)
        {
            T Result = Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            return Result;
        }
        /// <summary>
        /// Creates and returns an Api Client Result
        /// </summary>
        protected virtual T CreateResult(HttpResponseMessage Response, string ActionUrl = "")
        {
            T Result = Activator.CreateInstance(typeof(T), new object[] { Response, ActionUrl }) as T;
            return Result;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public HttpClientBase()
        {
            this.Client = new HttpClient();
        }
        /// <summary>
        /// Constructor
        /// <para>To create the <see cref="HttpClient"/> instance for the constructor of this class, you may simply use the <see cref="HttpClient"/> constructor
        /// or use the IHttpClientFactory (requires the Microsoft.Extensions.Http NuGet package). </para>
        /// <para>For an Asp.Net Core example of IHttpClientFactory/<see cref="HttpClient"/> pair usage see  </para>
        /// <list type="bullet">
        /// <item>https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2 </item>
        /// <item>https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore </item>
        /// <item>https://www.talkingdotnet.com/3-ways-to-use-httpclientfactory-in-asp-net-core-2-1/ </item>
        /// <item>https://danieldonbavand.com/httpclientfactory-net-core-2-1/ </item>
        /// <item>https://devblogs.microsoft.com/aspnet/asp-net-core-2-1-preview1-introducing-httpclient-factory/ </item>
        /// </list>
        /// <para>
        /// For a .Net Core console example of IHttpClientFactory/<see cref="HttpClient"/> pair usage see
        /// <list type="bullet">
        /// <item>https://stackoverflow.com/questions/52622586/can-i-use-httpclientfactory-in-a-net-core-app-which-is-not-asp-net-core </item>
        /// <item>https://merbla.com/2018/04/25/exploring-serilog-v2---using-the-http-client-factory/ </item>
        /// <item>https://stackoverflow.com/questions/52918000/ihttpclientfactory-in-net-core-2-1-console-app-references-system-net-http </item>
        /// <item>https://github.com/aspnet/HttpClientFactory/blob/6619149a5d84bd0b60abf3c3f2abc7b334426fa1/samples/HttpClientFactorySample/Program.cs </item>
        /// </list>
        /// </para>
        /// </summary>
        public HttpClientBase(HttpClient Client)
        {
            this.Client = Client;
        }
        /// <summary>
        /// Constructor.
        /// <para>May be used when a client-sie <see cref="HttpMessageHandler"/> is needed.</para>
        /// Links:
        /// <list type="bullet">
        /// <item>https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/httpclient-message-handlers</item>
        /// <item>https://thomaslevesque.com/2016/12/08/fun-with-the-httpclient-pipeline/</item>
        /// <item>https://www.sharpencode.com/article/WebApi/http-message-handler-httpclient-message-handler</item>
        /// </list>
        /// </summary>
        public HttpClientBase(HttpMessageHandler MessageHandler)
            : this(new HttpClient(MessageHandler))
        {
        }
        /// <summary>
        /// Constructor.
        /// <para><strong>NOTE:</strong> Setting <see cref="HttpClient.BaseAddress"/> makes it impossible to reset it again. </para>
        /// </summary>
        public HttpClientBase(string BaseUrl)
            : this(new HttpClient() { BaseAddress = new Uri(BaseUrl) })
        {
        }



        /* public */
        /// <summary>
        /// Executes a GET Action to Api
        /// </summary>
        public async Task<T> GetAsync(string ActionUrl)
        {
            T Result = CreateResult(ActionUrl); // Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                if (!IsAuthenticated)
                    Sys.Throw("Http Client is not authenticated.");

                HttpClientCallInfo CallInfo = new HttpClientCallInfo(HttpClientCallType.Get, ActionUrl);
                PrepareHeaders(CallInfo);
                CallBefore(CallInfo);

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                using (HttpResponseMessage Response = await Client.GetAsync(ActionUrl).ConfigureAwait(false))
                {
                    //Result.Response = Response;
                    await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);                   

                    CallInfo.ClientResult = Result;
                    CallInfo.Response = Response;       
                    CallAfter(CallInfo);
                }
            }
            catch (Exception Ex)
            {
                Result.ReasonPhrase = Ex.Message;
                Result.IsSuccess = false;
            }

            if (!Result.IsSuccess)
                OnError(Result);
            else
                OnResultLoaded(Result);

            return Result;
        }
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        public async Task<T> PostFormAsync(string ActionUrl, Dictionary<string, string> FormData)
        {
            T Result = CreateResult(ActionUrl);
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                if (!IsAuthenticated)
                    Sys.Throw("Http Client is not authenticated.");

                HttpClientCallInfo CallInfo = new HttpClientCallInfo(HttpClientCallType.PostForm, ActionUrl, FormData);
                PrepareHeaders(CallInfo);
                CallBefore(CallInfo);

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                using (HttpResponseMessage Response = await Client.PostAsync(ActionUrl, new FormUrlEncodedContent(FormData)).ConfigureAwait(false))
                {
                    //Result.Response = Response;
                    await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);

                    CallInfo.ClientResult = Result;
                    CallInfo.Response = Response;
                    CallAfter(CallInfo);
                }

            }
            catch (Exception Ex)
            {
                Result.ReasonPhrase = Ex.Message;
                Result.IsSuccess = false;
            }

            if (!Result.IsSuccess)
                OnError(Result);
            else
                OnResultLoaded(Result);

            return Result;
        }
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        public async Task<T> PostAsync(string ActionUrl, object Packet)
        {
            T Result = CreateResult(ActionUrl);
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                if (!IsAuthenticated)
                    Sys.Throw("Http Client is not authenticated.");

                HttpClientCallInfo CallInfo = new HttpClientCallInfo(HttpClientCallType.Post, ActionUrl, Packet);
                PrepareHeaders(CallInfo);               
                CallBefore(CallInfo);

                string JsonText = Json.ToJson(Packet);
                StringContent Content = new StringContent(JsonText, Encoding.UTF8, MediaTypeNames.Application.Json);

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                using (HttpResponseMessage Response = await Client.PostAsync(ActionUrl, Content).ConfigureAwait(false))
                {
                    //Result.Response = Response;
                    await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);

                    CallInfo.ClientResult = Result;
                    CallInfo.Response = Response;
                    CallAfter(CallInfo);
                }

            }
            catch (Exception Ex)
            {
                Result.ReasonPhrase = Ex.Message;
                Result.IsSuccess = false;
            }

            if (!Result.IsSuccess)
                OnError(Result);
            else
                OnResultLoaded(Result);

            return Result;
        }
        /// <summary>
        /// Executes a PUT Action to Api
        /// </summary>
        public async Task<T> PutAsync(string ActionUrl, object Packet)
        {
            T Result = CreateResult(ActionUrl);
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                if (!IsAuthenticated)
                    Sys.Throw("Http Client is not authenticated.");

                HttpClientCallInfo CallInfo = new HttpClientCallInfo(HttpClientCallType.Put, ActionUrl, Packet);
                PrepareHeaders(CallInfo);                
                CallBefore(CallInfo);

                string JsonText = Json.ToJson(Packet);
                StringContent Content = new StringContent(JsonText, Encoding.UTF8, MediaTypeNames.Application.Json);

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                using (HttpResponseMessage Response = await Client.PutAsync(ActionUrl, Content).ConfigureAwait(false))
                {
                    //Result.Response = Response;
                    await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);

                    CallInfo.ClientResult = Result;
                    CallInfo.Response = Response;
                    CallAfter(CallInfo);
                }
            }
            catch (Exception Ex)
            {
                Result.ReasonPhrase = Ex.Message;
                Result.IsSuccess = false;
            }

            if (!Result.IsSuccess)
                OnError(Result);
            else
                OnResultLoaded(Result);

            return Result;

        }
        /// <summary>
        /// Executes a DELETE Action to Api
        /// </summary>
        public async Task<T> DeleteAsync(string ActionUrl)
        {
            T Result = CreateResult(ActionUrl);
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                if (!IsAuthenticated)
                    Sys.Throw("Http Client is not authenticated.");

                HttpClientCallInfo CallInfo = new HttpClientCallInfo(HttpClientCallType.Delete, ActionUrl);
                PrepareHeaders(CallInfo);                
                CallBefore(CallInfo);

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                using (HttpResponseMessage Response = await Client.DeleteAsync(ActionUrl).ConfigureAwait(false))
                {
                    //Result.Response = Response;
                    await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);

                    CallInfo.ClientResult = Result;
                    CallInfo.Response = Response;
                    CallAfter(CallInfo);
                }

            }
            catch (Exception Ex)
            {
                Result.ReasonPhrase = Ex.Message;
                Result.IsSuccess = false;
            }

            if (!Result.IsSuccess)
                OnError(Result);
            else
                OnResultLoaded(Result);

            return Result;
        }

        /// <summary>
        /// Executes a GET Action to Api
        /// </summary>
        public T Get(string ActionUrl)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            return Sys.TaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return GetAsync(ActionUrl);
            }).Unwrap().GetAwaiter().GetResult();
        }
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        public T PostForm(string ActionUrl, Dictionary<string, string> FormData)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            return Sys.TaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return PostFormAsync(ActionUrl, FormData);
            }).Unwrap().GetAwaiter().GetResult();
        }
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        public T Post(string ActionUrl, object Packet)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            return Sys.TaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return PostAsync(ActionUrl, Packet);
            }).Unwrap().GetAwaiter().GetResult();
        }
        /// <summary>
        /// Executes a PUT Action to Api
        /// </summary>
        public T Put(string ActionUrl, object Packet)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            return Sys.TaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return PutAsync(ActionUrl, Packet);
            }).Unwrap().GetAwaiter().GetResult();
        }
        /// <summary>
        /// Executes a DELETE Action to Api
        /// </summary>
        public T Delete(string ActionUrl)
        {
            CultureInfo UiCulture = CultureInfo.CurrentUICulture;
            CultureInfo Culture = CultureInfo.CurrentCulture;

            return Sys.TaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = UiCulture;
                return DeleteAsync(ActionUrl);
            }).Unwrap().GetAwaiter().GetResult();
        }

 
        /* properties */
        /// <summary>
        /// The access token returned by Api on authentication
        /// </summary>
        public string AccessToken { get; protected set; }
        /// <summary>
        /// True when the client is authenticated and the access token is not null or empty.
        /// </summary>
        public virtual bool IsAuthenticated { get { return !string.IsNullOrWhiteSpace(AccessToken); } }
 


        /* events */
        /// <summary>
        /// Occurs on Api errors
        /// </summary>
        public event EventHandler<T> Error;
        /// <summary>
        /// Occurs when the result is loaded from the web response.
        /// </summary>
        public event EventHandler<T> ResultLoaded;
    }


}

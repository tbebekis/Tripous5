using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
 

namespace Tripous
{
    /// <summary>
    /// Represents a "typed" http client. Covers the <see cref="HttpClientBase{T}" /> class.
    /// </summary>
    public interface IHttpClientBase<T> where T : HttpClientResult
    {
        /// <summary>
        /// Executes a GET Action to Api
        /// </summary>
        Task<T> GetAsync(string ActionUrl);
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        Task<T> PostFormAsync(string ActionUrl, Dictionary<string, string> FormData);
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        Task<T> PostAsync(string ActionUrl, object Packet);
        /// <summary>
        /// Executes a PUT Action to Api
        /// </summary>
        Task<T> PutAsync(string ActionUrl, object Packet);
        /// <summary>
        /// Executes a DELETE Action to Api
        /// </summary>
        Task<T> DeleteAsync(string ActionUrl);

        /* properties */
        /// <summary>
        /// The access token returned by Api on authentication
        /// </summary>
        string AccessToken { get; }
        /// <summary>
        /// True when the client is authenticated and the access token is not null or empty.
        /// </summary>
        bool IsAuthenticated { get; }

        /* events */
        /// <summary>
        /// Occurs on Api errors
        /// </summary>
        event EventHandler<T> Error;
    }



    /// <summary>
    /// A base "typed" <see cref="HttpClient"/>.
    /// <para>A typed <see cref="HttpClient"/> is a class that accepts a <see cref="HttpClient"/> instance in its constructor, 
    /// and uses that instance in order to call a HTTP service.</para>
    /// <para>To create the <see cref="HttpClient"/> instance for the constructor of this class, you may simply use the <see cref="HttpClient"/> constructor
    /// or use the IHttpClientFactory (requires the Microsoft.Extensions.Http NuGet package). </para>
    /// <para>Links: 
    /// <list type="bullet">
    /// <item>https://thomaslevesque.com/tag/httpmessagehandler/ </item>
    /// <item>https://blogs.msdn.microsoft.com/henrikn/2012/08/07/httpclient-httpclienthandler-and-webrequesthandler-explained/ </item>
    /// <item>https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests </item>
    /// </list>
    /// </para>
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
        protected abstract void PrepareAuthenticationHeaders();
        /// <summary>
        /// Prepares the accept and the authentication header
        /// </summary>
        protected virtual void PrepareHeaders()
        {
            if (Client.DefaultRequestHeaders.Accept == null || !Client.DefaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            PrepareAuthenticationHeaders();
            
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
        public HttpClientBase(HttpClient Client = null)
        {
            this.Client = Client ?? new HttpClient();
        }
        /// <summary>
        /// Constructor
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
            T Result = Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                HttpResponseMessage Response = await Client.GetAsync(ActionUrl).ConfigureAwait(false);
                await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);
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
            T Result =  Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                HttpResponseMessage Response = await Client.PostAsync(ActionUrl, new FormUrlEncodedContent(FormData)).ConfigureAwait(false);
                await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);
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
            T Result = Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                string JsonText = Json.ToJson(Packet);
                StringContent Content = new StringContent(JsonText, Encoding.UTF8, "application/json");

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                HttpResponseMessage Response = await Client.PostAsync(ActionUrl, Content).ConfigureAwait(false);
                await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);
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
            T Result = Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                string JsonText = Json.ToJson(Packet);
                StringContent Content = new StringContent(JsonText, Encoding.UTF8, "application/json");

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                HttpResponseMessage Response = await Client.PutAsync(ActionUrl, Content).ConfigureAwait(false);
                await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);
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
            T Result = Activator.CreateInstance(typeof(T), new object[] { ActionUrl }) as T;
            try
            {
                if (!IsAuthenticated)
                    Authenticate();

                // CAUTION: In WinForms the await may deadlock threads. Use ConfigureAwait() to avoid it.
                // SEE: https://stackoverflow.com/a/10369275/1779320
                // SEE ALSO: https://blog.stephencleary.com/2012/02/async-and-await.html
                HttpResponseMessage Response = await Client.DeleteAsync(ActionUrl).ConfigureAwait(false);
                await Result.LoadFromResponseAsync(Response).ConfigureAwait(false);
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

namespace Tripous.Http
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

        /// <summary>
        /// Executes a GET Action to Api
        /// </summary>
        T Get(string ActionUrl);
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        T PostForm(string ActionUrl, Dictionary<string, string> FormData);
        /// <summary>
        /// Executes a POST Action to Api
        /// </summary>
        T Post(string ActionUrl, object Packet);
        /// <summary>
        /// Executes a PUT Action to Api
        /// </summary>
        T Put(string ActionUrl, object Packet);
        /// <summary>
        /// Executes a DELETE Action to Api
        /// </summary>
        T Delete(string ActionUrl);

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

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Claims;

using Microsoft.Net.Http.Headers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.StaticFiles;

using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Tripous;
using Tripous.Data;

namespace WebLib.AspNet
{

    /// <summary>
    /// Helper
    /// </summary>
    static public class WSys
    {
        static IServiceProvider fRootServiceProvider;

        /* private */
        /// <summary>
        /// FROM: https://stackoverflow.com/questions/13086856/mobile-device-detection-in-asp-net
        /// </summary>
        static Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        /// <summary>
        /// FROM: https://stackoverflow.com/questions/13086856/mobile-device-detection-in-asp-net
        /// </summary>
        static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        /// <summary>
        /// FROM: https://stackoverflow.com/questions/7576508/how-to-detect-crawlers-in-asp-net-mvc
        /// </summary>
        static Regex CrawlerCheck = new Regex(@"bot|crawler|baiduspider|80legs|ia_archiver|voyager|curl|wget|yahoo! slurp|mediapartners-google", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        /* construction */
        /// <summary>
        /// Static constructor
        /// </summary>
        static WSys()
        {
        }


        /* IServiceCollection */
        /// <summary>
        /// Returns a service specified by a type argument. If the service is not registered an exception is thrown.
        /// <para>WARNING: "Scoped" services can NOT be resolved from the "root" service provider. </para>
        /// <para>There are two solutions to the "Scoped" services problem:</para>
        /// <para> ● Use <c>HttpContext.RequestServices</c>, a valid solution since we use a "Scoped" service provider to create the service,  </para>
        /// <para> ● or add <c> .UseDefaultServiceProvider(options => options.ValidateScopes = false)</c> in the <c>CreateHostBuilder</c>() of the Program class</para>
        /// <para>see: https://github.com/dotnet/runtime/issues/23354 and https://devblogs.microsoft.com/dotnet/announcing-ef-core-2-0-preview-1/ </para>
        /// </summary>
        public static T GetService<T>()
        {
            return HttpContext != null ? HttpContext.RequestServices.GetRequiredService<T>() : RootServiceProvider.GetRequiredService<T>();
        }
        /// <summary>
        /// Replaces one service with another
        /// </summary>
        static public void ReplaceService(IServiceCollection Services, Type Original, Type Replacer, ServiceLifetime LifeTime = ServiceLifetime.Scoped)
        {
            var descriptor = new ServiceDescriptor(Original, Replacer, LifeTime);
            Services.Replace(descriptor);
        }

        /// <summary>
        /// Returns the current <see cref="ActionContext"/>.
        /// <para>WARNING: It should be called only when a valid <see cref="Microsoft.AspNetCore.Http.HttpRequest"/> exists. </para>
        /// </summary>
        static public ActionContext GetActionContext()
        {
            IActionContextAccessor service = GetService<IActionContextAccessor>();
            return (service != null) ? service.ActionContext : null;
        }
        /// <summary>
        /// Returns an <see cref="IUrlHelper"/>.
        /// <para>WARNING: It should be called only when a valid <see cref="Microsoft.AspNetCore.Http.HttpRequest"/> exists. </para>
        /// </summary>
        static public IUrlHelper GetUrlHelper()
        {
            ActionContext context = GetActionContext();
            IUrlHelperFactory factory = GetService<IUrlHelperFactory>();
            return (context != null && factory != null) ? factory.GetUrlHelper(context) : null;
        }

        /* Db */
        /// <summary>
        /// <para>After calling this method the <see cref="Db.Connections"/> is loaded from the appsettings.json file with Sql database connection information. </para>
        /// </summary>
        static public void AddSqlStores(this IServiceCollection Services, IConfiguration Configuration, string SectionName = "SqlConnections")
        {
            List<SqlConnectionInfo> Connections = new List<SqlConnectionInfo>();
            IConfigurationSection ConnectionsSection = Configuration.GetSection(SectionName);
            // services.Configure<List<SqlConnectionInfo>>(ConnectionsSection); // do we need to register settings with the DI?
            ConnectionsSection.Bind(Connections);

            Db.Connections = Connections;
        }

        /* google maps */
        /// <summary>
        /// Returns a Google Maps Url for a specified Query, for either a normal Map view or Sattelite view and a specified Zoom (1 to 20)
        /// <para>Query could be a full address or just a City or a Company name and City or any other combination.</para>
        /// <para>EXAMPLE: 1600 Pennsylvania Avenue; NW Washington, D.C. 20500 </para>
        /// <para>EXAMPLE: White House, Washington</para>
        /// <para>EXAMPLE: Baufox, Ηπείρου, Καλοχώρι, Θεσσαλονίκη, TK 57009 </para>
        /// <para>EXAMPLE: Baufox, Αθήνα</para>
        /// <para>Query could also have the format loc:LATITUDE+LONGTITUDE </para>
        /// <para>EXAMPLE: loc:40.62641513792309+22.948322824856376 </para>
        /// </summary>
        static public string GetGoogleMapQueryUrl(string Query, bool SatelliteView = false, int Zoom = 0)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append(@"https://maps.google.com/maps?");
            SB.Append("q=");
            if (!Query.Contains("loc:"))
                Query = HttpUtility.UrlEncode(Query, Encoding.UTF8);
            SB.Append(Query);
            SB.Append(SatelliteView ? "&t=k" : "&t=m");
            if (Zoom > 0 && Zoom <= 20)
                SB.Append($"&z={Zoom}");
            string Url = SB.ToString();
            return Url;
        }

        /* query string */
        /// <summary>
        /// Returns a value from query string, if any, else returns a default value.
        /// </summary>
        static public string GetQueryValue(string Key, string Default = "")
        {

            try
            {
                IQueryCollection QS = Query;
                return QS != null && QS.ContainsKey(Key) ? QS[Key].ToString() : Default;
            }
            catch
            {
            }

            return Default;

        }
        /// <summary>
        /// Returns a value from query string, if any, else returns a default value.
        /// </summary>
        static public int GetQueryValue(string Key, int Default = 0)
        {
            try
            {
                string S = GetQueryValue(Key, "");
                return !string.IsNullOrWhiteSpace(S) ? Convert.ToInt32(S) : Default;
            }
            catch
            {
            }

            return Default;
        }
        /// <summary>
        /// Returns a value from query string, if any, else returns a default value.
        /// </summary>
        static public bool GetQueryValue(string Key, bool Default = false)
        {
            try
            {
                string S = GetQueryValue(Key, "");
                return !string.IsNullOrWhiteSpace(S) ? Convert.ToBoolean(S) : Default;
            }
            catch
            {
            }

            return Default;

        }

        /// <summary>
        /// Returns the value of a query string parameter.
        /// <para>NOTE: When a parameter is included more than once, e.g. ?page=1&amp;page=2 then the result will be 1,2 hence this function returns an array.</para>
        /// </summary>
        static public string[] GetQueryValueArray(string Key)
        {
            try
            {
                if (HttpContext != null)
                {
                    if (HttpContext.Request.Query.ContainsKey(Key))
                        return HttpContext.Request.Query[Key].ToArray();
                }
            }
            catch
            {
            }

            return new string[0];
        }

        /* claims */
        /// <summary>
        /// Returns true if a claim exists in a sequence of claims
        /// </summary>
        static public bool ContainsClaim(IEnumerable<Claim> Claims, string TokenType)
        {
            return FindClaim(Claims, TokenType) != null;
        }
        /// <summary>
        /// Finds and returns a claim, if a claim exists in a sequence of claims, else null.
        /// </summary>
        static public Claim FindClaim(IEnumerable<Claim> Claims, string TokenType)
        {
            TokenType = TokenType.ToLowerInvariant();
            return  Claims.FirstOrDefault(item => item.Type.ToLowerInvariant() == TokenType);
        }
        /// <summary>
        /// Returns the value of a claim, if a claim exists in a sequence of claims, else null.
        /// </summary>
        static public string GetClaimValue(IEnumerable<Claim> Claims, string TokenType)
        {
            Claim Claim = FindClaim(Claims, TokenType);
            return Claim != null ? Claim.Value : null;
        }

        /* miscs */
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the culture code of the current request, e.g. el-GR
        /// </summary>
        static public string Localize(string Key)
        {
            return LocalizeFunc != null ? LocalizeFunc(Key) : Key;
        }

        /// <summary>
        /// Creates and returns a <see cref="HttpClient"/> using a <see cref="IHttpClientFactory"/>
        /// </summary>
        static public HttpClient CreateHttpClient()
        {
            IHttpClientFactory Factory = GetService<IHttpClientFactory>();
            return Factory.CreateClient();
        }

        /// <summary>
        /// Returns a <see cref="FileContentResult"/> for downloading a file or null.
        /// <para>Null is returned when no data is passed (null or length = 0) and the file does not exist.</para>
        /// <para>NOTE: If no binary Data is specified then the function tries to load the binary data from the specified file path. </para>
        /// <para>CAUTION: FilePath is mandatory.</para>
        /// </summary>
        static public FileContentResult GetFileContentResult(string FilePath, byte[] Data = null)
        {
            FileContentResult Result = null;

            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                if (Data == null || Data.Length <= 0)
                {
                    if (File.Exists(FilePath))
                        Data = File.ReadAllBytes(FilePath);
                }

                if (Data != null && Data.Length > 0)
                {
                    string FileName = Path.GetFileName(FilePath);
                    string ContentType;
                    new FileExtensionContentTypeProvider().TryGetContentType(FileName, out ContentType);
                    ContentType = ContentType ?? "application/octet-stream";

                    Result = new FileContentResult(Data, ContentType);
                    Result.FileDownloadName = FileName;
                }
            }

            return Result;

        }
        /// <summary>
        /// Returns a <see cref="FileContentResult"/> for downloading a file or null.
        /// <para>Null is returned when no data is passed (null or length = 0) and the file does not exist.</para>
        /// <para>NOTE: If no binary Data is specified then the function tries to load the binary data from the specified file path. </para>
        /// <para>CAUTION: FilePath is mandatory.</para>
        /// </summary>
        static public FileContentResult GetFileContentResult(string FilePath, Stream Stream)
        {
            byte[] Data = Streams.ToArray(Stream);
            return GetFileContentResult(FilePath, Data);
        }

        /// <summary>
        /// Reads and returns an HTTP header from <see cref="HttpRequest.Headers"/>
        /// </summary>
        static public string GetHttpHeader(this HttpRequest Request, string Key)
        {
            Key = Key.ToLowerInvariant();
            return Request == null ? string.Empty : Request.Headers.FirstOrDefault(x => x.Key.ToLowerInvariant() == Key).Value.FirstOrDefault();
        }

        /// <summary>
        /// Returns the referrer Url if any, else null.
        /// <para>NOTE: The HTTP referer is an optional HTTP header field that identifies the address of the webpage which is linked to the resource being requested. 
        /// By checking the referrer, the new webpage can see where the request originated</para>
        /// </summary>
        static public string GetReferrerUrl(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
                return R.Headers[HeaderNames.Referer];

            return null;
        }
        /// <summary>
        /// Returns the client IP address, that is the IP address of the visitor, if any, else null
        /// </summary>
        static public string GetClientIpAddress(HttpRequest R = null)
        {
            string Result = null;

            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
            {
                // first try to get the IP address from the X-Forwarded-For header
                // SEE: https://en.wikipedia.org/wiki/X-Forwarded-For
                var SV = R.Headers["X-Forwarded-For"];
                if (!StringValues.IsNullOrEmpty(SV))
                    Result = SV.FirstOrDefault();

                // next try the remote IP address
                if (string.IsNullOrWhiteSpace(Result) && R.HttpContext.Connection.RemoteIpAddress != null)
                    Result = R.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            // check to see if it is the IPv6 Loopback address
            if (!string.IsNullOrWhiteSpace(Result) && Result.Equals(IPAddress.IPv6Loopback.ToString(), StringComparison.InvariantCultureIgnoreCase))
                Result = IPAddress.Loopback.ToString();

            // remove the port if there
            if (!string.IsNullOrWhiteSpace(Result) && Result.Contains(':'))
                Result = Result.Split(':').FirstOrDefault();

            return Result;
        }

        /// <summary>
        /// Returns the domain name of the server and the TCP port number on which the server is listening. 
        /// The port number may be omitted if the port is the standard port for the service requested. 
        /// </summary>
        static public string GetHostDomainName(HttpRequest R = null)
        {
            string Result = null;

            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
            {
                Result = R.Headers[HeaderNames.Host];
            }

            return Result;
        }

        /// <summary>
        /// Returns the scheme of the current request, i.e. https or http
        /// </summary>
        static public string GetRequestProtocol(HttpRequest R = null)
        {
            return IsHttps(R) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

 
        /// <summary>
        /// Returns the raw relative Url path and query string of a specified request
        /// <note>SEE: https://stackoverflow.com/questions/28120222/get-raw-url-from-microsoft-aspnet-http-httprequest </note>
        /// </summary>
        static public string GetRelativeRawUrl(HttpRequest R = null)
        {
            string Result = string.Empty;

            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            Result = R.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;
 
            if (string.IsNullOrWhiteSpace(Result))
            {
                Result = R.Path.Value;
                Result = !string.IsNullOrWhiteSpace(Result) ? Uri.UnescapeDataString(Result) : string.Empty;
            }                 

            return Result;
        }
        /// <summary>
        /// Returns the relative Url of a request, along with the Query String, url-encoded.
        /// <note>SEE: https://stackoverflow.com/questions/28120222/get-raw-url-from-microsoft-aspnet-http-httprequest </note>
        /// </summary>
        static public string GetRelativeRawUrlEncoded(HttpRequest R = null)
        {
            return GetRelativeRawUrl(R).UrlEncode();
        }
        /// <summary>
        /// Returns the absolute Url of a request, along with the Query String, url-encoded.
        /// <para>Suitable for use in HTTP headers and other HTTP operations.</para>
        /// </summary>
        static public string GetAbsoluteUrlEncoded(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            return R.GetEncodedUrl();
        }
        /// <summary>
        /// Returns the combined components of the request URL in a fully un-escaped form (except for the QueryString) suitable only for display. 
        /// <para>This format should not be used in HTTP headers or other HTTP operations.</para>
        /// </summary>
        static public string GetAbsoluteDisplayUrl(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            return R.GetDisplayUrl();
        }

        /// <summary>
        /// Returns the absolute Url (e.g. containing scheme and host name) of a specified Route name
        /// </summary>
        static public string GetAbsoluteRouteUrl(IUrlHelper UrlHelper, string RouteName, object RouteValues = null)
        {
            string Scheme = HttpRequest.Scheme;
            return UrlHelper.RouteUrl(RouteName, RouteValues, Scheme);
        }

        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        static public string UrlEncode(string Url)
        {
            return System.Web.HttpUtility.UrlEncode(Url);
        }

        /// <summary>
        /// Escapes a url using the <see cref="Uri.EscapeUriString(string)"/>.
        /// <para>The <see cref="Uri.EscapeUriString(string)"/> escapes unreserved characters only..</para>
        /// <para>The <see cref="Uri.EscapeDataString(string)"/> escapes unreserved AND reserved charactes.</para>
        /// <para>Reserved Characters: :/?#[]@!$&amp;'()*+,;=  </para>
        /// <para>Unreserved Characters: alphanumeric and -._~ </para>
        /// <para>SEE: https://tools.ietf.org/html/rfc3986#section-2 </para>
        /// </summary>
        static public string UrlEscape(string Url)
        {
            return Uri.EscapeUriString(Url);
        }
        /// <summary>
        /// Escapes a url using the <see cref="Uri.EscapeDataString(string)"/>.
        /// <para>The <see cref="Uri.EscapeUriString(string)"/> escapes unreserved characters only..</para>
        /// <para>The <see cref="Uri.EscapeDataString(string)"/> escapes unreserved AND reserved charactes.</para>
        /// <para>Reserved Characters: :/?#[]@!$&amp;'()*+,;= </para>
        /// <para>Unreserved Characters: alphanumeric and -._~ </para>
        /// <para>SEE: https://tools.ietf.org/html/rfc3986#section-2 </para>
        /// </summary>
        static public string UrlEscapeAll(string Url)
        {
            return Uri.EscapeDataString(Url);
        }

        /// <summary>
        /// Returns true if the RequestScheme is https.
        /// </summary>
        static public bool IsHttps(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
            {
                return R.Headers["X-Forwarded-Proto"].ToString().Equals("https", StringComparison.OrdinalIgnoreCase)
                    || R.IsHttps;
            }

            return false;
        }
        /// <summary>
        /// Gets whether the specified HTTP request URI references the local host.
        /// </summary>
        /// <param name="R">HTTP request</param>
        /// <returns>True, if HTTP request URI references to the local host</returns>
        static public bool IsLocalRequest(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
            {
                // SEE: https://stackoverflow.com/questions/35240586/in-asp-net-core-how-do-you-check-if-request-is-local/
                ConnectionInfo CI = R.HttpContext.Connection;
                if (CI.RemoteIpAddress != null)
                {
                    return CI.LocalIpAddress != null ? CI.RemoteIpAddress.Equals(CI.LocalIpAddress) : IPAddress.IsLoopback(CI.RemoteIpAddress);
                }
            }


            return true;
        }
        /// <summary>
        /// Returns true if a specified request is an ajax request
        /// </summary>
        static public bool IsAjax(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            return R != null ? R.Headers["X-Requested-With"] == "XMLHttpRequest" : false;
        }

        /// <summary>
        /// Returns true if we are dealing with a mobile device/browser
        /// <para>FROM: https://stackoverflow.com/questions/13086856/mobile-device-detection-in-asp-net </para>
        /// </summary>
        static public bool IsMobile(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
            {
                string S = R.Headers[Microsoft.Net.Http.Headers.HeaderNames.UserAgent].ToString();
                return S.Length >= 4 && (MobileCheck.IsMatch(S) || MobileVersionCheck.IsMatch(S.Substring(0, 4)));
            }

            return false;
        }
        /// <summary>
        /// Returns true if we are dealing with a search endine bot.
        /// <para>FROM: https://stackoverflow.com/questions/7576508/how-to-detect-crawlers-in-asp-net-mvc  </para>
        /// </summary>
        static public bool IsCrawler(HttpRequest R = null)
        {
            if (R == null && WSys.IsRequestAvailable)
                R = WSys.HttpRequest;

            if (R != null)
            {
                string S = R.Headers[Microsoft.Net.Http.Headers.HeaderNames.UserAgent].ToString();
                return S.Length >= 4 && CrawlerCheck.IsMatch(S);
            }

            return false;
        }

        /* properties */
        /// <summary>
        /// Returns the base url of this application.
        /// <para>CAUTION: There should be a valid HttpContext in order to be able to return the base url.</para>
        /// </summary>
        static public string BaseUrl
        {
            get
            {
                if (HttpContext != null)
                {
                    string Scheme = HttpContext.Request.Scheme;
                    string Host = HttpContext.Request.Host.Host;
                    string Port = HttpContext.Request.Host.Port != 80 && HttpContext.Request.Host.Port != 443 ? $":{HttpContext.Request.Host.Port}" : "";

                    return $"{Scheme}://{Host}{Port}";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        static public string RootPath { get { return HostEnvironment.ContentRootPath; } }
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        static public string WebRootPath { get { return HostEnvironment.WebRootPath; } }
        /// <summary>
        /// The physical path of the \bin folder
        /// <para>e.g. C:\MyApp\bin\Debug\netcoreapp3.0\  </para>
        /// <para>e.g. C:\inetpub\wwwroot\bin\</para>
        /// </summary>
        static public string BinPath { get { return AppContext.BaseDirectory; } }

        /// <summary>
        /// Gets or sets the root <see cref="IServiceProvider"/>
        /// </summary>
        static public IServiceProvider RootServiceProvider
        {
            get { return fRootServiceProvider; }
            set
            {
                fRootServiceProvider = value;
                if (fRootServiceProvider != null)
                {
                    HttpContextAccessor = RootServiceProvider.GetRequiredService<IHttpContextAccessor>();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="IHttpContextAccessor"/>
        /// </summary>
        static public IHttpContextAccessor HttpContextAccessor { get; set; }
        /// <summary>
        /// Returns the HttpContext
        /// </summary>
        static public HttpContext HttpContext { get { return HttpContextAccessor != null ? HttpContextAccessor.HttpContext : null; } }
        /// <summary>
        /// Returns the current HTTP request, if any, else null.
        /// </summary>
        static public HttpRequest HttpRequest
        {
            get
            {
                try
                {
                    return HttpContextAccessor != null && HttpContextAccessor.HttpContext != null ? HttpContextAccessor.HttpContext.Request : null;
                }
                catch
                {
                }

                return null;
            }
        }
        /// <summary>
        /// Returns true if an HTTP Request is currently available
        /// </summary>
        static public bool IsRequestAvailable { get { return HttpRequest != null; } }
        /// <summary>
        /// Indicates whether the client is being redirected to a new location.
        /// </summary>
        static public bool IsRequestBeingRedirected
        {
            get
            {
                /* NOTE:
                 * The HttpContext.Response of the old ASP.NET MVC had the boolean property IsRequestBeingRedirected
                 * which indicated whether a redirection is done.
                 * 
                 * Asp.Net Core has no such a property. 
                 * So we check the HttpResponse.StatusCode to detect a redirection.
                 * 
                 * SEE: https://en.wikipedia.org/wiki/HTTP_301
                 * SEE: https://en.wikipedia.org/wiki/HTTP_302
                 */
                int HttpResponseStatusCode = HttpContext.Response.StatusCode;
                int[] RedirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found };
                return RedirectionStatusCodes.Contains(HttpResponseStatusCode);
            }
        }
        /// <summary>
        /// The query string as a collection of key-value pairs
        /// </summary>
        static public IQueryCollection Query => HttpRequest.Query;

        /// <summary>
        /// Gets or sets the <see cref="IWebHostEnvironment"/>
        /// </summary>
        static public IWebHostEnvironment HostEnvironment { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="IConfiguration"/>
        /// </summary>
        static public IConfiguration Configuration { get; set; }
        /// <summary>
        /// True when is development environment.
        /// </summary>
        static public bool IsDevelopment { get { return HostEnvironment == null? true: HostEnvironment.IsDevelopment(); } }


        /// <summary>
        /// Delegate used by the Localize() method
        /// </summary>
        static public Func<string, string> LocalizeFunc { get; set; }
    }

}

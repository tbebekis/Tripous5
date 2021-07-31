using System;
using System.Text;

using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

using Tripous;

namespace WebDesk.AspNet
{

    public class RequestInfo
    {
        /* construction */
        public RequestInfo()
        {
            DateTime DT = DateTime.UtcNow;
            Date = DT.ToString("yyyy-MM-dd");
            Time = DT.ToString("HH:mm:ss");
        }
        public RequestInfo(HttpContext Context)
            : this()
        {
            string S;

            HttpRequest Request = Context.Request; 
            RequestUrl = WSys.GetRelativeRawUrl(Request);  

            QueryString = Request.QueryString.Value;

            if (Request.RouteValues != null && Request.RouteValues.Count > 0)
            {
                StringBuilder SB = new StringBuilder();

                foreach (var Entry in Request.RouteValues)
                {
                    if (SB.Length > 0)
                        SB.Append(", ");
                    S = Entry.Value != null ? Entry.Value.ToString() : "null";
                    SB.Append($"{Entry.Key}: {S}");
                }

                RouteValues = SB.ToString();
            }

            Method = Request.Method;

            UserAgent = Request.Headers.ContainsKey(HeaderNames.UserAgent) ? Request.Headers[HeaderNames.UserAgent].ToString() : string.Empty;

            S = WSys.GetReferrerUrl(Request);
            ReferrerUrl = !string.IsNullOrWhiteSpace(S) ? Uri.UnescapeDataString(S) : string.Empty;
            RemoteIp = WSys.GetClientIpAddress(Request);
            Protocol = WSys.GetRequestProtocol(Request);
            IsAjax = WSys.IsAjax(Request);
            IsCrawler = WSys.IsCrawler(Request);

        }

        /* public */
        public override string ToString()
        {
            return $"{Method} {RequestUrl} {QueryString} FROM ({RemoteIp}) {ReferrerUrl}";
        }
        public string[] ToLines()
        {
            string JsonText = Json.ToJson(this);
            JsonText = JsonText.Trim('{', '}').Trim();
            string[] Result = JsonText.Split(Environment.NewLine);
            for (int i = 0; i < Result.Length; i++)
                Result[i] = Result[i].Trim();
            return Result;
        }
        public void WriteTo(StringBuilder SB)
        {
            string[] Lines = ToLines();
            foreach (string Line in Lines)
                SB.AppendLine(Line);
        }

        /* properties */
        public string RemoteIp { get; set; }
        public string ReferrerUrl { get; set; }

        public string RequestUrl { get; set; }
        public string QueryString { get; set; }
        public string RouteValues { get; set; }
        public string Method { get; set; }
        public string UserAgent { get; set; }

        public string Protocol { get; set; }
        public bool IsAjax { get; set; }
        public bool IsCrawler { get; set; }

        public string Date { get; }
        public string Time { get; }

    }

}

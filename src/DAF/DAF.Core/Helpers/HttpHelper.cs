using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Net.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace DAF.Core
{
    public class HttpHelper
    {
        public static string Get(string url, CookieCollection cookies = null, string userAgent = null, Uri referer = null, string language = null, Dictionary<string, string> otherHeaders = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;
            string html = string.Empty;
            try
            {
                var http = CreateHttpClient(cookies, userAgent, referer, language, url.ToLower().StartsWith("https://"), otherHeaders);
                html = http.GetStringAsync(url).Result;
            }
            catch (Exception ex)
            {
                html = string.Format("Service {0} throws an error: {1}", url, ex.Message);
            }

            return html;
        }

        public static string Post(string url, string json, CookieCollection cookies = null, string userAgent = null, Uri referer = null, string language = null, Dictionary<string, string> otherHeaders = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;
            string html = string.Empty;
            try
            {
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var http = CreateHttpClient(cookies, userAgent, referer, language, url.ToLower().StartsWith("https://"), otherHeaders);
                var postResult = http.PostAsync(url, content).Result;
                html = postResult.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                html = string.Format("Service {0} throws an error: {1}", url, ex.Message);
            }

            return html;
        }

        public static string Post<T>(string url, T data, CookieCollection cookies = null, string userAgent = null, Uri referer = null, string language = null, Dictionary<string, string> otherHeaders = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;
            string html = string.Empty;
            try
            {
                var http = CreateHttpClient(cookies, userAgent, referer, language, url.ToLower().StartsWith("https://"), otherHeaders);
                var postResult = http.PostAsJsonAsync(url, data).Result;
                html = postResult.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                html = string.Format("Service {0} throws an error: {1}", url, ex.Message);
            }

            return html;
        }

        public static string Post(string url, object obj, CookieCollection cookies = null, string userAgent = null, Uri referer = null, string language = null, Dictionary<string, string> otherHeaders = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;
            string html = string.Empty;
            try
            {
                var http = CreateHttpClient(cookies, userAgent, referer, language, url.ToLower().StartsWith("https://"), otherHeaders);
                var postResult = http.PostAsJsonAsync(url, obj).Result;
                html = postResult.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                html = string.Format("Service {0} throws an error: {1}", url, ex.Message);
            }

            return html;
        }

        public static HttpClient CreateHttpClient(CookieCollection cookies = null, string userAgent = null, Uri referer = null, string language = null, bool ssl = false, Dictionary<string, string> otherHeaders = null)
        {
            WebRequestHandler wrh = new WebRequestHandler();

            //如果是发送HTTPS请求  
            if (ssl)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                wrh.Credentials = CredentialCache.DefaultCredentials;
                if (wrh.Proxy != null)
                {
                    wrh.Proxy.Credentials = CredentialCache.DefaultCredentials;
                }
            }
            if (cookies != null)
            {
                wrh.UseCookies = true;
                if (wrh.CookieContainer == null)
                    wrh.CookieContainer = new CookieContainer();
                wrh.CookieContainer.Add(cookies);

            }
            HttpClient http = new HttpClient(wrh);
            if (!string.IsNullOrEmpty(language))
            {
                http.DefaultRequestHeaders.AcceptLanguage.Clear();
                http.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
            }
            if (referer != null)
            {
                http.DefaultRequestHeaders.Referrer = referer;
            }
            if (!string.IsNullOrEmpty(userAgent))
            {
                http.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }
            if (otherHeaders != null)
            {
                foreach (var kvp in otherHeaders)
                    http.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
            }
            http.MaxResponseContentBufferSize = 1024 * 1024;

            return http;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}

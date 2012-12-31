using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Serialization;

namespace DAF.Web.Security
{
    public class DefaultAccountProvider : IAccountProvider
    {
        private IAuthEncryptionProvider encrypt;
        private IJsonConvert jsonConvert;

        public DefaultAccountProvider(IAuthEncryptionProvider encrypt, IJsonConvert jsonConvert)
        {
            this.encrypt = encrypt;
            this.jsonConvert = jsonConvert;
        }

        public LoginMessage Login(string account, string password, string sessionId, string device, string deviceId)
        {
            encrypt.Key = WebHelper.CurrentSite.EncryptKey;
            encrypt.IV = WebHelper.CurrentSite.EncryptSecret;

            Dictionary<string, string> authData = new Dictionary<string, string>();
            authData.Add("account", account);
            authData.Add("password", password);

            string toUrl = string.Format("{0}?request=login&auth={1}",
                AuthUrl,
                AuthHelper.EncryptAuthInfo(encrypt, authData));

            Dictionary<string, string> otherHeaders = new Dictionary<string, string>();
            otherHeaders.Add("site", WebHelper.CurrentSite.SiteName);
            Uri referer = HttpContext.Current.Request.UrlReferrer ?? HttpContext.Current.Request.Url;

            var json = HttpHelper.Get(toUrl, WebHelper.GetSessionCookies(), HttpContext.Current.Request.UserAgent, referer, WebHelper.Localizer.GetCurrentCultureInfo(), otherHeaders);
            var result = jsonConvert.Deserialize<LoginMessage>(json);
            if (!string.IsNullOrEmpty(result.EncryptedSession))
            {
                var lsessionStr = encrypt.Decrypt(result.EncryptedSession);
                WebHelper.CurrentSession = jsonConvert.Deserialize<LocalSession>(lsessionStr);
            }
            return result;
        }

        public void Logout()
        {
            if (WebHelper.IsAuthenticated)
            {
                encrypt.Key = WebHelper.CurrentSite.EncryptKey;
                encrypt.IV = WebHelper.CurrentSite.EncryptSecret;
                
                Dictionary<string, string> authData = new Dictionary<string, string>();
                authData.Add("token", WebHelper.CurrentSession.AccessToken);

                WebHelper.CurrentSession = null;
                string toUrl = string.Format("{0}?request=logout&auth={1}",
                    AuthUrl,
                    AuthHelper.EncryptAuthInfo(encrypt, authData));

                Dictionary<string, string> otherHeaders = new Dictionary<string, string>();
                otherHeaders.Add("site", WebHelper.CurrentSite.SiteName);
                Uri referer = HttpContext.Current.Request.UrlReferrer ?? HttpContext.Current.Request.Url;

                HttpHelper.Get(toUrl, WebHelper.GetSessionCookies(), HttpContext.Current.Request.UserAgent, referer, WebHelper.Localizer.GetCurrentCultureInfo(), otherHeaders);
            }
        }

        public string AuthUrl
        {
            get { return WebHelper.HostUrl("Auth"); }
        }
    }
}

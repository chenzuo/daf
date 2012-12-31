using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Threading;
using Autofac;
using DAF.Core;
using DAF.Core.Localization;
using DAF.Core.Generators;
using DAF.Core.Logging;
using DAF.Core.Security;
using DAF.Core.FileSystem;
using DAF.Core.Serialization;
using DAF.Web.Security;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.Web.Menu;

namespace DAF.Web
{
    public class AuthHelper
    {
        public const string RandomChars = "ABCDEFGHJKLMNPQRSTUVWXYZ3456789";
        public const int RandomLength = 8;

        public static void RequireAuthorization()
        {
            if (HttpContext.Current == null)
                return;

            var request = HttpContext.Current.Request;
            var uri = request.Url;
            if (!IsAuthenticated)
            {
                AutoSignOn(
                    () =>
                    {
                        var sessionCookie = HttpContext.Current.Request.Cookies["sid"];
                        return sessionCookie == null ? null : sessionCookie.Value;
                    },
                    () =>
                    {
                        TransferSignOnInfo tso = null;

                        var fromUri = request.UrlReferrer;
                        if (fromUri != null && fromUri.BaseUrl() != uri.BaseUrl())
                        {
                            tso = new TransferSignOnInfo()
                            {
                                ClientId = AuthHelper.CurrentClient.ClientId,
                                DeviceId = request.UserHostAddress,
                                DeviceInfo = request.UserAgent,
                                SessionId = HttpContext.Current.Session.SessionID,
                                FromClientId = request.QueryString["fcid"],
                                FromSessionId = request.QueryString["sid"]
                            };
                        }
                        return tso;
                    });
            }

            if (IsAuthenticated)
            {
                // 用户已经登录，判断权限
                if (AuthHelper.CurrentSession.CanAccess(AuthHelper.CurrentClient.ClientId, uri.LocalPath, PermissionType.Operation))
                {
                    return;
                }
            }

            HttpContext.Current.Response.Redirect("~/Account/SignOn?redirect=" + HttpUtility.UrlEncode(request.RawUrl));
        }

        public static bool AutoSignOn(Func<string> getSessionCookieValue, Func<TransferSignOnInfo> getTransferSignOnInfo)
        {
            ISSOClientProvider cp = IOC.Current.GetService<ISSOClientProvider>();
            var sessionCookieValue = getSessionCookieValue();
            if (!string.IsNullOrEmpty(sessionCookieValue))
            {
                var encrypt = cp.GetEncryptor();
                try
                {
                    var decrypted = encrypt.Decrypt(sessionCookieValue);
                    if (!string.IsNullOrEmpty(decrypted))
                    {
                        IJsonSerializer js = IOC.Current.GetService<IJsonSerializer>();
                        var session = js.Deserialize<Session>(decrypted);
                        cp.SaveSession(session);
                        return true;
                    }
                }
                catch
                {
                }
            }
            else
            {
                var transferSignOnInfo = getTransferSignOnInfo();
                if (transferSignOnInfo != null)
                {
                    var r = cp.TransferSignOn(transferSignOnInfo);
                    return r.Status == ResponseStatus.Success;
                }
            }
            return false;
        }

        public static ServerResponse SignOn(string accountOrEmailOrMobile, string password, bool rememberMe = false, string captcha = null)
        {
            ServerResponse response = new ServerResponse();
            var signOnInfo = new SignOnInfo()
            {
                ClientId = CurrentClient.ClientId,
                SessionId = HttpContext.Current.Session.SessionID,
                DeviceId = HttpContext.Current.Request.UserHostAddress,
                DeviceInfo = HttpContext.Current.Request.UserAgent,
                AccountOrEmailOrMobile = accountOrEmailOrMobile,
                Password = password,
            };

            bool captchaPassed = true;

            try
            {
                if (!string.IsNullOrEmpty(captcha))
                {
                    var captchaGenerator = IOC.Current.GetService<ICaptchaGenerator>();
                    if (captchaGenerator.Verify(signOnInfo.SessionId, captcha) == false)
                    {
                        captchaPassed = false;
                        response.Status = ResponseStatus.Failed;
                        response.Message = DAF.SSO.Resources.Locale(o => o.CaptchaNotCorrect);
                    }
                }

                if (captchaPassed)
                {
                    ISSOClientProvider scp = IOC.Current.GetService<ISSOClientProvider>();

                    var r = scp.SignOn(signOnInfo);
                    if (r.Status == ResponseStatus.Success)
                    {
                        response.Status = ResponseStatus.Success;
                        if (rememberMe)
                        {
                            SetSessionCookie();
                        }
                    }
                    else
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = r.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }

        public static void SignOff()
        {
            RemoveSessionCookie();
            ISSOClientProvider scp = IOC.Current.GetService<ISSOClientProvider>();
            scp.SignOff();
        }

        public static ServerResponse Register(string account, string password, string confirmPassword, string email = null, string mobile = null, Sex? sex = null, DateTime? birthday = null, string fullName = null, string nickName = null)
        {
            ServerResponse response = new ServerResponse();
            RegisterInfo info = new RegisterInfo()
            {
                ClientId = CurrentClient.ClientId,
                SessionId = HttpContext.Current.Session.SessionID,
                DeviceId = HttpContext.Current.Request.UserHostAddress,
                DeviceInfo = HttpContext.Current.Request.UserAgent,
                Account = account,
                Password = password,
                ConfirmPassword = confirmPassword,
                Email = email,
                Mobile = mobile,
                Sex = sex,
                Birthday = birthday,
                FullName = fullName,
                NickName = nickName
            };

            try
            {
                var cp = IOC.Current.GetService<ISSOClientProvider>();
                var r = cp.Register(info);
                response.Status = r.Status;
                response.Message = r.Message;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }

        public static ServerResponse ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            ServerResponse response = new ServerResponse();

            if (IsAuthenticated)
            {
                ChangePasswordInfo info = new ChangePasswordInfo()
                {
                    ClientId = CurrentClient.ClientId,
                    SessionId = HttpContext.Current.Session.SessionID,
                    DeviceId = HttpContext.Current.Request.UserHostAddress,
                    DeviceInfo = HttpContext.Current.Request.UserAgent,
                    UserId = CurrentSession.User.UserId,
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    ConfirmPassword = confirmPassword
                };

                try
                {
                    var cp = IOC.Current.GetService<ISSOClientProvider>();
                    var r = cp.ChangePassword(info);
                    response.Status = r.Status;
                    response.Message = r.Message;
                }
                catch (Exception ex)
                {
                    response.Status = ResponseStatus.Exception;
                    response.Message = ex.Message;
                }
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = DAF.SSO.Resources.Locale(o => o.RequireAuthentication);
            }

            return response;
        }

        public static ServerResponse ResetPassword(string emailOrMobile)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                IRandomTextGenerator generator = IOC.Current.GetService<IRandomTextGenerator>();
                var newPassword = generator.Generate(RandomChars, RandomLength);
                ResetPasswordInfo info = new ResetPasswordInfo()
                {
                    ClientId = CurrentClient.ClientId,
                    SessionId = HttpContext.Current.Session.SessionID,
                    DeviceId = HttpContext.Current.Request.UserHostAddress,
                    DeviceInfo = HttpContext.Current.Request.UserAgent,
                    EmailOrMobile = emailOrMobile,
                    NewPassword = newPassword
                };

                var cp = IOC.Current.GetService<ISSOClientProvider>();
                var r = cp.ResetPassword(info);
                response.Status = r.Status;
                response.Message = r.Message;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }

        public static void SetSessionCookie()
        {
            if (IsAuthenticated && HttpContext.Current.Response != null)
            {
                SetClientCookie();

                IJsonSerializer js = IOC.Current.GetService<IJsonSerializer>();
                ISSOConfiguration sc = IOC.Current.GetService<ISSOConfiguration>();
                ISSOClientProvider cp = IOC.Current.GetService<ISSOClientProvider>();
                var val = js.Serialize(CurrentSession);
                var encrypt = cp.GetEncryptor();
                var encryptedVal = encrypt.Encrypt(val);
                HttpCookie c = new HttpCookie("sid", encryptedVal);
                c.Path = "/";
                //c.Domain = CurrentClient.BaseUrl;
                c.Expires = DateTime.Now.AddMinutes(sc.SessionExpiredTimeOutMunites);
                HttpContext.Current.Response.Cookies.Remove("sid");
                HttpContext.Current.Response.Cookies.Add(c);
            }
        }

        public static void RemoveSessionCookie()
        {
            var sessionCookie = HttpContext.Current.Request.Cookies["sid"];
            if (sessionCookie != null)
            {
                sessionCookie.Expires = DateTime.Now.AddMinutes(-1d);
                HttpContext.Current.Response.Cookies.Add(sessionCookie);
            }
        }

        public static void SetClientCookie()
        {
            if (CurrentClient != null && HttpContext.Current.Response != null)
            {
                HttpCookie c = new HttpCookie("cid", CurrentClient.ClientId);
                c.Path = "/";
                //c.Domain = CurrentClient.BaseUrl;
                c.Expires = DateTime.MaxValue;
                HttpContext.Current.Response.Cookies.Remove("cid");
                HttpContext.Current.Response.Cookies.Add(c);
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                return CurrentSession != null && CurrentSession.IsAuthenticated();
            }
        }

        public static ISession CurrentSession
        {
            get
            {
                ISSOClientProvider clientProvider = IOC.Current.ResolveOptional<ISSOClientProvider>();
                ISession currentSession = clientProvider.GetCurrentSession();

                if (currentSession != null)
                    return currentSession;

                if (HttpContext.Current.Session != null)
                {
                    ISession defaultSession = HttpContext.Current.Session["DefaultUserSession"] as ISession;
                    if (defaultSession != null)
                        return defaultSession;

                    defaultSession = new Session
                    {
                        Theme = DAF.Web.Resources.Locale(o => o.DefaultTheme),
                        Skin = DAF.Web.Resources.Locale(o => o.DefaultSkin),
                        Locale = DAF.Web.Resources.Locale(o => o.DefaultLocale),
                        TimeZone = Convert.ToDouble(DAF.Web.Resources.Locale(o => o.DefaultTimeZone)),
                        SessionId = HttpContext.Current.Session.SessionID
                    };

                    HttpContext.Current.Session["DefaultUserSession"] = defaultSession;
                    return defaultSession;
                }
                return null;
            }
            set
            {
                ISSOClientProvider clientProvider = IOC.Current.ResolveOptional<ISSOClientProvider>();
                clientProvider.SaveSession(value);
            }
        }

        public static SSOClient CurrentClient
        {
            get
            {
                if (HttpContext.Current.Items == null)
                    return null;

                SSOClient currentClient = HttpContext.Current.Items["CurrentClient"] as SSOClient;
                if (currentClient == null)
                {
                    IObjectProvider<SSOClient> clientProvider = IOC.Current.ResolveOptional<IObjectProvider<SSOClient>>();
                    currentClient = clientProvider.GetObject();
                    if (currentClient != null)
                    {
                        HttpContext.Current.Items["CurrentSite"] = currentClient;
                    }
                }
                return currentClient;
            }
        }

        public static SSOServer CurrentServer
        {
            get
            {
                if (HttpContext.Current.Items == null)
                    return null;

                SSOServer currentServer = HttpContext.Current.Items["CurrentServer"] as SSOServer;
                if (currentServer == null)
                {
                    IObjectProvider<SSOServer> serverProvider = IOC.Current.ResolveOptional<IObjectProvider<SSOServer>>();
                    currentServer = serverProvider.GetObject();
                    if (currentServer != null)
                    {
                        HttpContext.Current.Items["CurrentSite"] = currentServer;
                    }
                }
                return currentServer;
            }
        }

        public static SSOClient[] Clients
        {
            get
            {
                if (HttpContext.Current.Items == null)
                    return null;

                SSOClient[] clients = HttpContext.Current.Items["Clients"] as SSOClient[];
                if (clients == null)
                {
                    IObjectProvider<SSOClient[]> clientsProvider = IOC.Current.ResolveOptional<IObjectProvider<SSOClient[]>>();
                    clients = clientsProvider.GetObject();
                    if (clients != null)
                    {
                        HttpContext.Current.Items["Clients"] = clients;
                    }
                }
                return clients ?? new SSOClient[0];
            }
        }
    }
}

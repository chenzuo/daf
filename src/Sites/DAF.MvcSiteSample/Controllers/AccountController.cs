using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Security;
using DAF.Core.Serialization;
using DAF.Web;
using DAF.Web.Security;
using DAF.Web.Mvc;
using DAF.Web.Mvc.Controllers;
using DAF.SSO;
using DAF.SSO.Server;
using DAF.SSO.Client;

namespace DAF.MvcSiteSample.Controllers
{
    public class AccountController : CommonController
    {
        private ISSOClientProvider ssoClientProvider;
        private IObjectProvider<SSOServer> serverProvider;
        private IObjectProvider<SSOClient> clientProvider;
        private ICaptchaGenerator captcha;
        private IJsonSerializer jsonSerializer;

        public AccountController(ISSOClientProvider ssoClientProvider, IObjectProvider<SSOServer> serverProvider, IObjectProvider<SSOClient> clientProvider,
            ICaptchaGenerator captcha, IJsonSerializer jsonSerializer)
        {
            this.ssoClientProvider = ssoClientProvider;
            this.serverProvider = serverProvider;
            this.clientProvider = clientProvider;
            this.captcha = captcha;
            this.jsonSerializer = jsonSerializer;
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult SignOn(string redirect = "/")
        {
            ViewBag.HasTriedLogin = false;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult SignOn(SignOnInfo model, string redirect = "/")
        {
            if (ModelState.IsValid)
            {
                var response = AuthHelper.SignOn(model.AccountOrEmailOrMobile, model.Password, Request.Form["RememberMe"].ConvertTo<bool>(false), Request.Form["Captcha"]);
                if (response.Status == ResponseStatus.Success)
                {
                    return Redirect(redirect);
                }
                else
                {
                    ViewBag.Message = response.Message;
                }
            }
            else
            {
                ModelState.AddModelError("", DAF.SSO.Resources.Locale(o => o.SignOnFailed));
            }
            ViewBag.HasTriedLogin = true;
            return View(model);
        }

        [HttpGet]
        public virtual ActionResult SignOff(string redirect = "/")
        {
            ssoClientProvider.SignOff();
            return Redirect(redirect);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            RegisterInfo model = new RegisterInfo();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult Register(RegisterInfo model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string redirect = "/";
                    var response = ssoClientProvider.Register(model);
                    if (response.Status == ResponseStatus.Success)
                    {
                        return Redirect(redirect);
                    }
                    else
                    {
                        ViewBag.Message = response.Message;
                    }
                }
                catch (ObjectExistsException ex)
                {
                    ModelState.AddModelError(ex.KeyNames, ex);
                }
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        [OAuthorize]
        public ActionResult ChangePassword()
        {
            ChangePasswordInfo model = new ChangePasswordInfo();
            return View(model);
        }

        [HttpPost]
        [OAuthorize]
        public ActionResult ChangePassword(ChangePasswordInfo model)
        {
            if (ModelState.IsValid)
            {
                var response = ssoClientProvider.ChangePassword(model);
                if (response.Status == ResponseStatus.Success)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ViewBag.Message = response.Message;
                }
            }

            return View(model);
        }

        [OAuthorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}
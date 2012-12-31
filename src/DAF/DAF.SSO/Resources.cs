using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core;

namespace DAF.SSO
{
    public class Resources : ResourceBase<Resources>
    {
        public ConstString RequireAuthentication = "RequireAuthentication";
        public ConstString AccountNotFound = "AccountNotFound";
        public ConstString AccountLocked = "AccountLocked";
        public ConstString AccountIsReadOnly = "AccountIsReadOnly";
        public ConstString UserSessionNotFound = "UserSessionNotFound";
        public ConstString UserSessionExpired = "UserSessionExpired";
        public ConstString DuplicatedAccount = "DuplicatedAccount";
        public ConstString SignOnFailed = "SignOnFailed";
        public ConstString RegisterFailed = "RegisterFailed";
        public ConstString PasswordInvalid = "PasswordInvalid";
        public ConstString ConfirmPasswordIsNotSameToPassword = "ConfirmPasswordIsNotSameToPassword";
        public ConstString ChangePasswordSuccessfully = "ChangePasswordSuccessfully";
        public ConstString ResetPasswordSuccessfully = "ResetPasswordSuccessfully";
        public ConstString RememberMe = "RememberMe";
        public ConstString Captcha = "Captcha";

        public ConstString ChangePassword = "ChangePassword";
        public ConstString Register = "Register";
        public ConstString SignOn = "SignOn";
        public ConstString SignOff = "SignOff";
        public ConstString RefreshCaptcha = "RefreshCaptcha";
        public ConstString ResetPassword = "ResetPassword";
        public ConstString ResetPasswordTitle = "ResetPasswordTitle";
        public ConstString CaptchaNotCorrect = "CaptchaNotCorrect";
        public ConstString WelcomeUser = "WelcomeUser";
        public ConstString EmailOrMobileNotFound = "EmailOrMobileNotFound";

        public ConstString CurrentSelectedClient = "CurrentSelectedClient";

    }
}

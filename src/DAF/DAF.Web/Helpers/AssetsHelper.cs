using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;

namespace DAF.Web
{
    public class AssetsHelper
    {
        private static string GetTheme()
        {
            string theme = "Default";
            if (AuthHelper.CurrentSession != null && !string.IsNullOrEmpty(AuthHelper.CurrentSession.Theme))
            {
                theme = AuthHelper.CurrentSession.Theme;
            }
            return theme;
        }

        private static string GetSkin()
        {
            string skin = "Default";
            if (AuthHelper.CurrentSession != null && !string.IsNullOrEmpty(AuthHelper.CurrentSession.Skin))
            {
                skin = AuthHelper.CurrentSession.Skin;
            }
            return skin;
        }

        public static string ThemePartial(string file)
        {
            return string.Format("~/Themes/{0}/{1}", GetTheme(), file);
        }

        public static IHtmlString SkinCss(string cssFile, string media = "")
        {
            string css = string.Format("/Themes/{0}/Skins/{1}/Contents/{2}", GetTheme(), GetSkin(), cssFile);
            return Css(css, AuthHelper.CurrentClient.BaseUrl, media);
        }

        public static IHtmlString SkinScript(string jsFile)
        {
            string js = string.Format("/Themes/{0}/Skins/{1}/Scripts/{2}", GetTheme(), GetSkin(), jsFile);
            return Script(js, AuthHelper.CurrentClient.BaseUrl);
        }

        public static IHtmlString Css(string cssFile, string cdnUrl = "", string media = "")
        {
            if (!cdnUrl.EndsWith("/"))
                cdnUrl += "/";
            if (cssFile.StartsWith("/"))
                cssFile = cssFile.TrimStart('/');
            StringBuilder sb = new StringBuilder();
            sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"");
            sb.Append(cdnUrl.UriCombine(cssFile));
            sb.Append("\"");
            if (!string.IsNullOrEmpty(media))
                sb.AppendFormat(" media=\"{0}\"", media);
            sb.Append(" />");

            return new HtmlString(sb.ToString());
        }

        public static IHtmlString Script(string jsFile, string cdnUrl = "")
        {
            if (!cdnUrl.EndsWith("/"))
                cdnUrl += "/";
            if (jsFile.StartsWith("/"))
                jsFile = jsFile.TrimStart('/');
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\" src=\"");
            sb.Append(cdnUrl.UriCombine(jsFile));
            sb.Append("\" ></script>");

            return new HtmlString(sb.ToString());
        }

        public static IHtmlString CDNCss(string cssFileName)
        {
            return Css("Content/" + cssFileName, CDNUrl);
        }

        public static IHtmlString CDNScript(string jsFileName)
        {
            return Script("Scripts/" + jsFileName, CDNUrl);
        }

        public static IHtmlString File(string file, string cdnUrl = "")
        {
            return new HtmlString(cdnUrl.UriCombine(file));
        }

        public static IHtmlString CDNFile(string file)
        {
            return File(file, CDNUrl);
        }

        public static IHtmlString CDNError(int errorCode, string ext)
        {
            string file = string.Format("/Content/errors/{0}.{1}", errorCode, ext);
            return CDNFile(file);
        }

        public static string CDNUrl { get; set; }
    }
}

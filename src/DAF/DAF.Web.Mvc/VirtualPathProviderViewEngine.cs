using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace DAF.Web.Mvc
{
    public abstract class VirtualPathProviderViewEngine : IViewEngine
    {
        // format is ":ViewCacheEntry:{cacheType}:{prefix}:{name}:{controllerName}:{areaName}:"
        protected const string _cacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";
        protected const string _cacheKeyPrefix_Master = "Master";
        protected const string _cacheKeyPrefix_Partial = "Partial";
        protected const string _cacheKeyPrefix_View = "View";
        protected VirtualPathProvider _vpp;
        protected static readonly string[] _emptyLocations;

        static VirtualPathProviderViewEngine()
        {
            _emptyLocations = new string[0];
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] AreaMasterLocationFormats
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] AreaPartialViewLocationFormats
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] AreaViewLocationFormats
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] MasterLocationFormats
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] PartialViewLocationFormats
        {
            get;
            set;
        }

        public IViewLocationCache ViewLocationCache
        {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] ViewLocationFormats
        {
            get;
            set;
        }

        protected VirtualPathProvider VirtualPathProvider
        {
            get
            {
                if (_vpp == null)
                {
                    _vpp = HostingEnvironment.VirtualPathProvider;
                }
                return _vpp;
            }
            set
            {
                _vpp = value;
            }
        }

        protected VirtualPathProviderViewEngine()
        {
            if (HttpContext.Current == null || HttpContext.Current.IsDebuggingEnabled)
            {
                ViewLocationCache = DefaultViewLocationCache.Null;
            }
            else
            {
                ViewLocationCache = new DefaultViewLocationCache();
            }
        }

        protected string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return String.Format(CultureInfo.InvariantCulture, _cacheKeyFormat,
                GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName);
        }

        protected abstract IView CreatePartialView(ControllerContext controllerContext, string partialPath);

        protected abstract IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath);

        protected virtual bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            // this is the original file exists method, which cost expensive.
            // return VirtualPathProvider.FileExists(virtualPath);
            var physicalPath = controllerContext.HttpContext.Server.MapPath(virtualPath);
            return File.Exists(physicalPath);
        }

        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrWhiteSpace(partialViewName))
            {
                throw new ArgumentException("partialViewName");
            }

            string[] searched;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, controllerName, _cacheKeyPrefix_Partial, useCache, out searched);

            if (String.IsNullOrWhiteSpace(partialPath))
            {
                return new ViewEngineResult(searched);
            }

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }

        public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrWhiteSpace(viewName))
            {
                throw new ArgumentException("viewName");
            }

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats, "ViewLocationFormats", viewName, controllerName, _cacheKeyPrefix_View, useCache, out viewLocationsSearched);
            string masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats, "MasterLocationFormats", masterName, controllerName, _cacheKeyPrefix_Master, useCache, out masterLocationsSearched);

            if (String.IsNullOrWhiteSpace(viewPath) || (String.IsNullOrWhiteSpace(masterPath) && !String.IsNullOrWhiteSpace(masterName)))
            {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        protected string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrWhiteSpace(name))
            {
                return String.Empty;
            }

            object areaObj = null;
            string areaName = string.Empty;
            controllerContext.RouteData.DataTokens.TryGetValue("area", out areaObj);
            if (areaObj != null)
                areaName = areaObj.ToString();
            bool usingAreas = !String.IsNullOrWhiteSpace(areaName);
            List<ViewLocation> viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);

            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                    "Property {0} Cannot be NullOrEmpty", locationsPropertyName));
            }

            bool nameRepresentsPath = IsSpecificPath(name);
            string cacheKey = CreateCacheKey(cacheKeyPrefix, name, (nameRepresentsPath) ? String.Empty : controllerName, areaName);

            if (useCache)
            {
                return ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
            }

            return (nameRepresentsPath) ?
                GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations) :
                GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, cacheKey, ref searchedLocations);
        }

        protected string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            string result = String.Empty;
            searchedLocations = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                ViewLocation location = locations[i];
                string virtualPath = GetFormatedVirtualPath(controllerContext.HttpContext, location, name, controllerName, areaName);

                if (FileExists(controllerContext, virtualPath))
                {
                    searchedLocations = _emptyLocations;
                    result = virtualPath;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        protected string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string result = name;

            if (!FileExists(controllerContext, name))
            {
                result = String.Empty;
                searchedLocations = new[] { name };
            }

            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
            return result;
        }

        protected virtual string GetFormatedVirtualPath(HttpContextBase context, ViewLocation location, string name, string controllerName, string areaName)
        {
            return location.Format(name, controllerName, areaName);
        }

        protected static List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            List<ViewLocation> allLocations = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
            {
                foreach (string areaViewLocationFormat in areaViewLocationFormats)
                {
                    allLocations.Add(new AreaAwareViewLocation(areaViewLocationFormat));
                }
            }

            if (viewLocationFormats != null)
            {
                foreach (string viewLocationFormat in viewLocationFormats)
                {
                    allLocations.Add(new ViewLocation(viewLocationFormat));
                }
            }

            return allLocations;
        }

        protected static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        protected virtual bool? IsValidPath(ControllerContext controllerContext, string virtualPath)
        {
            return null;
        }

        public virtual void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IDisposable disposable = view as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public class ViewLocation
        {

            protected string _virtualPathFormatString;

            public ViewLocation(string virtualPathFormatString)
            {
                _virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName);
            }

            public string VirtualPathFormatString { get { return _virtualPathFormatString; } }
        }

        public class AreaAwareViewLocation : ViewLocation
        {

            public AreaAwareViewLocation(string virtualPathFormatString)
                : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string controllerName, string areaName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName);
            }

        }
    }
}
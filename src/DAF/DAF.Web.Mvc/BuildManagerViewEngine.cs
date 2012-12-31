using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.Mvc;
using System.IO;

namespace DAF.Web.Mvc
{
    public abstract class BuildManagerViewEngine : VirtualPathProviderViewEngine
    {
        protected sealed override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            var physicalPath = controllerContext.HttpContext.Server.MapPath(virtualPath);
            return File.Exists(physicalPath);

            // this is the original file exists method, which cost expensive.
            // return (BuildManager.GetObjectFactory(virtualPath, false) != null);
        }
        protected abstract bool IsValidCompiledType(ControllerContext controllerContext, string virtualPath, Type compiledType);
        protected sealed override bool? IsValidPath(ControllerContext controllerContext, string virtualPath)
        {
            Type compiledType = BuildManager.GetCompiledType(virtualPath);
            if (compiledType != null)
            {
                return new bool?(this.IsValidCompiledType(controllerContext, virtualPath, compiledType));
            }
            return null;
        }
    }
}

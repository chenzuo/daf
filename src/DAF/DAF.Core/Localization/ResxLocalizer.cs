using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Web;

namespace DAF.Core.Localization
{
    public class ResxLocalizer : ILocalizer
    {
        private ResourceManager rm;
        private Type type;
        private List<AssemblyNameResourceManager> resourceManagers = new List<AssemblyNameResourceManager>();

        public ResxLocalizer(Type type)
        {
            this.type = type;
            ResourceManager temp = new ResourceManager(type.Assembly.GetName().Name + ".Resources", type.Assembly);
            rm = temp;
        }

        private ResourceManager GetResourceManager(string nameSpace)
        {
            nameSpace = NormalizeAssemblyName(nameSpace);
            var nrm = resourceManagers.FirstOrDefault(o => string.Equals(o.NameSpace, nameSpace, StringComparison.InvariantCultureIgnoreCase));
            if (nrm != null)
                return nrm.ResourceManager;

            //ResourceManager rm = ResourceManager.CreateFileBasedResourceManager(nameSpace, AppDomain.CurrentDomain.BaseDirectory, null);
            //if (rm != null)
            //{
            //    AddResourceManager(nameSpace, rm);
            //}

            //return rm;

            return null;
        }

        public void AddResourceManager(string nameSpace, ResourceManager rm)
        {
            nameSpace = NormalizeAssemblyName(nameSpace);
            if (resourceManagers.Any(o => string.Equals(o.NameSpace, nameSpace, StringComparison.InvariantCultureIgnoreCase)))
                resourceManagers.RemoveAll(o => string.Equals(o.NameSpace, nameSpace, StringComparison.InvariantCultureIgnoreCase));
            rm.IgnoreCase = true;
            resourceManagers.Add(new AssemblyNameResourceManager() { NameSpace = nameSpace, ResourceManager = rm });
        }

        private string NormalizeAssemblyName(string nameSpace)
        {
            return nameSpace.Replace(".", "_");
        }

        public string GetCurrentCultureInfo()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }

        public void SetCurrentCultureInfo(string culture)
        {
            try
            {
                CultureInfo cultureInfo = new CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch
            {
            }
        }

        public string Get(string resourceName, string nameSpace = "Common", string cultureInfo = null)
        {
            if (nameSpace == "mscorlib")
                return null;
            if (string.IsNullOrWhiteSpace(cultureInfo))
                cultureInfo = GetCurrentCultureInfo();
            nameSpace = NormalizeAssemblyName(nameSpace);
            var nrm = GetResourceManager(nameSpace);
            if (nrm != null)
                return nrm.GetString(resourceName, new CultureInfo(cultureInfo));
            return resourceName;
            //object resVal = null;
            //try
            //{
            //    resVal = HttpContext.GetGlobalResourceObject(nameSpace, resourceName, new CultureInfo(cultureInfo));

            //}
            //catch { }
            //return resVal == null ? resourceName : resVal.ToString();
        }

        public void Set(string resourceName, string value, string nameSpace = "Common", string cultureInfo = null)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
        }

        internal class AssemblyNameResourceManager
        {
            public string NameSpace { get; set; }
            public ResourceManager ResourceManager { get; set; }
        }
    }
}

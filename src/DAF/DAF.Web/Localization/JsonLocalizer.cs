using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using DAF.Core;
using DAF.Core.Collections;
using DAF.Core.Serialization;
using DAF.Core.Localization;
using DAF.Core.FileSystem;

namespace DAF.Web.Localization
{
    public class JsonLocalizer : ILocalizer
    {
        private List<JsonResource> jsonResources;
        private IJsonSerializer jsonSerializer;
        private IFileSystemProvider fileProvider;

        public JsonLocalizer(IFileSystemProvider fileProvider, IJsonSerializer jsonSerializer)
        {
            jsonResources = new List<JsonResource>();
            this.fileProvider = fileProvider;
            this.fileProvider.SetRootPath("~/".GetPhysicalPath());
            this.jsonSerializer = jsonSerializer;
            LoadResources();
        }

        private void LoadResources()
        {
            var dirs = fileProvider.GetPaths("Localization", false)
                .Union(fileProvider.GetPaths("/Areas/*/Localization", false));
            dirs.GetFiles("*.js", false)
            .ForEach(o =>
            {
                JsonResource jr = new JsonResource();
                jr.FileName = o.FullName;

                string[] fiNames = o.FileNameWithoutExtension().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (fiNames.Length == 2)
                {
                    jr.NameSpace = fiNames[0];
                    jr.CultureInfoName = fiNames[1];
                }
                else if (fiNames.Length == 1)
                {
                    jr.NameSpace = fiNames[0];
                    jr.CultureInfoName = string.Empty;
                }

                string json = File.ReadAllText(o.FullName);

                jr.Resources = jsonSerializer.Deserialize<SerializableDictionary>(json);
                if (jr.Resources == null)
                    jr.Resources = new SerializableDictionary();

                jsonResources.Add(jr);
            });
        }

        private string NormalizeName(string name)
        {
            return name.Replace(".", "_");
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

        public string Get(string resourceName, string nameSpace = "DAF.Core", string cultureInfo = null)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                return string.Empty;
            if (string.IsNullOrEmpty(cultureInfo))
                cultureInfo = GetCurrentCultureInfo();
            nameSpace = NormalizeName(nameSpace);
            resourceName = NormalizeName(resourceName);
            JsonResource jr = jsonResources.FirstOrDefault(o => o.NameSpace == nameSpace && o.CultureInfoName == cultureInfo);
            if (jr == null)
                jr = jsonResources.FirstOrDefault(o => o.NameSpace == nameSpace && o.CultureInfoName == string.Empty);

            if (jr != null && jr.Resources != null)
            {
                if (jr.Resources.ContainsKey(resourceName))
                {
                    return jr.Resources[resourceName] as string;
                }
            }
            return resourceName;
        }

        public void Set(string resourceName, string value, string nameSpace = "DAF.Core", string cultureInfo = null)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                return;
            if (string.IsNullOrEmpty(cultureInfo))
                cultureInfo = GetCurrentCultureInfo();
            nameSpace = NormalizeName(nameSpace);
            resourceName = NormalizeName(resourceName);
            var res = jsonResources.FirstOrDefault(o => o.NameSpace == nameSpace && o.CultureInfoName == cultureInfo);
            if (res == null)
            {
                var defaultRes = jsonResources.FirstOrDefault(o => o.NameSpace == nameSpace && o.CultureInfoName == string.Empty);
                if (defaultRes != null)
                {
                    res = new JsonResource() { NameSpace = nameSpace, CultureInfoName = cultureInfo };
                    res.FileName = defaultRes.FileName.Replace(".js", string.Format(".{0}.js", cultureInfo));
                    res.Resources = new SerializableDictionary();
                    jsonResources.Add(res);
                }
            }
            if (res.Resources.ContainsKey(resourceName))
            {
                if (string.IsNullOrWhiteSpace(value))
                    res.Resources.Remove(resourceName);
                else
                {
                    res.Resources[resourceName] = value;
                }
            }
            else
            {
                res.Resources.Add(resourceName, value);
            }
        }

        public void Flush()
        {
            foreach (var res in jsonResources)
            {
                if (string.IsNullOrEmpty(res.FileName))
                {
                    continue;
                }

                var json = jsonSerializer.Serialize(res.Resources);
                File.WriteAllText(res.FileName, json);
            }
        }

        public List<JsonResource> Resources { get { return jsonResources; } }
    }

    public class JsonResource
    {
        public string CultureInfoName { get; set; }
        public string FileName { get; set; }
        public string NameSpace { get; set; }
        public SerializableDictionary Resources { get; set; }
    }
}

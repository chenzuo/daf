using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Serialization;

namespace DAF.Core.DataMonitor
{
    public class JsonChangeVersionManager : IChangeVersionManager
    {
        private List<VersionInfo> versions;
        private IObjectProvider<IEnumerable<VersionInfo>> jsonSerializer;

        public JsonChangeVersionManager(IObjectProvider<IEnumerable<VersionInfo>> jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
            versions = new List<VersionInfo>();
            var vs = jsonSerializer.GetObject();
            if (vs != null)
            {
                versions.AddRange(vs);
            }
        }

        public VersionInfo GetLastVersion(string tableName)
        {
            return versions.FirstOrDefault(o => o.TableName == tableName);
        }

        public void SaveLastVersion(string tableName, VersionInfo lastVersion)
        {
            var v = GetLastVersion(tableName);
            if(v == null)
            {
                v = lastVersion;
                versions.Add(v);
            }
            else
            {
                v.LastUpdateTime = lastVersion.LastUpdateTime;
                v.LastUpdateVersion = lastVersion.LastUpdateVersion;
            }
            jsonSerializer.SaveObject(versions);
        }
    }
}

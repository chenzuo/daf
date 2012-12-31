using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DAF.Web.Security
{
    public class ProtectedDirAssetChecker : IProtectedAssetChecker
    {
        private string[] dirs = null;

        public ProtectedDirAssetChecker(string dirs)
        {
            if (!string.IsNullOrEmpty(dirs))
                this.dirs = dirs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public bool IsProtectedAsset(Uri uri)
        {
            if (dirs == null || dirs.Length <= 0)
                return true;
            return dirs.Any(o => uri.LocalPath.StartsWith(o, StringComparison.OrdinalIgnoreCase));
        }
    }
}

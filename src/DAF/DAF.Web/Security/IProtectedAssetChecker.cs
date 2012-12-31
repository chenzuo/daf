using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Web.Security
{
    public interface IProtectedAssetChecker
    {
        bool IsProtectedAsset(Uri uri);
    }
}

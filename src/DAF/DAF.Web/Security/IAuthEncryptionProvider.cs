using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Security;

namespace DAF.Web.Security
{
    public interface IAuthEncryptionProvider : IEncryptionProvider
    {
        string Key { get; set; }
        string IV { get; set; }
    }
}

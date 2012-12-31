using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.SSO
{
    public interface ISSOConfiguration
    {
        string TokenAllowedChars { get; }
        int TokenLength { get; }
        int SessionExpiredTimeOutMunites { get; }
        int TransferExpiredTimeOutMunites { get; }
    }
}

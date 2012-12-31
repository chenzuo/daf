using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.SSO
{
    public class DefaultSSOConfiguration : ISSOConfiguration
    {
        public string TokenAllowedChars
        {
            get { return "ABCDEFGHJKLMNPQRSTUVWXYZ3456789"; }
        }

        public int TokenLength
        {
            get { return 8; }
        }

        public int SessionExpiredTimeOutMunites
        {
            get { return 120; }
        }

        public int TransferExpiredTimeOutMunites
        {
            get { return 10; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace DAF.SSO.Client
{
    public interface IDefaultSessionProvider
    {
        ISession NewSession();
    }
}

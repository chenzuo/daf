using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Web.Security
{
    public enum LoginStatus
    {
        Success = 0,
        AccountNotExists = 1,
        PasswordNotCorrect = 2,
        Exception = 9
    }
}

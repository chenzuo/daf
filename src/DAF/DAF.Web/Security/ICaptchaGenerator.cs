using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DAF.Web.Security
{
    public interface ICaptchaGenerator
    {
        bool Verify(string sessionId, string userInput);
        Bitmap Generate(string sessionId, string randomText, int width, int height);
    }
}

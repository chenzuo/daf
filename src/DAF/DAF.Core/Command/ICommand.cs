using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Command
{
    public interface ICommand
    {
        string Name { get; }
        Dictionary<string, string> Args { get; set; }
        object Run(object context);
    }
}

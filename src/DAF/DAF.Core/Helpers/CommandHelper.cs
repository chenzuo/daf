using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core.Command;

namespace DAF.Core
{
    public class CommandHelper
    {
        /// <summary>
        /// 执行命令， 如：Run("session name:abc,limit:5", HttpContext.Currrent);
        /// </summary>
        /// <param name="cmd">命令名称</param>
        /// <param name="context">命令执行上下文</param>
        /// <returns></returns>
        public static object Run(string cmd, object context)
        {
            cmd = cmd.Trim();
            var cmds = IOC.Current.GetService<IEnumerable<ICommand>>();
            if (cmds != null)
            {
                int idx = cmd.IndexOf(' ');
                var cmdName = cmd.Substring(0, idx);
                var args = cmd.Substring(idx + 1).Trim();
                var c = cmds.FirstOrDefault(o => o.Name == cmdName);
                if (c != null)
                {
                    var dic = args.ToDictionary(",", ":");
                    c.Args = dic;
                    return c.Run(context);
                }
            }
            return null;
        }
    }
}

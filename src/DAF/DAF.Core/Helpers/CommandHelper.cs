using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Autofac;
using DAF.Core.Command;

namespace DAF.Core
{
    public class CommandHelper
    {
        public const string CommandRegexPattern = @"{\s*cmd=(?<cmd>\w+)\s*(?<args>(,*[\w|:]+)*)\s*}";
        public static Regex CommandRegex = new Regex(CommandRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="input">含有命令的字符串，如others{cmd=url with:sso_client,name=DAF.File.Site}more_texts</param>
        /// <param name="context">解析时用到的context，web中，一般是HttpContext.Current</param>
        /// <returns>解析后的字符串</returns>
        public static string AnalysisCommands(string input, object context)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            input = input.Trim();
            if (CommandRegex.IsMatch(input))
            {
                var cmds = IOC.Current.GetService<IEnumerable<ICommand>>();
                string output = CommandRegex.Replace(input, m =>
                    {
                        var cmd = m.Groups["cmd"].Value;
                        var args = m.Groups["args"].Value;
                        var obj = Run(cmd, args, context);
                        if (obj != null)
                            return obj.ToString();
                        return m.Value;
                    });
                return output == input ? null : output;
            }
            return input;
        }

        /// <summary>
        /// 执行命令， 如：Run("session", "name:abc,limit:5", HttpContext.Currrent);
        /// </summary>
        /// <param name="cmd">命令名称</param>
        /// <param name="args">命令参数</param>
        /// <param name="context">命令执行上下文</param>
        /// <returns></returns>
        public static object Run(string cmd, string args, object context)
        {
            var cmds = IOC.Current.GetService<IEnumerable<ICommand>>();
            if (cmds != null)
            {
                var c = cmds.FirstOrDefault(o => o.Name == cmd);
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

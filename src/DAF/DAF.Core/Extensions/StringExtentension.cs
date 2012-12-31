using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace DAF.Core
{
    public static class StringExtentension
    {
        public static string Summary(this string source, int length)
        {
            if (source.Length <= length)
                return source;
            return source.Substring(0, length);
        }

        public static int GetLength(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return 0;
            return Encoding.Default.GetBytes(source).Length;
        }

        /// <summary>
        /// 将字符串转换成Camel命名法字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>Camel命名法字符串</returns>
        public static string ToCamelCase(this string source)
        {
            return source.Substring(0, 1).ToLower() + source.Substring(1).Replace(" ", "");
        }

        /// <summary>
        /// 将字符串转换成Pascal命名法字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>Pascal命名法字符串</returns>
        public static string ToPascalCase(this string source)
        {
            return source.Substring(0, 1).ToUpper() + source.Substring(1).Replace(" ", "");
        }

        public static byte[] ToBase64Bytes(this string source)
        {
            return Convert.FromBase64String(source);
        }

        public static string ToBase64String(this byte[] source)
        {
            return Convert.ToBase64String(source);
        }

        public static byte[] ToUTF8Bytes(this string source)
        {
            return Encoding.UTF8.GetBytes(source);
        }

        public static string ToUTF8String(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }

        public static byte[] Copy(this byte[] source, int length, bool appendIfNotEnough = false)
        {
            if (source != null)
            {
                if (source.Length > length)
                {
                    byte[] dest = new byte[length];
                    Array.Copy(source, dest, length);
                    return dest;
                }
                else if (appendIfNotEnough)
                {
                    byte[] dest = new byte[length];
                    int lastIdx = 0;
                    int lastLength = source.Length;
                    while (length > 0)
                    {
                        Array.Copy(source, 0, dest, lastIdx, lastLength);
                        lastIdx += lastLength;
                        length -= lastLength;
                        lastLength = length > source.Length ? source.Length : length;
                    }
                    return dest;
                }
            }
            return source;
        }

        /// <summary>
        /// 将字符串转换成字符串集合
        /// </summary>
        /// <param name="formatValue">原字符串</param>
        /// <param name="comma">分隔符</param>
        /// <returns>字符串集合</returns>
        public static List<string> ToList(this string formatValue, char comma)
        {
            List<string> list = new List<string>();
            list.AddRange(formatValue.Split(comma));
            return list;
        }

        public static List<string> ToList(this string formatValue)
        {
            return ToList(formatValue, ',');
        }

        public static List<string> ToList(this string formatValue, string leftComma, string rightComma)
        {
            List<string> list = new List<string>();
            int sidx = formatValue.IndexOf(leftComma);
            int nidx = 0;
            while (sidx > 0 && sidx < formatValue.Length)
            {
                nidx = formatValue.IndexOf(rightComma, sidx);
                if (nidx > sidx)
                {
                    string str = formatValue.Substring(sidx, nidx - sidx + 1);
                    list.Add(str);
                    sidx = formatValue.IndexOf(leftComma, nidx);
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        /// <summary>
        /// 将字符串转为字符串字典
        /// </summary>
        /// <param name="formatValue">字符串</param>
        /// <param name="comma">各字符串之间的分隔符</param>
        /// <param name="equal">字符串名称与值之间的分隔符</param>
        /// <param name="leftComma">值前特殊串</param>
        /// <param name="rightComma">值后特殊串</param>
        /// <returns>字符串字典</returns>
        public static Dictionary<string, string> ToDictionary(this string formatValue, string comma = "&", string equal = "=", string leftComma = "", string rightComma = "")
        {
            return ToDictionary(formatValue, o => o, o => o, comma, equal, leftComma, rightComma);
        }

        public static Dictionary<T, V> ToDictionary<T, V>(this string formatValue, Func<string, T> keyResolver, Func<string, V> valResolver, string comma = "&", string equal = "=", string leftComma = "", string rightComma = "")
        {
            Dictionary<T, V> dic = new Dictionary<T, V>();
            if (string.IsNullOrWhiteSpace(formatValue))
                return dic;
            int startIdx = 0;
            int equalIdx = 0;
            int leftIdx = 0, rightIdx = 0;
            string key = "", value = "";
            while (startIdx < formatValue.Length)
            {
                equalIdx = formatValue.IndexOf(equal, startIdx);
                key = formatValue.Substring(startIdx, equalIdx - startIdx);
                if (string.IsNullOrWhiteSpace(leftComma))
                    leftIdx = equalIdx + 1;
                else
                    leftIdx = formatValue.IndexOf(leftComma, equalIdx) + leftComma.Length;
                if (string.IsNullOrWhiteSpace(rightComma))
                    rightIdx = formatValue.IndexOf(comma, leftIdx);
                else
                    rightIdx = formatValue.IndexOf(rightComma, leftIdx);
                if (rightIdx < 0)
                    rightIdx = formatValue.Length;
                value = formatValue.Substring(leftIdx, rightIdx - leftIdx);
                T dicKey = keyResolver(key);
                V dicVal = valResolver(value);
                if (dic.ContainsKey(dicKey))
                {
                    dic[dicKey] = dicVal;
                }
                else
                {
                    dic.Add(dicKey, dicVal);
                }
                rightIdx = formatValue.IndexOf(comma, rightIdx);
                if (rightIdx < 0)
                    rightIdx = formatValue.Length;
                startIdx = rightIdx + comma.Length;
            }
            return dic;
        }
        public static NameValueCollection ToNameValueCollection(this string formatValue, string comma = "&", string equal = "=", string leftComma = "", string rightComma = "", Func<string, string> keyResolver = null, Func<string, string> valResolver = null)
        {
            NameValueCollection nv = new NameValueCollection();
            if (string.IsNullOrWhiteSpace(formatValue))
                return nv;
            int startIdx = 0;
            int equalIdx = 0;
            int leftIdx = 0, rightIdx = 0;
            string key = "", value = "";
            while (startIdx < formatValue.Length)
            {
                equalIdx = formatValue.IndexOf(equal, startIdx);
                key = formatValue.Substring(startIdx, equalIdx - startIdx);
                if (string.IsNullOrWhiteSpace(leftComma))
                    leftIdx = equalIdx + 1;
                else
                    leftIdx = formatValue.IndexOf(leftComma, equalIdx) + leftComma.Length;
                if (string.IsNullOrWhiteSpace(rightComma))
                    rightIdx = formatValue.IndexOf(comma, leftIdx);
                else
                    rightIdx = formatValue.IndexOf(rightComma, leftIdx);
                if (rightIdx < 0)
                    rightIdx = formatValue.Length;
                value = formatValue.Substring(leftIdx, rightIdx - leftIdx);
                string nvKey = keyResolver == null ? key : keyResolver(key);
                string nvVal = valResolver == null ? value : valResolver(value);
                if (nv.AllKeys.Contains(nvKey))
                {
                    nv[nvKey] = nvVal;
                }
                else
                {
                    nv.Add(nvKey, nvVal);
                }
                rightIdx = formatValue.IndexOf(comma, rightIdx);
                if (rightIdx < 0)
                    rightIdx = formatValue.Length;
                startIdx = rightIdx + comma.Length;
            }
            return nv;
        }

        public static bool IsMatch(this string text, string pattern, StringMatchType matchType, bool ignoreCase)
        {
            StringComparison comparison = StringComparison.Ordinal;
            if(ignoreCase)
                comparison = StringComparison.OrdinalIgnoreCase;
            switch(matchType)
            {
                default:
                case StringMatchType.Equals:
                    return string.Equals(text, pattern, comparison);
                case StringMatchType.StartWith:
                    return text.StartsWith(pattern, comparison);
                case StringMatchType.EndWith:
                    return text.EndsWith(pattern, comparison);
                case StringMatchType.Contains:
                    return text.IndexOf(pattern, comparison) >= 0;
                case StringMatchType.RegexMatch:
                    RegexOptions options = RegexOptions.None;
                    if(ignoreCase)
                        options = RegexOptions.IgnoreCase;
                    return Regex.IsMatch(text, pattern, options);
                case StringMatchType.WildCharMatch:
                    StringComparison sc = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                    if (!pattern.StartsWith("*"))
                    {
                        int stop = pattern.IndexOf('*');
                        if (!text.StartsWith(pattern.Substring(0, stop), sc))
                            return false;
                    }

                    if (!pattern.EndsWith("*"))
                    {
                        int start = pattern.LastIndexOf('*') + 1;
                        if (!text.EndsWith(pattern.Substring(start, pattern.Length - start), sc))
                            return false;
                    }

                    string regex = pattern.Replace(@".", @"\.").Replace(@"*", @".*");
                    if (ignoreCase)
                        return Regex.IsMatch(text, regex, RegexOptions.IgnoreCase);
                    else
                        return Regex.IsMatch(text, regex);
            }
        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>压缩后的字符串</returns>
        public static string Compress(this string source)
        {
            byte[] buffer = UTF8Encoding.Unicode.GetBytes(source);
            MemoryStream ms = new MemoryStream();
            using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
            {
                gzip.Write(buffer, 0, buffer.Length);
            }
            return Convert.ToBase64String(ms.GetBuffer()).TrimEnd('\0');
        }

        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="str">压缩后的字符串</param>
        /// <returns>原字符串</returns>
        public static string Decompress(this string source)
        {
            byte[] buffer = Convert.FromBase64String(source);
            MemoryStream ms = new MemoryStream();
            MemoryStream msOut = new MemoryStream();
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            byte[] writeData = new byte[4096];
            using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
            {
                int n;
                while ((n = gzip.Read(writeData, 0, writeData.Length)) != 0)
                {
                    msOut.Write(writeData, 0, n);
                }
            }
            return UTF8Encoding.Unicode.GetString(msOut.GetBuffer()).TrimEnd('\0');
        }

        public static string FormatWhenHasValue(this string source, string format)
        {
            if (string.IsNullOrWhiteSpace(source))
                return source;
            return string.Format(format, source);
        }

        public static string Replace(this string text, Dictionary<string, string> words)
        {
            return Replace(text, words, true);
        }

        public static string Replace(this string text, Dictionary<string, string> words, bool ignoreCase)
        {
            if (string.IsNullOrWhiteSpace(text) || words == null || words.Count <= 0)
                return text;
            string pattern = String.Join("|", words.Keys.ToArray());
            RegexOptions options = RegexOptions.Compiled;
            if (ignoreCase)
                options = options | RegexOptions.IgnoreCase;
            Regex regex = new Regex(pattern, options);
            return regex.Replace(text, m => words[m.Value]);
        }

        public static string Repeat(this string text, int count)
        {
            while (count-- > 1)
                text += text;
            return text;
        }

        public static string PathCombine(this string root, params string[] paths)
        {
            string separator = Path.DirectorySeparatorChar.ToString();
            if (!root.EndsWith(separator))
                root += separator;

            var ps = paths.Cast<string, string>(o => o.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar));

            return root + string.Join(separator, ps.ToArray());
        }

        // web extensions
        public static string RemoveTags(this string html)
        {
            return String.IsNullOrEmpty(html)
                ? ""
                : Regex.Replace(html, "<[^<>]*>", "", RegexOptions.Singleline);
        }

        public static string UriCombine(this string root, params string[] paths)
        {
            if (!root.EndsWith("/"))
                root += "/";

            var ps = paths.Select(o => o.TrimStart('/').TrimEnd('/'));

            return root + string.Join("/", ps.ToArray());
        }

        public static string AppendQueryString(this string url, string qs)
        {
            string split = url.IndexOf('?') > 0 ? "&" : "?";
            return string.Concat(url, split, qs);
        }

        public static string AppendQueryString(this string url, string name, string val)
        {
            string split = url.IndexOf('?') > 0 ? "&" : "?";
            return string.Format("{0}{1}{2}={3}", url, split, name, val);
        }

        public static string GetPhysicalPath(this string path)
        {
            if (path.IndexOf(":\\") > 0)
                return path;
            if (!path.StartsWith("~"))
            {
                if (!path.StartsWith("/"))
                    path = "~/" + path;
                else
                    path = "~" + path;
            }
            if(HttpContext.Current != null)
                return HttpContext.Current.Server.MapPath(path);
            return HostingEnvironment.MapPath(path);
        }

    }
}

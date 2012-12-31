using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace DAF.File.Site.Models
{
    public class FileType
    {
        public FileType()
        {
        }

        public FileType(string code, string name, string regex, int limitSize, bool showPreview)
        {
            TypeCode = code;
            TypeName = name;
            RegexPattern = regex;
            LimitFileSize = limitSize;
            ShowPreview = showPreview;
        }

        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string RegexPattern { get; set; }
        public int LimitFileSize { get; set; }
        public bool ShowPreview { get; set; }

        public static FileType[] CommonTypes
        {
            get
            {
                return new FileType[]
                    {
                        new FileType("images", "图片", @"(\.|\/)(gif|jpe?g|png)$", 5 * 1024 *1024, true),
                        new FileType("audio", "音频", @"(\.|\/)(mp3)$", 10 * 1024 *1024, false),
                        new FileType("video", "视频", @"(\.|\/)(mpe?g|mp4)$", 100 * 1024 *1024, true),
                        new FileType("flash", "Flash", @"(\.|\/)(flv|swf)$", 10 * 1024 *1024, false),
                        new FileType("files", "其他文件", @".+$", 100 * 1024 *1024, false)
                    };
            }
        }

        public static string GetFriendlyFileSize(int fileSize)
        {
            if (fileSize > 1073741824)
                return string.Format("{0}MB", fileSize / 1048576);
            if (fileSize > 1048576)
                return string.Format("{0}MB", fileSize / 1048576);
            if (fileSize > 1024)
                return string.Format("{0}KB", fileSize / 1024);
            return string.Format("{0}B", fileSize);
        }
    }

    public static class FileTypeExtensions
    {
        public static FileType GetFileTypeByExtension(this IEnumerable<FileType> fileTypes, string extension)
        {
            foreach (var ft in fileTypes)
            {
                if (Regex.IsMatch(extension, ft.RegexPattern))
                    return ft;
            }
            return null;
        }
    }
}
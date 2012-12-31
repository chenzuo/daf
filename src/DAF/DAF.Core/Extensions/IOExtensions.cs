using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAF.Core
{
    public static class IOExtensions
    {
        public static string FileNameWithoutExtension(this FileInfo file)
        {
            int length = file.Name.LastIndexOf('.');
            if (length == -1)
            {
                return file.FullName;
            }
            return file.Name.Substring(0, length);
        }

        public static string MakeRelativeTo(this string path, string parent)
        {
            if (!path.StartsWith(parent))
                throw new Exception(string.Format("path {0} not relative to {1}!", path, parent));

            return path.Substring(parent.Length);
        }

        public static DirectoryInfo GetDirectory(this string rootPath, string path)
        {
            return new DirectoryInfo(Path.Combine(rootPath, path));
        }

        public static IEnumerable<DirectoryInfo> GetDirectories(this string rootPath, params string[] path)
        {
            if (path == null || path.Length <= 0)
                yield return null;

            foreach (var p in path)
                yield return new DirectoryInfo(Path.Combine(rootPath, p));
        }

        public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo dir, string searchPattern, bool recursive)
        {
            if (!dir.Exists)
                return Enumerable.Empty<DirectoryInfo>();

            return dir.GetDirectories(searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<DirectoryInfo> GetDirectories(this IEnumerable<DirectoryInfo> dirs, string searchPattern, bool recursive)
        {
            List<DirectoryInfo> dis = new List<DirectoryInfo>();
            foreach (var dir in dirs)
                dis.AddRange(dir.GetDirectories(searchPattern, recursive));
            return dis;
        }

        public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo dir, string searchPattern, bool recursive)
        {
            if (!dir.Exists)
                return Enumerable.Empty<FileInfo>();

            return dir.GetFiles(searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<FileInfo> GetFiles(this IEnumerable<DirectoryInfo> dirs, string searchPattern, bool recursive)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (var dir in dirs)
                files.AddRange(dir.GetFiles(searchPattern, recursive));
            return files;
        }

        public static long GetStorageSize(string path)
        {
            var dir = new DirectoryInfo(path);
            long size = 0;
            if (dir.Exists)
            {
                size = dir.GetFiles("*.*", true).Sum(f => f.Length);
            }
            return size;
        }

        public static string MakeRelative(string root, string path)
        {
            return path.Replace(root, "").Replace(Path.DirectorySeparatorChar, '/');
        }
    }
}

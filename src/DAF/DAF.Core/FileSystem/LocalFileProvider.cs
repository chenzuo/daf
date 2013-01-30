using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace DAF.Core.FileSystem
{
    public class LocalFileProvider : IFileSystemProvider
    {
        private string root;

        public LocalFileProvider()
        {
            this.root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_files");
        }

        public void SetRootPath(string root)
        {
            this.root = root;
            if (Directory.Exists(root) == false)
                Directory.CreateDirectory(root);
        }

        public DirectoryInfo GetPath(string relativePath, bool createIfNotExists = true)
        {
            string path = root.PathCombine(relativePath);
            var dir = new DirectoryInfo(path);
            if (createIfNotExists && dir.Exists == false)
                dir.Create();

            return dir;
        }

        public FileInfo GetFile(string filePath)
        {
            string path = root.PathCombine(filePath);
            var file = new FileInfo(path);
            return file;
        }

        public IEnumerable<DirectoryInfo> GetPaths(string relativePathPattern, bool recursive = false)
        {
            DirectoryInfo parent = new DirectoryInfo(root);
            if (string.IsNullOrEmpty(relativePathPattern))
            {
                return new DirectoryInfo[] { parent };
            }

            if(recursive)
            {
                return parent.GetDirectories(relativePathPattern, true);
            }
            else
            {
                string[] patterns = relativePathPattern.Split(new char[] { PathSlash }, StringSplitOptions.RemoveEmptyEntries);
                List<DirectoryInfo> dirs = new List<DirectoryInfo>();
                Stack<Tuple<DirectoryInfo, int>> st = new Stack<Tuple<DirectoryInfo, int>>();
                st.Push(new Tuple<DirectoryInfo, int>(parent, 0));
                while (st.Count > 0)
                {
                    var p = st.Pop();
                    if (p.Item2 >= patterns.Length)
                        dirs.Add(p.Item1);
                    else
                    {
                        var pattern = patterns[p.Item2];
                        p.Item1.GetDirectories(pattern, false).ForEach(d => st.Push(new Tuple<DirectoryInfo, int>(d, p.Item2 + 1)));
                    }
                }

                return dirs;
            }
        }

        public IEnumerable<FileInfo> GetFiles(string relativePathPattern, string filePattern, bool recursive = false)
        {
            var dirs = GetPaths(relativePathPattern, recursive);
            List<FileInfo> files = new List<FileInfo>();
            dirs.ForEach(d => files.AddRange(d.GetFiles(filePattern, recursive)));

            return files;
        }

        public long GetStorageSize(string relativePath = null)
        {
            string path = root.PathCombine(relativePath);
            var dir = new DirectoryInfo(path);
            long size = 0;
            if (dir.Exists)
            {
                size = dir.GetFiles("*.*", true).Sum(f => f.Length);
            }
            return size;
        }

        public string MakeRelative(string path)
        {
            return path.Replace(root, "").Replace(Path.DirectorySeparatorChar, PathSlash);
        }

        protected virtual bool IsSearchPattern(string pattern)
        {
            return pattern.IndexOf('?') >= 0 || pattern.IndexOf('*') >= 0;
        }

        protected virtual char PathSlash
        {
            get
            {
                return '/';
            }
        }

        public string Root
        {
            get
            {
                return root;
            }
        }
    }
}

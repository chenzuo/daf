using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAF.Core.FileSystem
{
    public interface IFileSystemProvider
    {
        void SetRootPath(string root);
        DirectoryInfo GetPath(string relativePath, bool createIfNotExists = true);
        FileInfo GetFile(string filePath);
        IEnumerable<DirectoryInfo> GetPaths(string relativePathPattern, bool recursive = false);
        IEnumerable<FileInfo> GetFiles(string relativePathPattern, string filePattern, bool recursive = false);
        long GetStorageSize(string relativePath = null);
        string MakeRelative(string path);
        string Root { get; }
    }
}

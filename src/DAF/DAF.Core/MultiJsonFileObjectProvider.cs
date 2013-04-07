using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.Serialization;
using DAF.Core.FileSystem;

namespace DAF.Core
{
    public class MultiJsonFileObjectProvider<T> : IObjectProvider<IEnumerable<T>>
    {
        protected string paths;
        protected string fileName;
        protected IJsonSerializer jsonSerializer;
        protected IFileSystemProvider fileProvider;

        public MultiJsonFileObjectProvider(string paths, string fileName, IFileSystemProvider fileProvider, IJsonSerializer jsonSerializer)
        {
            this.paths = paths;
            this.fileName = fileName;
            this.fileProvider = fileProvider;
            this.fileProvider.SetRootPath("~/".GetPhysicalPath());
            this.jsonSerializer = jsonSerializer;
        }

        protected virtual void InitializeObject(T obj)
        {
        }

        protected virtual void AddToList(List<T> objs, T obj)
        {
            objs.Add(obj);
        }

        public virtual IEnumerable<T> GetObject()
        {
            if (string.IsNullOrEmpty(paths))
                return Enumerable.Empty<T>();

            var ps = paths.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<DirectoryInfo> dirs = new List<DirectoryInfo>();
            foreach (var p in ps)
            {
                dirs.AddRange(fileProvider.GetPaths(p, false));
            }
            List<T> objs = new List<T>();
            dirs.GetFiles(fileName, false)
                .ForEach(f =>
                    {
                        string json = File.ReadAllText(f.FullName);
                        if (!string.IsNullOrEmpty(json))
                        {
                            IEnumerable<T> os = jsonSerializer.Deserialize<IEnumerable<T>>(json);
                            foreach (var obj in os)
                            {
                                InitializeObject(obj);
                                AddToList(objs, obj);
                            }
                        }
                    });
            return objs;
        }

        public virtual void SaveObject(IEnumerable<T> obj)
        {
            throw new NotImplementedException();
        }
  }
}

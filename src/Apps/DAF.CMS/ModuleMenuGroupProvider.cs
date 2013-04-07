using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DAF.Core;
using DAF.Core.Serialization;
using DAF.Core.FileSystem;
using DAF.Web;
using DAF.Web.Menu;

namespace DAF.CMS
{
    public class ModuleMenuGroupProvider : MultiJsonFileObjectProvider<MenuGroup>
    {
        public ModuleMenuGroupProvider(string paths, string fileName, IFileSystemProvider fileProvider, IJsonSerializer jsonSerializer)
            : base(paths, fileName, fileProvider, jsonSerializer)
        {
        }

        protected override void InitializeObject(MenuGroup obj)
        {
        }

        protected override void AddToList(List<MenuGroup> objs, MenuGroup obj)
        {
            var mg = objs.FirstOrDefault(o => o.Name == obj.Name);
            if (mg != null)
                mg.Merge(obj);
            else
                objs.Add(obj);
        }
    }
}

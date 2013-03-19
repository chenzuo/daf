using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Data.BLToolkit;

namespace SqlDataLuceneChange
{
    public class DBSet : IEntitySet
    {
        public string ConnectionString
        {
            get { return "SSODB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(User)
                };
            }
        }
    }
}

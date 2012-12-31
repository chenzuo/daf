using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DAF.Core.Map
{
    public class ReflectionMapProvider : IMapProvider
    {
        public TT Map<TF, TT>(TF obj, Func<TT> existsingObj)
        {
            var mobj = existsingObj();

            var objProps = obj.GetType().GetProperties();
            var mobjProps = mobj.GetType().GetProperties();

            foreach (var op in objProps)
            {
                var mop = mobjProps.FirstOrDefault(o => o.Name == op.Name && o.PropertyType.Equals(op.PropertyType));
                if (mop != null)
                    mop.SetValue(mobj, op.GetValue(obj, null), null);
            }
            return mobj;
        }
    }
}

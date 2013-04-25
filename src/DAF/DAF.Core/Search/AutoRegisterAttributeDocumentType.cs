using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DAF.Core.IOC;

namespace DAF.Core.Search
{
    public class AutoRegisterAttributeDocumentType : IAutoRegister
    {
        public void Register(IIocBuilder builder, Type type)
        {
            var docAttr = type.GetCustomAttribute<DocumentAttribute>(true);
            if (docAttr != null)
            {
                if (string.IsNullOrEmpty(docAttr.TypeName))
                    docAttr.TypeName = type.Name;
                if (docAttr.ObjectType == null)
                    docAttr.ObjectType = type;
                if (docAttr.DocumentBuilder == null)
                    docAttr.DocumentBuilder = typeof(AttributeDocumentBuilder);
                if (docAttr.IndexPathBuilder == null)
                    docAttr.IndexPathBuilder = typeof(LocaleDateTimeIndexPathBuilder);
                if (docAttr.FacetFieldNameProvider == null)
                    docAttr.FacetFieldNameProvider = typeof(LocaleFacetFieldNameProvider);
                AttributedTypes.SearchTypes.Add(type.Name, docAttr);
            }
        }
    }
}

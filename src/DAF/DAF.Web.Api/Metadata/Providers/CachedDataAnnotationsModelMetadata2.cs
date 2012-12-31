using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Metadata.Providers;

namespace DAF.Web.Api.Metadata.Providers
{
    public class CachedDataAnnotationsModelMetadata2 : System.Web.Http.Metadata.Providers.CachedDataAnnotationsModelMetadata
    {
        public CachedDataAnnotationsModelMetadata2(CachedDataAnnotationsModelMetadata prototype, Func<object> modelAccessor)
            : base(prototype, modelAccessor)
        {
            var prototype2 = prototype as CachedDataAnnotationsModelMetadata2;
            if (prototype2 != null)
            {
                this.Attributes = prototype2.Attributes;
            }
        }

        public CachedDataAnnotationsModelMetadata2(DataAnnotationsModelMetadataProvider provider, Type containerType, Type modelType, string propertyName, IEnumerable<Attribute> attributes)
            : base(provider, containerType, modelType, propertyName, attributes)
        {
            this.Attributes = attributes;
        }

        public IEnumerable<Attribute> Attributes { get; private set; }
    }
}

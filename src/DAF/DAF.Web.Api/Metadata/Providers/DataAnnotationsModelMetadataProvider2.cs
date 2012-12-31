using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Metadata.Providers;

namespace DAF.Web.Api.Metadata.Providers
{
    public class DataAnnotationsModelMetadataProvider2 : DataAnnotationsModelMetadataProvider
    {
        protected override CachedDataAnnotationsModelMetadata CreateMetadataFromPrototype(CachedDataAnnotationsModelMetadata prototype, Func<object> modelAccessor)
        {
            return new CachedDataAnnotationsModelMetadata2(prototype, modelAccessor);
        }

        protected override CachedDataAnnotationsModelMetadata CreateMetadataPrototype(IEnumerable<Attribute> attributes, Type containerType, Type modelType, string propertyName)
        {
            return new CachedDataAnnotationsModelMetadata2(this, containerType, modelType, propertyName, attributes);
        }
    }
}

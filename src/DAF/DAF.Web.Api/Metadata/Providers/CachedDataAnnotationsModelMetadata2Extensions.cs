using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.Metadata.Providers;

namespace DAF.Web.Api.Metadata.Providers
{
    public static class CachedDataAnnotationsModelMetadata2Extensions
    {
        public static bool IsRequried(this CachedDataAnnotationsModelMetadata2 metadata)
        {
            return metadata.Attributes.OfType<RequiredAttribute>().Any() || metadata.Attributes.OfType<KeyAttribute>().Any();
        }
    }
}

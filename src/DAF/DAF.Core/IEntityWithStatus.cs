using System;

namespace DAF.Core
{
    public interface IEntityWithStatus
    {
        DataStatus Status { get; set; }
    }
}

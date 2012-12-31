using System;
using System.Runtime.Serialization;

namespace DAF.Core
{
    public interface IServerResponse
    {
        ResponseStatus Status { get; }
        string Message { get; }
    }

    public interface IServerResponse<T> : IServerResponse
    {
        T Data { get; }
    }
}

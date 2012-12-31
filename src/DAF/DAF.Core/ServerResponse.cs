using System;
using System.Runtime.Serialization;

namespace DAF.Core
{
    [DataContract]
    [KnownType(typeof(ResponseStatus))]
    public class ServerResponse : IServerResponse
    {
        [DataMember]
        public ResponseStatus Status { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    [DataContract(Name = "ServerResponseOf{0}{#}")]
    [KnownType(typeof(ResponseStatus))]
    public class ServerResponse<T> : IServerResponse<T>
    {
        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public ResponseStatus Status { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}

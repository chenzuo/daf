using System;
using System.Runtime.Serialization;
using DAF.Core.Localization;

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

    public static class ServerResponseExtensions
    {
        public static ServerResponse On(this ServerResponse response, Func<bool> action, string msgTrue, string msgFalse)
        {
            if (response == null)
                response = new ServerResponse();

            try
            {
                if (action())
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = msgTrue;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = msgFalse;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }
        public static ServerResponse<T> On<T>(this ServerResponse<T> response, Func<T> action, string msgTrue, string msgFalse)
            where T : class
        {
            if (response == null)
                response = new ServerResponse<T>();

            response.Data = null;
            try
            {
                var obj = action();
                if (obj != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = obj;
                    response.Message = msgTrue;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = msgFalse;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

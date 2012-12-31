using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DAF.Core;

namespace DAF.Web.Api.MessageHandlers
{
    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler()
        {
        }

        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Log the request information
            LogRequestLoggingInfo(request);

            // Execute the request
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
            	var response = task.Result;
                // Extract the response logging info then persist the information
                LogResponseLoggingInfo(response);
            	return response;
            });
        }

        private void LogRequestLoggingInfo(HttpRequestMessage request)
        {
            string info = string.Format("{0},{1},{2}:",
                request.RequestUri.AbsoluteUri, request.Method.Method, request.Headers.Host);
                        
            if (request.Content != null)
            {
                request.Content.ReadAsByteArrayAsync()
                    .ContinueWith(task =>
                    {
                        info += Encoding.UTF8.GetString(task.Result);
                        LogHelper.Logger.Information(info);
                    });

                return;
            }
        }

        private void LogResponseLoggingInfo(HttpResponseMessage response)
        {
            string info = string.Format("{0},{1},{2},{3}:",
                response.RequestMessage.RequestUri.AbsoluteUri, response.RequestMessage.Method.Method, response.RequestMessage.Headers.Host, response.StatusCode);
            
            if (response.Content != null)
            {
                response.Content.ReadAsByteArrayAsync()
                    .ContinueWith(task =>
                    {
                        info += Encoding.UTF8.GetString(task.Result);
                        LogHelper.Logger.Information(info);
                    });

                return;
            }
        }
    }
}
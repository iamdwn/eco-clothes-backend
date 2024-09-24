using System.Net;

namespace EventBus.Response
{
    public class ResponseObject<T>
    {
        public T? Data { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public ResponseObject(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }

        public ResponseObject(T? data, HttpStatusCode status, string message)
        {
            Data = data;
            Status = status;
            Message = message;
        }

        public ResponseObject() { }
    }
}

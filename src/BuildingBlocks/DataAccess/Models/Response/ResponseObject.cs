using System.Net;

namespace DataAccess.Models.Response
{
    public class ResponseObject
    {
        //public T? Data { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public ResponseObject(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }

        // Success Response
        public static ResponseObject Success() => new(HttpStatusCode.OK, "Success!");

        public static ResponseObject Success(string message) => new(HttpStatusCode.OK, message);

        public static ResponseObject<T> Success<T>(T data) => new(data, HttpStatusCode.OK, "Success!");

        public static ResponseObject<T> Success<T>(T data, string message) => new(data, HttpStatusCode.OK, message);


        // Failure Response
        public static ResponseObject Failure() => new(HttpStatusCode.BadRequest, "Internal Server!");

        public static ResponseObject Failure(string error) => new(HttpStatusCode.BadRequest, error);

        public static ResponseObject<T> Failure<T>(T data) => new(data, HttpStatusCode.BadRequest, "Internal Server!");

        public static ResponseObject<T> Failure<T>(T data, string error) => new(data, HttpStatusCode.BadRequest, error);
    }

    public class ResponseObject<T> : ResponseObject
    {
        public T? Data { get; set; }

        public ResponseObject(T data, HttpStatusCode status, string message) : base(status, message)
        {
            Data = data;
        }
    }
}

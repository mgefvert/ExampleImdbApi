using System.Net;

namespace ImdbData;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public AppException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public static AppException CreateNotAllowed() => new(HttpStatusCode.Forbidden, "Create object is not allowed");
    public static AppException DeleteNotAllowed() => new(HttpStatusCode.Forbidden, "Delete object is not allowed");
    public static AppException GetNotAllowed() => new (HttpStatusCode.Forbidden, "Get object is not allowed");
    public static AppException ListNotAllowed() => new(HttpStatusCode.Forbidden, "List objects is not allowed");
    public static AppException NotFound(string id) => new(HttpStatusCode.NotFound, $"Object '{id}' not found");
    public static AppException UpdateNotAllowed() => new(HttpStatusCode.Forbidden, "Update object is not allowed");
}

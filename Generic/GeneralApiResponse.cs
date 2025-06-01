namespace Event_Organization_System.Generic;

public class GeneralApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; }

    public GeneralApiResponse()
    {
        Errors = new List<string>();
        Timestamp = DateTime.UtcNow;
    }

    public static GeneralApiResponse<T> Success(T data, string message = "Operation completed successfully", int statusCode = 200)
    {
        return new GeneralApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            StatusCode = statusCode,
            Errors = new List<string>()
        };
    }

    public static GeneralApiResponse<T> Success(string message = "Operation completed successfully", int statusCode = 200)
    {
        return new GeneralApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = default(T),
            StatusCode = statusCode,
            Errors = new List<string>()
        };
    }

    public static GeneralApiResponse<T> Failure(string error, int statusCode = 400)
    {
        return new GeneralApiResponse<T>
        {
            IsSuccess = false,
            Message = "Operation failed",
            Data = default(T),
            StatusCode = statusCode,
            Errors = new List<string> { error }
        };
    }

    public static GeneralApiResponse<T> Failure(List<string> errors, int statusCode = 400)
    {
        return new GeneralApiResponse<T>
        {
            IsSuccess = false,
            Message = "Operation failed",
            Data = default(T),
            StatusCode = statusCode,
            Errors = errors ?? new List<string>()
        };
    }

    public static GeneralApiResponse<T> Failure(string message, List<string> errors, int statusCode = 400)
    {
        return new GeneralApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default(T),
            StatusCode = statusCode,
            Errors = errors ?? new List<string>()
        };
    }
}

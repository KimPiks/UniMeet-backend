namespace UniMeet.API.Responses;

public class ApiResponse<T>
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public DateTime TimeStampUtc { get; private set; } = DateTime.UtcNow;
    public T Data { get; private set; }
    
    private ApiResponse(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
    
    public static ApiResponse<T> Ok(T data, string message)
    {
        return new ApiResponse<T>(true, message, data);
    }
    
    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T>(false, message, default!);
    }
}
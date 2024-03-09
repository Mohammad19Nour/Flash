namespace ProjectP.Errors;
public class ApiException : ApiResponse
{
    public ApiException(int statusCode, string? message = null,string? details = null) 
        : base(statusCode: statusCode ,messageEN: message)
    {
        Details = details!;
    }

    public string Details { get; set; }
}
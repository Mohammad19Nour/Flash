namespace ProjectP.Errors;

public class ApiOkResponse <T>: ApiResponse
{
    public T Data { get; }
    public int StatusCode { get; set; }
    public ApiOkResponse(T result) : base(200)
    {
        Data = result;
        StatusCode = 200;
    }
}
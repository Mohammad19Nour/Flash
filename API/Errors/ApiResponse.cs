namespace ProjectP.Errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string? messageEN = null,string? messageAR = null)
    {
        StatusCode = statusCode;
        MessageEN = messageEN ?? GetDefaultMessageForStatusCode(statusCode);
        MessageAR = messageAR;

        if (StatusCode == 200) Status = true;
    }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Success",
            400 => "A bad request, you have made",
            401 => "Authorized, you are not",
            403 => "Forbidden",
            404 => "Resource, it was not",
            405 => "Methode type, it was wrong",
            500 => "Internal Server Error",
            _ => "Default error message"

        };
    }

    public int StatusCode { get; set; }
    public bool Status { get; set; } = false;
    public string MessageEN { get; set; }
    public string MessageAR { get; set; }
}
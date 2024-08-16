using System.Text.Json.Serialization;

public class BaseResponse
{
	[JsonPropertyName("success")]
    public bool Success { get; set; }
	[JsonPropertyName("result")]
	public object? Result { get; set; }
    [JsonPropertyName("error")]
	public string Error { get; set; }

    public BaseResponse(bool success, object? result, string error)
    {
        Error = error;
        Result = result;
		Success = success;
    }

    // Successful response without any result
    public BaseResponse()
    {
        Success = true;
    }

    // Successful response with a result
    public BaseResponse(object? result) : this()
    {
        Result = result;
    }

    // Unsuccessful response with an error message
    public BaseResponse(string error)
    {
        Success = false;
        Error = error;
    }
}

namespace Strider.Posterr.Domain.Dtos;

public class PostCreateResponse
{
    private PostCreateResponse(bool success, string? errorMessage=null)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
    public bool Success { get;  }
    public string? ErrorMessage { get;  }

    public static PostCreateResponse WithSuccess()
    {
        var response = new PostCreateResponse(true);
        return response;
    }

    public static PostCreateResponse WithError(string message)
    {
        var response = new PostCreateResponse(false, message);
        return response;
    }
}
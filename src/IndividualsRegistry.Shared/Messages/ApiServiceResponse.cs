using System.Net;

namespace IndividualsRegistry.Shared.Messages;

public abstract class ApiServiceResponse
{
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ValidationErrors { get; set; } = [];
}

public class ApiServiceResponse<T> : ApiServiceResponse
{
    public ApiServiceResponse() { }

    public ApiServiceResponse(T data, ApiServiceResponse response)
    {
        Message = response.Message;
        StatusCode = response.StatusCode;
        ValidationErrors = response.ValidationErrors;
        Data = data;
    }
    public T Data { get; set; }
}

public class SuccessApiServiceResponse : ApiServiceResponse
{
    public SuccessApiServiceResponse()
    {
        StatusCode = HttpStatusCode.OK;
    }
}

public class SuccessApiServiceResponse<T> : ApiServiceResponse<T>
{
    public SuccessApiServiceResponse(T data, string message = null)
    {
        Data = data;
        StatusCode = HttpStatusCode.OK;
        Message = message;
    }
}

public class ValidationFailedApiServiceResponse : ApiServiceResponse
{
    public ValidationFailedApiServiceResponse(string param)
    {
        Message = $"Invalid parameter '{param}'";
        StatusCode = HttpStatusCode.BadRequest;
    }

    public void AddError(string error)
    {
        ValidationErrors.Add(error);
    }
}

public class ValidationFailedApiGenericServiceResponse<T> : ApiServiceResponse<T>
{
    public ValidationFailedApiGenericServiceResponse(string param)
    {
        Message = $"Invalid parameter '{param}'";
        StatusCode = HttpStatusCode.BadRequest;
    }
}

public class ExternalServiceFailedApiGenericServiceResponse<T> : ApiServiceResponse<T>
{
    public ExternalServiceFailedApiGenericServiceResponse(string serviceName, string message, string status)
    {
        Message = $"External service '{serviceName}' failed: {status} message: {message}"; ;
        StatusCode = HttpStatusCode.InternalServerError;
    }
}

public class InternalServiceFailedApiServiceResponse : ApiServiceResponse
{
    public InternalServiceFailedApiServiceResponse(string message = null)
    {
        Message = $"Internal service error '{message}'"; ;
        StatusCode = HttpStatusCode.InternalServerError;
    }
}

public class BadRequestApiServiceResponse<T> : ApiServiceResponse<T>
{
    public BadRequestApiServiceResponse(T data, string message = null, List<string> validationErrors = null)
    {
        Message = message;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public BadRequestApiServiceResponse(string message = null, List<string> validationErrors = null)
    {
        StatusCode = HttpStatusCode.BadRequest;
        Message = message;
        ValidationErrors = validationErrors;
    }
}

public class NotFoundApiServiceResponse<T> : ApiServiceResponse<T>
{
    public NotFoundApiServiceResponse(string message = null)
    {
        StatusCode = HttpStatusCode.NotFound;
        Message = message;
    }
}

namespace Api.Common.Model;

public sealed class Result<T> where T : class
{
    public Guid RequestId { get; private set; }
    public T? Data { get; private set; }
    public int StatusCode { get; private set; }
    public string? Message { get; private set; }
    public string? Detail { get; private set; }


    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            RequestId = Guid.NewGuid(),
            Data = data,
            StatusCode = 200,
            Message = MessageConstant.ResultMessage.Success
        };
    }

    public static Result<T> Error(string message)
    {
        return new Result<T>
        {
            RequestId = Guid.NewGuid(),
            Data = null,
            StatusCode = 500,
            Message = MessageConstant.ResultMessage.Error,
            Detail = message
        };
    }

    public static Result<T> NotFound(string message)
    {
        return new Result<T>
        {
            RequestId = Guid.NewGuid(),
            Data = null,
            StatusCode = 404,
            Message = MessageConstant.ResultMessage.NotFound,
            Detail = message
        };
    }

    public static Result<T> BadRequest(string message)
    {
        return new Result<T>
        {
            RequestId = Guid.NewGuid(),
            Data = null,
            StatusCode = 400,
            Message = MessageConstant.ResultMessage.BadRequest,
            Detail = message
        };
    }
}
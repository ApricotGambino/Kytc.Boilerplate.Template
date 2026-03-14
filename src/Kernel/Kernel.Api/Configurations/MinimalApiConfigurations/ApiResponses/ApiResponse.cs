// ApiResponse.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;

//TODO: Document all this.
//NOTE: Should a 200 HTTP request contain an error?
//I think so.  I believe this is a philosopical debate, and I'm drawing a line in the sand.
//Different response formats require the consumer to handle that structure, and that's so much extra work.
//The 200 indicates the transmission was good, the business logic behind it happened. The results just may not be what you want.

//I believe you absolutely can get a 200 with an 'IsSuccess' of false.
//"I heard what you asked for, but what you've asked for is wrong."

//I believe the server itself should be responsible for everything else.  We CAN of course provide additional information, like unauthorized, or even 500 errors.
public class ApiResponse
{
    public ApiResponse(bool isSuccess, ApiError error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public ApiError Error { get; }

    public static ApiResponse Success() => new(true, ApiError.None);
    public static ApiResponse<T> Success<T>(T data) => new(true, ApiError.None, data);
    public static ApiResponse Failure(ApiError error) => new(false, error);
    public static ApiResponse<T> Failure<T>(ApiError error) => new(false, error, default);

    public static Ok<ApiResponse<T>> Ok<T>(T data)
    {
        return TypedResults.Ok(Success(data));
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; }

    public ApiResponse(bool isSuccess, ApiError error, T? data) : base(isSuccess, error)
    {
        Data = data;
    }
}


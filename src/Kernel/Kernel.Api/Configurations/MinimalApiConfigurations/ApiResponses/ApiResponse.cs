// ApiResponse.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository.

using Kernel.Data.Mapping;
using Kernel.Infrastructure.Extensions.Pagination;
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
    public static ApiResponse<T> Success<T>(T data) where T : IDto => new(true, ApiError.None, data);
    public static ApiResponse Failure(ApiError error) => new(false, error);
    public static ApiResponse<T> Failure<T>(ApiError error) where T : IDto => new(false, error, default);
    public static ApiResponse ValidationFailure(ApiError error) => new(false, error);

    public static Ok<ApiResponse<TDto>> Ok<TEntity, TDto>(TEntity data)
        where TDto : IDto, IMap<TEntity, TDto>
    {
        var mappedEntity = TDto.MapFrom(data);
        return TypedResults.Ok(Success(mappedEntity));
    }

    public static Ok<ApiResponse<T>> Ok<T>(T data)
        where T : IDto
    {
        return TypedResults.Ok(Success(data));
    }

    //public static Ok<ApiResponse<ApiPagedResults<TDto>>> Ok<TDto, TBaseEntity>(PagedResults<TBaseEntity> data)
    //    where TDto : IDto, IMap<TBaseEntity, TDto>
    //    where TBaseEntity : BaseEntity
    //{

    //    var test = data.Convert2().To2<ApiPagedResults<TDto>>();



    //    var pagedDto = new ApiPagedResults<TDto>();
    //    pagedDto.PageSize = data.PageSize;
    //    pagedDto.TotalPages = data.TotalPages;
    //    pagedDto.Page = data.Page;
    //    pagedDto.TotalItems = data.TotalItems;
    //    pagedDto.Results = new List<TDto>();

    //    foreach (var entity in data.Results)
    //    {
    //        var uh = TDto.MapFrom(entity);
    //        pagedDto.Results.Add(uh);
    //    }

    //    return TypedResults.Ok(Success(pagedDto));
    //}

}

public class ApiResponse<T> : ApiResponse
where T : IDto
{
    public T? Data { get; }

    public ApiResponse(bool isSuccess, ApiError error, T? data) : base(isSuccess, error)
    {
        Data = data;
    }
}

public static class MapperExtensions
{
    public sealed class Mapper<T>
    {
        private readonly PagedResults<T>? _pagedSource;
        private readonly T? _source;

        public Mapper(T source)
        {
            _source = source;
        }
        public Mapper(PagedResults<T> pagedSource)
        {
            _pagedSource = pagedSource;
        }

        public TTo To1<TTo>()
        where TTo : IMap<T, TTo>
        {
            return TTo.MapFrom(_source);
        }

        public ApiPagedResults<TDto1> To2<TDto1>()
        where TDto1 : IMap<T, TDto1>//, IDto
        {
            var pagedResults = _pagedSource as PagedResults<T>;
            var pagedDto = new ApiPagedResults<TDto1>();
            pagedDto.PageSize = pagedResults.PageSize;
            pagedDto.TotalPages = pagedResults.TotalPages;
            pagedDto.Page = pagedResults.Page;
            pagedDto.TotalItems = pagedResults.TotalItems;
            pagedDto.Results = new List<TDto1>();

            foreach (var entity in pagedResults.Results)
            {
                var uh = TDto1.MapFrom(entity);
                pagedDto.Results.Add(uh);
            }

            return pagedDto;
        }
    }

    //TODO: Rename this.
    public static Mapper<T> Convert1<T>(this T entity)
    {
        return new Mapper<T>(entity);
    }

    public static Mapper<T> Convert2<T>(this PagedResults<T> entity)
    {
        return new Mapper<T>(entity);
    }

    public static Mapper<PagedResults<T>> Convert3<T>(this PagedResults<T> entity)
    {
        return new Mapper<PagedResults<T>>(entity);
    }

}

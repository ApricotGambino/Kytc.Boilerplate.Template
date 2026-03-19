using Application.Features.ExampleFeature.Services;
using Data.Entities.Example;
using FluentValidation;
using Kernel.Api.Configurations.MinimalApiConfigurations;
using Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;
using Kernel.Data.Entities;
using Kernel.Data.Mapping;
using Microsoft.AspNetCore.Http.HttpResults;
//TODO: Explain why we used 'TypedResults'
namespace Api.Endpoints
{
    public class ExampleEndpoint : BaseEndpointGroup
    {
        private readonly IExampleService _exampleService;

        public ExampleEndpoint(IExampleService exampleService) => _exampleService = exampleService;

        public override void Map(RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet(GetExampleEntityGeneratedDtoByIdAsync);

            //groupBuilder.MapGet(asdfasdfasdf);


            //groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingContextAsync);
            //groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync);
            //groupBuilder.MapGet(GetMostRecentExampleEntitiesPaginatedAsync);
            //groupBuilder.MapGet(GetExampleEntityCustomDtoByIdAsync);

            //groupBuilder.MapGet(ThrowExpectedExceptionAsync);
            //groupBuilder.MapGet(ThrowUnexpectedExceptionAsync);
            //groupBuilder.MapGet(ReturnApiErrorAsync);



            //groupBuilder.MapPost(AddExampleEntityAsync);
            //groupBuilder.MapPost(AddExampleEntityWithCustomDtoAsync);
        }


        public async Task<Ok<ApiResponse<ExampleEntityGeneratedDto>>> GetExampleEntityGeneratedDtoByIdAsync(int id)
        {
            var entity = await _exampleService.GetExampleEntityByIdAsync(id);
            return ApiResponse.Ok(ExampleEntityGeneratedDto.MapFrom(entity));
        }

        //public async Task<Ok<ApiResponse<ApiPagedResults<ExampleEntityGeneratedDto>>>> GetMostRecentExampleEntitiesAsync(int pageNumber, int pageSize)
        //{

        //    //This method will call the service layer to get the most recent example entities.
        //    //We could do that two ways:
        //    //  var entities = await _exampleService.GetMostRecentExampleEntitiesUsingContextAsync(pageNumber, pageSize)
        //    //  var entities = await _exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(pageNumber, pageSize)
        //    //In general, you should write your methods to call a read only repo when fetching data, so we'll use that here.

        //    var entities = await _exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(pageNumber, pageSize);

        //    //entities here returns a PagedResult object containing the list of entities.

        //    //NOTE: This showcases returning a PagedResult list of entities,
        //    //but in order to return those back to the client, we have to map those entities to a DTO.
        //    //If you try to return the entities without mapping, the compiler will complain about the ApiPagedResults not having a type of IDto.
        //    var entities = await _exampleService.GetMostRecentExampleEntitiesUsingContextAsync(pageNumber, pageSize);
        //    var pagedDto = entities.Convert().To<ExampleEntityGeneratedDto>();

        //    return ApiResponse.Ok(pagedDto);
        //}

        //public async Task<Ok<ApiResponse<ApiPagedResults<ExampleEntityGeneratedDto>>>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(int pageNumber, int pageSize)
        //{
        //    //NOTE: This is pretty much identical to the GetMostRecentExampleEntitiesUsingContextAsync method, but we're just calling a different
        //    //service method to get the data.
        //    var entities = await _exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(pageNumber, pageSize);
        //    var pagedDto = entities.Convert().To<ExampleEntityGeneratedDto>();

        //    return ApiResponse.Ok(pagedDto);
        //}

        //public async Task<Ok<ApiResponse<ExampleEntityCustomDto>>> GetExampleEntityCustomDtoByIdUsingDefaultMapperAsync(int id)
        //{
        //    var entity = await _exampleService.GetExampleEntityByIdAsync(id);
        //    return ApiResponse.Ok<ExampleEntity, ExampleEntityCustomDto>(entity);

        //    //NOTE: You could also manually use the default mapper:
        //    //var mappedDto = ExampleEntityCustomDto.MapFrom(entity);
        //    //return ApiResponse.Ok(mappedDto);
        //}

        //public async Task<Ok<ApiResponse<ExampleEntityCustomDto>>> GetExampleEntityCustomDtoByIdUsingCustomMapperAsync(int id)
        //{
        //    var entity = await _exampleService.GetExampleEntityByIdAsync(id);
        //    var complexMappedDto = ExampleEntityCustomDto.ComplexMapFrom(entity, id);
        //    return ApiResponse.Ok(complexMappedDto);
        //}

        //public async Task<Ok<ApiResponse<ExampleEntityCustomDto>>> asdfasdf(int id)
        //{
        //    var entity = await _exampleService.GetExampleEntityByIdAsync(id);

        //    var generatedDto = ExampleEntityCustomDto.ComplexMapFrom(entity, id);
        //    return ApiResponse.Ok(generatedDto);
        //}

        //public async Task<Ok<ExampleEntityGeneratedDto>> AddExampleEntityAsync(ExampleEntityGeneratedCreateRequestDto createRequest)
        //{
        //    var entityToCreate = createRequest.MapToEntity();

        //    var createdEntity = await _exampleService.AddExampleEntityAsync(entityToCreate);

        //    return TypedResults.Ok(ExampleEntityGeneratedDto.MapFrom(createdEntity));
        //}

        //public async Task<Ok<ExampleEntityCustomDto>> AddExampleEntityWithCustomDtoAsync(ExampleEntityCustomCreateRequestDto createRequest)
        //{
        //    var entityToCreate = new ExampleEntity()
        //    {
        //        AString = createRequest.AString ?? createRequest.ACompletelyNewValueNotFromTheEntity,
        //        AStringWithNumbers = createRequest.AStringWithNumbers,
        //        ANumber = createRequest.ANumber,
        //        ABool = createRequest.ABool,
        //        ADateTimeOffset = createRequest.ADateTimeOffset,
        //        AFutureDate = createRequest.AFutureDate
        //    };

        //    var createdEntity = await _exampleService.AddExampleEntityAsync(entityToCreate);

        //    var exampleCustomDto = new ExampleEntityCustomDto()
        //    {
        //        Id = createdEntity.Id,
        //        AString = createdEntity.AString,
        //        AStringWithNumbers = createdEntity.AStringWithNumbers,
        //        ANumber = createdEntity.ANumber,
        //        ABool = createdEntity.ABool,
        //        ADateTimeOffset = createdEntity.ADateTimeOffset,
        //        AFutureDate = createdEntity.AFutureDate,
        //        ACompletelyNewValueNotFromTheEntity = createRequest.AString ?? createRequest.ACompletelyNewValueNotFromTheEntity
        //    };

        //    return TypedResults.Ok(exampleCustomDto);
        //}

        //public static async Task<Ok<ApiResponse>> ThrowExpectedExceptionAsync()
        //{
        //    throw new ApiPresentedException("Something went wrong that wasn't quite validation, and also not quite an internal error like a NullReferenceException, but we did stop the execution, and wanted this message to surface back to the API.");
        //}

        //public static async Task<Ok<ApiResponse>> ReturnApiErrorAsync()
        //{
        //    var apiMessages = new Dictionary<string, string[]>();

        //    apiMessages.Add("This is some error topic", ["This is more information about the topic", "And look!  We can even have more."]);
        //    apiMessages.Add("This is another error topic, not the same as the other.", ["but this one only only has one item"]);
        //    apiMessages.Add("and finally", ["look at the source, and notice how this string is formatted automatically on response"]);

        //    var apiError = new ApiError("This is an example of returning a failure manually.", apiMessages, Activity.Current?.Id ?? string.Empty, System.Net.HttpStatusCode.Unused);

        //    return TypedResults.Ok(ApiResponse.Failure(apiError));
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S112:General or reserved exceptions should never be thrown", Justification = "<Pending>")]
        //public static async Task<Ok<ApiResponse<ExampleEntityGeneratedDto>>> ThrowUnexpectedExceptionAsync()
        //{
        //    throw new NullReferenceException("Everyone's favourite error, a Null Reference Exception!");
        //}
    }




    /// <summary>
    /// This is an example of a custom made DTO.
    /// </summary>
    public record ExampleEntityCustomDto : ExampleEntityGeneratedDto, IMap<ExampleEntity, ExampleEntityCustomDto>
    {
        public required string ACompletelyNewValueNotFromTheEntity { get; set; }
        public string AStringJoinedWithANumber
        {
            get
            {
                return $"{AString}-{ANumber}";
            }
        }

        public static new ExampleEntityCustomDto MapFrom(ExampleEntity entity)
        {
            return new ExampleEntityCustomDto()
            {
                Id = entity.Id,
                AString = entity.AString,
                AStringWithNumbers = entity.AStringWithNumbers,
                ANumber = entity.ANumber,
                ABool = entity.ABool,
                ADateTimeOffset = entity.ADateTimeOffset,
                AFutureDate = entity.AFutureDate,
                ACompletelyNewValueNotFromTheEntity = "This value came from the interface implemented method."
            };
        }

        public static ExampleEntityCustomDto ComplexMapFrom(ExampleEntity entity, int isEven)
        {
            var customDto = MapFrom(entity);
            customDto.ACompletelyNewValueNotFromTheEntity = isEven % 2 == 0 ? "Is Even" : "Is Odd";
            return customDto;
        }
    }
    public record ExampleEntityCustomCreateRequestDto() : IExampleEntityFields
    {
        public required string? AString { get; set; }
        public required string AStringWithNumbers { get; set; }
        public required int ANumber { get; set; }
        public required bool ABool { get; set; }
        public required DateTimeOffset ADateTimeOffset { get; set; }
        public required DateTimeOffset? AFutureDate { get; set; }

        public required string ACompletelyNewValueNotFromTheEntity { get; set; }
    }
    public class ExampleEntityCustomCreateValidator : AbstractValidator<ExampleEntityCustomCreateRequestDto>
    {
        public ExampleEntityCustomCreateValidator()
        {
            //Include the 'normal' validations from the actual entity.
            Include(new ExampleEntityValidator());
            //This will validate the AStringWithNumbers is no greater than 10 characters long, and must contain a number.
            RuleFor(x => x.ACompletelyNewValueNotFromTheEntity).NotEmpty().MaximumLength(2).Must(p => p.Contains("#"))
                .WithMessage($"{nameof(ExampleEntityCustomCreateRequestDto.ACompletelyNewValueNotFromTheEntity)} must contain a '#' symbol.");
        }
    }

    //TODO: Move this where it should go.
    //public class Error
    //{
    //    public Error(string message)
    //    {
    //        Message = message;
    //    }

    //    public string Message { get; }

    //    public static Error None => new(string.Empty);

    //    public static implicit operator Error(string message) => new(message);

    //    public static implicit operator string(Error error) => error.Message;
    //}
    //public class Result<T> : Result
    //{
    //    public T? Data { get; }

    //    public Result(bool isSuccess, Error error, T? data) : base(isSuccess, error)
    //    {
    //        Data = data;
    //    }
    //}
    //public class Result
    //{
    //    public Result(bool isSuccess, Error error)
    //    {
    //        IsSuccess = isSuccess;
    //        Error = error;
    //    }

    //    public bool IsSuccess { get; }
    //    public Error Error { get; }

    //    public static Result Success() => new(true, Error.None);

    //    public static Result Failure(Error error) => new(false, error);

    //    public static Result<T> Success<T>(T data) => new(true, Error.None, data);

    //    public static Result<T> Failure<T>(Error error) => new(false, error, default);

    //    public static Ok<Result<T>> TEST<T>(T data)
    //    {
    //        return TypedResults.Ok(Success(data));
    //    }
    //}
    //public record ApiResponse<T>
    //    where T : new()
    //{
    //    public bool IsSuccess { get; set; }

    //    public HttpStatusCode StatusCode { get; set; }

    //    public string Message { get; set; }

    //    public T? Data { get; set; }

    //    public Exception? Exception { get; set; }


    //    public ApiResponse(T data)
    //    {
    //        IsSuccess = true;
    //        StatusCode = HttpStatusCode.OK;
    //        Message = "Operation completed successfully";

    //    }
    //}


    //TODO: Explain why use records instead of classes.
    //TODO: Explain that we're adding 'required' on each property so that any DTOs that use this DTO can't accidently forget to initialize the value.
    //TODO: Explain why we have a constructor that takes itself?
    //public record ExampleEntityGeneratedDto() : IExampleEntityFields, IPrimaryKey<int>, IMap<ExampleEntity, ExampleEntityGeneratedDto>, IDto
    //{
    //    public required int Id { get; set; }
    //    public required string? AString { get; set; }
    //    public required string AStringWithNumbers { get; set; }
    //    public required int ANumber { get; set; }
    //    public required bool ABool { get; set; }
    //    public required DateTimeOffset ADateTimeOffset { get; set; }
    //    public required DateTimeOffset? AFutureDate { get; set; }

    //    public static ExampleEntityGeneratedDto MapFrom(ExampleEntity entity)
    //    {
    //        return new ExampleEntityGeneratedDto()
    //        {
    //            Id = entity.Id,
    //            AString = entity.AString,
    //            AStringWithNumbers = entity.AStringWithNumbers,
    //            ANumber = entity.ANumber,
    //            ABool = entity.ABool,
    //            ADateTimeOffset = entity.ADateTimeOffset,
    //            AFutureDate = entity.AFutureDate,
    //        };
    //    }
    //}
    public record ExampleEntityGeneratedCreateRequestDto() : IExampleEntityFields
    {
        public required string? AString { get; set; }
        public required string AStringWithNumbers { get; set; }
        public required int ANumber { get; set; }
        public required bool ABool { get; set; }
        public required DateTimeOffset ADateTimeOffset { get; set; }
        public required DateTimeOffset? AFutureDate { get; set; }
    }
    public record ExampleEntityGeneratedFullDto() : ExampleEntityGeneratedDto, IBaseEntityAuditProperties
    {
        public DateTimeOffset CreatedDateTimeOffset { get; set; }
        public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsSoftDeleted { get; set; }
    }
    public static class ExampleEntityGeneratedMappingExtensions
    {
        //public static ExampleEntityGeneratedDto MapToDto(this ExampleEntity entity)
        //{
        //    return new ExampleEntityGeneratedDto()
        //    {
        //        Id = entity.Id,
        //        AString = entity.AString,
        //        AStringWithNumbers = entity.AStringWithNumbers,
        //        ANumber = entity.ANumber,
        //        ABool = entity.ABool,
        //        ADateTimeOffset = entity.ADateTimeOffset,
        //        AFutureDate = entity.AFutureDate,
        //    };
        //}

        public static ExampleEntity MapToEntity(this ExampleEntityGeneratedDto generatedDto)
        {
            return new ExampleEntity()
            {
                Id = generatedDto.Id,
                AString = generatedDto.AString,
                AStringWithNumbers = generatedDto.AStringWithNumbers,
                ANumber = generatedDto.ANumber,
                ABool = generatedDto.ABool,
                ADateTimeOffset = generatedDto.ADateTimeOffset,
                AFutureDate = generatedDto.AFutureDate,
            };
        }

        public static ExampleEntity MapToEntity(this ExampleEntityGeneratedCreateRequestDto generatedDto)
        {
            return new ExampleEntity()
            {
                AString = generatedDto.AString,
                AStringWithNumbers = generatedDto.AStringWithNumbers,
                ANumber = generatedDto.ANumber,
                ABool = generatedDto.ABool,
                ADateTimeOffset = generatedDto.ADateTimeOffset,
                AFutureDate = generatedDto.AFutureDate,
            };
        }
    }

    //public class test()
    //{
    //    public void test1()
    //    {
    //        var a = ExampleEntityService
    //    }
    //}
}


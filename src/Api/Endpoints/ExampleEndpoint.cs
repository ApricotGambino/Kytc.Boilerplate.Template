using Application.Features.ExampleFeature.Services;
using GeneratedDtos;
using Kernel.Api.Configurations.MinimalApiConfigurations;
using Kernel.Api.Configurations.MinimalApiConfigurations.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;
//TODO: Explain why we used 'TypedResults'
//TODO: Explain why use records instead of classes.
//TODO: Explain that we're adding 'required' on each property so that any DTOs that use this DTO can't accidently forget to initialize the value.
//TODO: Explain why we have a constructor that takes itself?
namespace Api.Endpoints
{
    //NOTE: This entire endpoint can be removed, it's here to help guide you how to get started.

    //NOTE: This is an example endpoint.  This file can be read from top-to-bottom, because as we proceed, we'll illustrate how you
    //probably want to structure your own endpoint.


    public class ExampleEndpoint : BaseEndpointGroup
    {

        //TODO: All the documentation here should probably be removed into a readme instead,
        //because it's going to make this very messy.

        //Here we create whatever service instance we'll be using for this endpoint.
        //NOTE: An endpoint doesn't have to only use one service, but there's a good chance that
        //your service should be structure to represent a 1:1 ratio of endpoint-to-service.
        //For example, a 'product' service probably wouldn't be used in a 'user' service.
        //That's not always true, but unless you're seperating your concerns, you might as well have
        //one enormous unorganized endpoint with all services being used.
        private readonly IExampleService _exampleService;

        public ExampleEndpoint(IExampleService exampleService) => _exampleService = exampleService;

        public override void Map(RouteGroupBuilder groupBuilder)
        {
            //This is the 'Map' method, and it's the override from the BaseEndpointGroup,
            //you can expose an endpoint by applying an appropriate HTTP verb with your method handler as seen below.


            //TODO: Explain Get/Post/Put/Delete https://osamadev.medium.com/understanding-http-verbs-in-apis-6514e236263b

            groupBuilder.MapGet(GetExampleEntityIdAsync);
            groupBuilder.MapGet(GetMostRecentExampleEntitiesAsync);

            groupBuilder.MapPost(AddExampleEntityAsync);


            //groupBuilder.MapGet(asdfasdfasdf);



            //groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync);
            //groupBuilder.MapGet(GetMostRecentExampleEntitiesPaginatedAsync);
            //groupBuilder.MapGet(GetExampleEntityCustomDtoByIdAsync);

            //groupBuilder.MapGet(ThrowExpectedExceptionAsync);
            //groupBuilder.MapGet(ThrowUnexpectedExceptionAsync);
            //groupBuilder.MapGet(ReturnApiErrorAsync);



            //groupBuilder.MapPost(AddExampleEntityAsync);
            //groupBuilder.MapPost(AddExampleEntityWithCustomDtoAsync);
        }



        public async Task<Ok<ApiResponse<ExampleEntityResponse>>> GetExampleEntityIdAsync(int id)
        {
            var entity = await _exampleService.GetExampleEntityByIdAsync(id);
            ExampleEntityResponse dto = ExampleEntityResponse.MapFrom(entity);
            return ApiResponse.Ok(dto);
        }

        public async Task<Ok<ApiResponse<ApiPagedResults<ExampleEntityResponse>>>> GetMostRecentExampleEntitiesAsync(int pageNumber, int pageSize)
        {

            //This method will call the service layer to get the most recent example entities.
            //We could do that two ways:
            //  var entities = await _exampleService.GetMostRecentExampleEntitiesUsingContextAsync(pageNumber, pageSize)
            //  var entities = await _exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(pageNumber, pageSize)
            //In general, you should write your methods to call a read only repo when fetching data, so we'll use that here.

            var entities = await _exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(pageNumber, pageSize);

            //entities here returns a PagedResult object containing the list of entities.

            //NOTE: This showcases returning a PagedResult list of entities,
            //but in order to return those back to the client, we have to map those entities to a DTO.
            //If you try to return the entities without mapping, the compiler will complain about the ApiPagedResults not having a type of IDto.
            //var entities = await _exampleService.GetMostRecentExampleEntitiesUsingContextAsync(pageNumber, pageSize);
            //var pagedDto = entities.Convert().To<ExampleEntityGeneratedDto>();

            //var pagedDto = new ApiPagedResults<ExampleEntityResponse>();
            //pagedDto.PageSize = entities.PageSize;
            //pagedDto.TotalPages = entities.TotalPages;
            //pagedDto.Page = entities.Page;
            //pagedDto.TotalItems = entities.TotalItems;
            //pagedDto.Results = new List<ExampleEntityResponse>();

            //foreach (var entity in entities.Results)
            //{
            //    var uh = ExampleEntityResponse.MapFrom(entity);
            //    pagedDto.Results.Add(uh);
            //}

            //var pagedDto = entities.Convert1().To1<ApiPagedResults<ExampleEntityResponse>>();
            var test = entities.Convert2().To2<ExampleEntityResponse>();
            //var a = entities.Convert1().To1<ApiPagedResults<ExampleEntityResponse, ExampleEntity>>();
            //return ApiResponse.Ok<ExampleEntityResponse, ExampleEntity>(entities);
            return ApiResponse.Ok(test);
        }

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

        public async Task<Ok<ApiResponse<ExampleEntityResponse>>> AddExampleEntityAsync(ExampleEntityCreateRequest createRequest)
        {
            var entityToCreate = ExampleEntityCreateRequest.MapFrom(createRequest);

            var createdEntity = await _exampleService.AddExampleEntityAsync(entityToCreate);

            var createdEntityDto = ExampleEntityResponse.MapFrom(createdEntity);

            return ApiResponse.Ok(createdEntityDto);
        }
        public async Task<Ok<ApiResponse<ADifferentExampleEntityResponse>>> AddADifferentExampleEntityAsync(ADifferentExampleEntityCreateRequest createRequest)
        {
            var entityToCreate = ADifferentExampleEntityCreateRequest.MapFrom(createRequest);

            var createdEntity = await _exampleService.AddADifferentExampleEntityAsync(entityToCreate);

            var createdEntityDto = ADifferentExampleEntityResponse.MapFrom(createdEntity);

            return ApiResponse.Ok(createdEntityDto);
        }

        public async Task<Ok<ApiResponse<ExampleEntityResponse>>> UpdateExampleEntityAsync(ExampleEntityUpdateRequest updateRequest)
        {
            var entityToUpdate = ExampleEntityUpdateRequest.MapFrom(updateRequest);

            var updatedEntity = await _exampleService.UpdateExampleEntityAsync(entityToUpdate);

            var updatedEntityDto = ExampleEntityResponse.MapFrom(updatedEntity);

            return ApiResponse.Ok(updatedEntityDto);
        }


    }

}


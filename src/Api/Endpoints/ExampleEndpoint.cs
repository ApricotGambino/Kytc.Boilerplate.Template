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
            groupBuilder.MapPut(UpdateExampleEntityAsync);
            groupBuilder.MapDelete(DeleteExampleEntityAsync);


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

            var test = entities.Convert2().To2<ExampleEntityResponse>();
            return ApiResponse.Ok(test);
        }

        public async Task<Ok<ApiResponse<ExampleEntityResponse>>> AddExampleEntityAsync(ExampleEntityCreateRequest createRequest)
        {
            var entityToCreate = ExampleEntityCreateRequest.MapFrom(createRequest);

            var createdEntity = await _exampleService.AddExampleEntityAsync(entityToCreate);

            var createdEntityDto = ExampleEntityResponse.MapFrom(createdEntity);

            return ApiResponse.Ok(createdEntityDto);
        }

        public async Task<Ok<ApiResponse<ExampleEntityResponse>>> UpdateExampleEntityAsync(ExampleEntityUpdateRequest updateRequest)
        {
            var entityToUpdate = ExampleEntityUpdateRequest.MapFrom(updateRequest);

            var updatedEntity = await _exampleService.UpdateExampleEntityAsync(entityToUpdate);

            var updatedEntityDto = ExampleEntityResponse.MapFrom(updatedEntity);

            return ApiResponse.Ok(updatedEntityDto);
        }

        public async Task<Ok<ApiResponse<ExampleEntityResponse>>> DeleteExampleEntityAsync(int id)
        {
            var deletedEntity = await _exampleService.DeleteExampleEntityAsync(id);

            return ApiResponse.Ok(ExampleEntityResponse.MapFrom(deletedEntity));
        }

    }

}


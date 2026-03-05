using Application.Features.ExampleFeature.Services;
using Data.Entities.Example;
using Kernel.Api.Configurations.MinimalApiConfigurations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints
{
    public class ExampleEndpoint() : BaseEndpointGroup
    {
        public override void Map(RouteGroupBuilder groupBuilder)
        {
            //groupBuilder.RequireAuthorization();

            groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingContextAsync);
            groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync);
            groupBuilder.MapGet(GetExampleEntitiesByIdAsync);
            groupBuilder.MapPost(AddExampleEntityAsync);
        }

        public static async Task<Ok<List<ExampleEntity>>> GetMostRecentExampleEntitiesUsingContextAsync(IExampleService exampleService)
        {
            var entities = await exampleService.GetMostRecentExampleEntitiesUsingContextAsync();

            return TypedResults.Ok(entities);
        }

        public static async Task<Ok<List<ExampleEntity>>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(IExampleService exampleService)
        {
            var entities = await exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync();

            return TypedResults.Ok(entities);
        }

        public static async Task<Ok<ExampleEntity>> GetExampleEntitiesByIdAsync(IExampleService exampleService, int id)
        {
            var entity = await exampleService.GetExampleEntitiesByIdAsync(id);

            return TypedResults.Ok(entity);
        }

        public static async Task<Ok<ExampleEntity>> AddExampleEntityAsync(IExampleService exampleService, ExampleEntity exampleEntity)
        {
            var entity = await exampleService.AddExampleEntityAsync(exampleEntity);

            return TypedResults.Ok(entity);
        }
    }
}


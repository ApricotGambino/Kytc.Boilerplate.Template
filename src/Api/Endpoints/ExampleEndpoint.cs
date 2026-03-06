using Application.Features.ExampleFeature.Services;
using Data.Entities.Example;
using Kernel.Api.Configurations.MinimalApiConfigurations;
using Kernel.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints
{
    public class ExampleEndpoint : BaseEndpointGroup
    {
        static IExampleService? s_exampleService;

        public ExampleEndpoint(IExampleService exampleService) { s_exampleService = exampleService; }

        public override void Map(RouteGroupBuilder groupBuilder)
        {
            //groupBuilder.RequireAuthorization();

            groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingContextAsync);
            groupBuilder.MapGet(GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync);
            groupBuilder.MapGet(GetExampleEntitiesByIdAsync);

            groupBuilder.MapGet(GetTESTByIdAsync);

            groupBuilder.MapPost(AddExampleEntityAsync);

        }

        public static async Task<Ok<List<ExampleEntity>>> GetMostRecentExampleEntitiesUsingContextAsync()
        {
            var entities = await s_exampleService.GetMostRecentExampleEntitiesUsingContextAsync();

            return TypedResults.Ok(entities);
        }

        public static async Task<Ok<List<ExampleEntity>>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync()
        {
            var entities = await s_exampleService.GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync();

            return TypedResults.Ok(entities);
        }

        public static async Task<Ok<ExampleEntity>> GetExampleEntitiesByIdAsync(int id)
        {
            var entity = await s_exampleService.GetExampleEntitiesByIdAsync(id);

            return TypedResults.Ok(entity);
        }

        public static async Task<Ok<ExampleEntityGeneratedDto>> GetTESTByIdAsync(int id)
        {
            var entity = await s_exampleService.GetExampleEntitiesByIdAsync(id);

            return TypedResults.Ok(entity.MapToDto());
        }

        public static async Task<Ok<ExampleEntity>> AddExampleEntityAsync(ExampleEntity exampleEntity)
        {
            //var entity = await exampleService.AddExampleEntityAsync(exampleEntity);
            var entity = await s_exampleService.AddExampleEntityAsync(exampleEntity);


            var validator = new ExampleEntityValidator();

            var result = validator.Validate(exampleEntity);

            return TypedResults.Ok(entity);
        }
    }


    public interface IExampleEntityGeneratedFields
    {
        string AString { get; init; }
        string AStringWithNumbers { get; init; }
        int ANumber { get; init; }
        bool ABool { get; init; }
        DateTimeOffset ADateTimeOffset { get; init; }
    }

    public record ExampleEntityGeneratedDto() : IExampleEntityGeneratedFields, IPrimaryKey<int>
    {
        public required int Id { get; set; }
        public required string AString { get; init; }
        public required string AStringWithNumbers { get; init; }
        public required int ANumber { get; init; }
        public required bool ABool { get; init; }
        public required DateTimeOffset ADateTimeOffset { get; init; }
    }

    public record ExampleEntityGeneratedCreateRequestDto() : IExampleEntityGeneratedFields
    {
        public required string AString { get; init; }
        public required string AStringWithNumbers { get; init; }
        public required int ANumber { get; init; }
        public required bool ABool { get; init; }
        public required DateTimeOffset ADateTimeOffset { get; init; }
    }

    public record ExampleEntityGeneratedFullDto() : ExampleEntityGeneratedDto, IBaseEntityAuditProperties
    {
        public DateTimeOffset CreatedDateTimeOffset { get; set; }
        public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsSoftDeleted { get; set; }
    }

    public static class ExampleEntityMappingExtensions
    {
        public static ExampleEntityGeneratedDto MapToDto(this ExampleEntity entity)
        {
            return new ExampleEntityGeneratedDto()
            {
                Id = entity.Id,
                AString = entity.AString,
                AStringWithNumbers = entity.AStringWithNumbers,
                ANumber = entity.ANumber,
                ABool = entity.ABool,
                ADateTimeOffset = entity.ADateTimeOffset,
            };
        }
    }




}


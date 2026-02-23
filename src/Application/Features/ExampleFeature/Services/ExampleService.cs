//namespace Application.Features.ExampleFeature.Services;

//using System.Collections.Generic;
//using Data.Entities.Example;
//using Data.EntityFramework;
//using KernelInfrastructure.Repositories;
//using Microsoft.EntityFrameworkCore;


//TODO: Create a set of examples inside the Sandbox Test project to illustrate
//why you should not call the context directly, but instead use the repo, (Because you might forget to
//use the things that make stuff more efficient)

//TODO: Also, GetMostRecentLogsUsingContextAsync is an example of why we need to use repos instead of
//services.  Services should be for logic, not fetching data. Explain that too.

//TODO: Should services return entities, or DTOs?
//public interface IExampleService
//{
//    public List<ExampleEntity> DoSomeExampleAction();
//    public Task<List<ExampleEntity>> GetMostRecentEntitiesUsingContextAsync();
//    public Task<List<ExampleEntity>> GetMostRecentEntitiesUsingReadOnlyRepoAsync();
//}

//public class ExampleService(ApplicationDbContext context) : IExampleService
//{
//    private readonly ApplicationDbContext _Context = context;

//    public List<ExampleEntity> DoSomeExampleAction()
//    {
//        return [];
//    }
//    public Task<List<ExampleEntity>> GetMostRecentEntitiesUsingContextAsync()
//    {
//        return _Context.ExampleEntities.OrderByDescending(o => o.Id).ToListAsync();
//    }

//    public Task<List<ExampleEntity>> GetMostRecentEntitiesUsingReadOnlyRepoAsync()
//    {
//        var logContext = new ReadOnlyEntityRepo<ExampleEntity>(_Context);
//        return logContext.GetEntityQueryable().OrderByDescending(o => o.Id).ToListAsync();
//    }

//}

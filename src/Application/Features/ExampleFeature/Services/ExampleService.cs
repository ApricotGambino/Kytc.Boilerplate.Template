namespace Application.Features.ExampleFeature.Services;

using System.Collections.Generic;
using Data.Entities.Example;
using Data.EntityFramework;
using Kernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


//TODO: Create a set of examples inside the Sandbox Test project to illustrate
//why you should not call the context directly, but instead use the repo, (Because you might forget to
//use the things that make stuff more efficient)

//TODO: Also, GetMostRecentLogsUsingContextAsync is an example of why we need to use repos instead of
//services.Services should be for logic, not fetching data.Explain that too.

//TODO: Should services return entities, or DTOs?
public interface IExampleService
{
    public Task<List<ExampleEntity>> GetMostRecentExampleEntitiesUsingContextAsync();
    public Task<List<ExampleEntity>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync();
    public Task<ExampleEntity> GetExampleEntityByIdAsync(int id);
    public Task<ExampleEntity> AddExampleEntityAsync(ExampleEntity exampleEntityToAdd);
}

public class ExampleService(ApplicationDbContext context) : IExampleService
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<ExampleEntity>> GetMostRecentExampleEntitiesUsingContextAsync()
    {
        return _context.ExampleEntities.OrderByDescending(o => o.Id).ToListAsync();
    }

    public Task<List<ExampleEntity>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync()
    {
        var repo = new ReadOnlyEntityRepo<ExampleEntity, ApplicationDbContext>(_context);
        return repo.GetEntityQueryable().OrderByDescending(o => o.Id).ToListAsync();
    }

    public async Task<ExampleEntity> AddExampleEntityAsync(ExampleEntity exampleEntityToAdd)
    {
        await _context.ExampleEntities.AddAsync(exampleEntityToAdd);
        await _context.SaveChangesAsync();
        return exampleEntityToAdd;
    }

    public Task<ExampleEntity> GetExampleEntityByIdAsync(int id)
    {
        var repo = new ReadOnlyEntityRepo<ExampleEntity, ApplicationDbContext>(_context);
        return repo.GetSingleEntityByIdAsync(id);
    }

}

namespace Application.Features.ExampleFeature.Services;

using System.Collections.Generic;
using Data.Entities.ADifferentExampleSchema;
using Data.Entities.ExampleSchema;
using Data.EntityFramework;
using Kernel.Infrastructure.Extensions.Pagination;
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
    public Task<ExampleEntity> GetExampleEntityByIdAsync(int id);
    public Task<ExampleEntity> AddExampleEntityAsync(ExampleEntity entityToAdd);
    public Task<ExampleEntity> UpdateExampleEntityAsync(ExampleEntity entityToUpdate);
    public Task<PagedResults<ExampleEntity>> GetMostRecentExampleEntitiesUsingContextAsync(int pageNumber, int pageSize);
    public Task<PagedResults<ExampleEntity>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(int pageNumber, int pageSize);
    public Task<PagedResults<ExampleEntity>> GetMostRecentExampleEntitiesPaginatedAsync(int pageNumber, int pageSize);

    //TODO: Make a new service.
    public Task<ADifferentExampleEntity?> GetADifferentExampleEntityByExampleEntityIdAsync(int id);
    public Task<ADifferentExampleEntity> AddADifferentExampleEntityAsync(ADifferentExampleEntity entityToAdd);
}

public class ExampleService(ApplicationDbContext context) : IExampleService
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<ExampleEntity>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync()
    {
        var repo = new ReadOnlyEntityRepo<ExampleEntity, ApplicationDbContext>(_context);
        return repo.GetEntityQueryable().OrderByDescending(o => o.Id).ToListAsync();
    }

    public async Task<ExampleEntity> UpdateExampleEntityAsync(ExampleEntity entityToUpdate)
    {
        //TODO: Test if this even works.
        _context.ExampleEntities.Update(entityToUpdate);
        //await _context.ExampleEntities.AddAsync(exampleEntityToAdd);
        await _context.SaveChangesAsync();
        return entityToUpdate;
    }

    public async Task<ExampleEntity> AddExampleEntityAsync(ExampleEntity entityToAdd)
    {
        await _context.ExampleEntities.AddAsync(entityToAdd);
        await _context.SaveChangesAsync();
        return entityToAdd;
    }

    public async Task<ADifferentExampleEntity> AddADifferentExampleEntityAsync(ADifferentExampleEntity entityToAdd)
    {
        await _context.ADifferentExampleEntites.AddAsync(entityToAdd);
        await _context.SaveChangesAsync();
        return entityToAdd;
    }

    public Task<ExampleEntity> GetExampleEntityByIdAsync(int id)
    {
        var repo = new ReadOnlyEntityRepo<ExampleEntity, ApplicationDbContext>(_context);
        return repo.GetSingleEntityByIdAsync(id);
    }

    public Task<PagedResults<ExampleEntity>> GetMostRecentExampleEntitiesUsingContextAsync(int pageNumber, int pageSize)
    {
        return _context.ExampleEntities.OrderByDescending(o => o.Id).ToPaginatedResultsAsync(pageNumber, pageSize);
    }

    public Task<PagedResults<ExampleEntity>> GetMostRecentExampleEntitiesUsingReadOnlyRepoAsync(int pageNumber, int pageSize)
    {
        var repo = new ReadOnlyEntityRepo<ExampleEntity, ApplicationDbContext>(_context);
        return repo.GetEntityQueryable().OrderByDescending(o => o.Id).ToPaginatedResultsAsync(pageNumber, pageSize);
    }

    public async Task<PagedResults<ExampleEntity>> GetMostRecentExampleEntitiesPaginatedAsync(int pageNumber, int pageSize)
    {
        var repo = new ReadOnlyEntityRepo<ExampleEntity, ApplicationDbContext>(_context);
        var entities = await repo.GetPaginatedEntityOrderedByIdNewestFirstAsync(pageNumber, pageSize);
        return entities;
    }

    public async Task<ADifferentExampleEntity?> GetADifferentExampleEntityByExampleEntityIdAsync(int id)
    {
        //TODO: Why do I have to include here?
        var differentExample = await _context.ADifferentExampleEntites.Include(p => p.ExampleEntity).Where(p => p.ExampleEntityId == id).FirstOrDefaultAsync();
        return differentExample;
    }
}

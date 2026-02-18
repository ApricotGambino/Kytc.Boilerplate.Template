namespace TestShared.TestObjects;

using System.Collections.Generic;
using KernelInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public interface ITestExampleService
{
    public List<TestEntity> DoSomeExampleAction();
    public Task<List<TestEntity>> GetMostRecentEntitiesUsingContextAsync();
    public Task<List<TestEntity>> GetMostRecentEntitiesUsingReadOnlyRepoAsync();
}

public class TestExampleService(TestingDatabaseContext context, ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext> readonlyRepo) : ITestExampleService
{
    private readonly TestingDatabaseContext _Context = context;
    private readonly ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext> _ReadonlyRepo = readonlyRepo;

    public List<TestEntity> DoSomeExampleAction()
    {
        return [];
    }
    public Task<List<TestEntity>> GetMostRecentEntitiesUsingContextAsync()
    {
        return _Context.TestEntities.OrderByDescending(o => o.Id).ToListAsync();
    }

    public Task<List<TestEntity>> GetMostRecentEntitiesUsingReadOnlyRepoAsync()
    {
        return _ReadonlyRepo.GetEntityQueryable().OrderByDescending(o => o.Id).ToListAsync();
    }

}

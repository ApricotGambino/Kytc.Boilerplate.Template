namespace TestShared.TestObjects;

using System.Collections.Generic;
using KernelInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public interface ITestExampleService
{
    public List<TestEntity> DoSomeExampleAction();
    public Task<List<TestEntity>> GetAllEntitiesUsingContextAsync();
    public Task<List<TestEntity>> GetAllEntitiesUsingReadOnlyRepoAsync();
}

public class TestExampleService(TestingDatabaseContext context, ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext> readonlyRepo) : ITestExampleService
{
    private readonly TestingDatabaseContext _Context = context;
    private readonly ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext> _ReadonlyRepo = readonlyRepo;

    public List<TestEntity> DoSomeExampleAction()
    {
        return [];
    }
    public Task<List<TestEntity>> GetAllEntitiesUsingContextAsync()
    {
        return _Context.TestEntities.ToListAsync();
    }

    public Task<List<TestEntity>> GetAllEntitiesUsingReadOnlyRepoAsync()
    {
        return _ReadonlyRepo.GetEntityQueryable().ToListAsync();
    }

}

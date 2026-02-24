using Kernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TestShared.TestObjects;

public interface ITestExampleService
{
    public List<TestEntity> DoSomeExampleAction();
    public Task<List<TestEntity>> GetAllEntitiesUsingContextAsync();
    public Task<List<TestEntity>> GetAllEntitiesUsingReadOnlyRepoAsync();
}

public class TestExampleService(TestingDatabaseContext context, ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext> readonlyRepo) : ITestExampleService
{
    private readonly TestingDatabaseContext _context = context;
    private readonly ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext> _readonlyRepo = readonlyRepo;

    public List<TestEntity> DoSomeExampleAction()
    {
        return [];
    }
    public Task<List<TestEntity>> GetAllEntitiesUsingContextAsync()
    {
        return _context.TestEntities.ToListAsync();
    }

    public Task<List<TestEntity>> GetAllEntitiesUsingReadOnlyRepoAsync()
    {
        return _readonlyRepo.GetEntityQueryable().ToListAsync();
    }

}

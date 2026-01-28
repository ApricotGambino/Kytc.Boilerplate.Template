namespace Api.UnitTests.ConfigurationTests;

using System.Linq;
using Domain.Entities.Admin;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class asdfasdfasdf : BaseTestFixture
{

    //TODO: Implement a keyset pagination as well.
    private List<Log> Getlogs()
    {
        var list = new List<Log>();
        list.Add(new Log() { Id = 1, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 1", TimeStamp = DateTime.Now.AddDays(-30) });
        list.Add(new Log() { Id = 2, Level = "Fatal", MessageTemplate = "MessageTemplate", Message = "Test log 2", TimeStamp = DateTime.Now.AddDays(-15) });
        list.Add(new Log() { Id = 3, Level = "Warning", MessageTemplate = "MessageTemplate", Message = "Test log 3", TimeStamp = DateTime.Now.AddDays(-2) });
        list.Add(new Log() { Id = 4, Level = "Information", MessageTemplate = "MessageTemplate", Message = "Test log 4", TimeStamp = DateTime.Now });
        return list;
    }

    private List<Log> GetLogsPagination(int pageNumber = 1, int pageSize = 10, string orderByProperty = nameof(BaseEntity.CreatedDate), bool orderByAscending = false)
    {
        var result = Getlogs().AsQueryable();

        Func<Log, Object> orderByPropertyFunc = s => s.GetType().GetProperty(orderByProperty.Trim()).GetValue(s);

        if (orderByAscending)
        {
            result = result.OrderBy(orderByPropertyFunc).AsQueryable();
            var b = 1;
        }
        else
        {
            result = result.OrderByDescending(orderByPropertyFunc).AsQueryable();
        }

        return result
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();
    }



    [Test]
    public async Task dddddddd()
    {
        //var allLogs = Getlogs();
        var logs = GetLogsPagination(1, 2, nameof(Log.TimeStamp));

        Assert.That(logs, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task aaaa()
    {
        //var allLogs = Getlogs();
        var logs = GetLogsPagination(1, 2, nameof(Log.TimeStamp));

        Assert.That(logs, Is.Ordered.Descending.By(nameof(Log.TimeStamp)));
    }
    [Test]
    public async Task bbbb()
    {
        //var allLogs = Getlogs();
        var logs = GetLogsPagination(1, 2, nameof(Log.TimeStamp), true);

        Assert.That(logs, Is.Ordered.Ascending.By(nameof(Log.TimeStamp)));
    }
}



namespace Infrastructure.Features.Logging.Services;

using System.Threading.Tasks;
using Domain.Entities.Admin;
using Domain.Interfaces.Features.Logging;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class LoggingService(ApplicationDbContext context) : ILoggingService
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<Log>> GetMostRecentLogsAsync()
    {

        var logQuery = _context.Logs
                //.Where(p => p.TimeStamp >= DateTime.Now.AddDays(-30))
                .OrderByDescending(p => p.TimeStamp)
                .Take(1000);

        //if (!includeInformationLevel)
        //{
        //    logQuery = logQuery.Where(p => p.Level != LogLevel.Information.ToString());
        //}

        return logQuery.ToListAsync();
    }

    //public Task<List<Log>> GetLogs(int pageSize, int skip)
    //{

    //    var logQuery = _context.Logs
    //            //.Where(p => p.TimeStamp >= DateTime.Now.AddDays(-30))
    //            .OrderByDescending(p => p.TimeStamp)
    //            .Skip(skip)
    //            .Take(pageSize);

    //    //if (!includeInformationLevel)
    //    //{
    //    //    logQuery = logQuery.Where(p => p.Level != LogLevel.Information.ToString());
    //    //}

    //    return logQuery.ToListAsync();
    //}
}

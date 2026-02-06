namespace Infrastructure.Features.Logging.Services;

using System.Collections.Generic;
using Domain.Entities.Admin;
using Domain.Interfaces.Features.Logging;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class LoggingService(ApplicationDbContext context) : ILoggingService
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<Log>> GetMostRecentLogsUsingContextAsync()
    {
        return _context.Logs.OrderByDescending(o => o.Id).ToListAsync();
    }

    public Task<List<Log>> GetMostRecentLogs_RepoBase_GetEntityQueryableAsync()
    {
        var logContext = new ReadOnlyEntityRepo<Log>(_context);
        return logContext.GetEntityQueryable().OrderByDescending(o => o.Id).ToListAsync();
    }
}

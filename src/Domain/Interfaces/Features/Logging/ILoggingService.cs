namespace Domain.Interfaces.Features.Logging;

using System.Collections.Generic;
using Domain.Entities.Admin;

public interface ILoggingService
{
    public Task<List<Log>> GetMostRecentLogsUsingContextAsync();
}

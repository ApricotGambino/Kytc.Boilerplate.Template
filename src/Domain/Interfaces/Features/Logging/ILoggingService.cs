namespace Domain.Interfaces.Features.Logging;

using Domain.Entities.Admin;

public interface ILoggingService
{
    //TODO: This method needs to justify itself outside of the generic 
    //EF fetch generic methods.  
    public Task<List<Log>> GetMostRecentLogsAsync();
}
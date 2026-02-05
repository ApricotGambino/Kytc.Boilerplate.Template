namespace Sandbox;

using Domain.Entities.Admin;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

//NOTE: This is a place where I might do TDD.
//TODO: Update this so it's clear how/what I do. 
public class TDDSandbox
{
    public interface ILoggingService
    {
        //TODO: This method needs to justify itself outside of the generic 
        //EF fetch generic methods.  
        public Task<List<Log>> GetMostRecentLogsAsync();
    }

    public class LoggingService(ApplicationDbContext context) : ILoggingService
    {
        private readonly ApplicationDbContext _context = context;

        public Task<List<Log>> GetMostRecentLogsAsync()
        {
            return _context.Logs.ToListAsync();
        }
    }
}

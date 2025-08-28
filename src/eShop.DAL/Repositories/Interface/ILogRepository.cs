namespace eShop.DAL.Repositories.Interface;

using Core.Entities;

/// <summary>
/// it will get log data and expected to contain most data in all app for that it will be optimize as most
/// </summary>
public interface ILogRepository {
    Task<IEnumerable<LogEntry>> GetLogsAsync(int pageNumber = 1, int pageSize = 50);
    Task<IEnumerable<LogEntry>> GetLogsByLevelAsync(string level, int pageNumber = 1, int pageSize = 50);
    Task<IEnumerable<LogEntry>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 50);
    Task<LogEntry> GetLogByIdAsync(int id);
    Task<int> GetTotalLogsCountAsync();
    Task<int> GetTotalLogsCountByLevelAsync(string level);
    Task<int> GetTotalLogsCountByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task DeleteLogsOlderThanAsync(int days);
}
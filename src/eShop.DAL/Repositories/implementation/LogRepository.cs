namespace eShop.DAL.Repositories.implementation;

using Core.Entities;
using Dapper;
using Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class LogRepository : ILogRepository {
    private readonly string _connectionString;
    private readonly ILogger<LogRepository> _logger;

    public LogRepository(IConfiguration configuration, ILogger<LogRepository> logger) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<IEnumerable<LogEntry>> GetLogsAsync(int pageNumber = 1, int pageSize = 50) {
        using var connection = new SqlConnection(_connectionString);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
            SELECT Id, Message, MessageTemplate, Level, TimeStamp, Exception, Properties
            FROM Logs
            ORDER BY TimeStamp DESC , Id DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        return await connection.QueryAsync<LogEntry>(query, new { Offset = offset, PageSize = pageSize });
    }

    public async Task<IEnumerable<LogEntry>> GetLogsByLevelAsync(string level, int pageNumber = 1, int pageSize = 50) {
        using var connection = new SqlConnection(_connectionString);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
            SELECT Id, Message, MessageTemplate, Level, TimeStamp, Exception, Properties
            FROM Logs
            WHERE Level = @Level
            ORDER BY TimeStamp DESC, Id DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        return await connection.QueryAsync<LogEntry>(query, new { Level = level, Offset = offset, PageSize = pageSize });
    }

    public async Task<IEnumerable<LogEntry>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 50) {
        using var connection = new SqlConnection(_connectionString);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
            SELECT Id, Message, MessageTemplate, Level, TimeStamp, Exception, Properties
            FROM Logs
            WHERE TimeStamp BETWEEN @StartDate AND @EndDate
            ORDER BY TimeStamp DESC , Id DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        return await connection.QueryAsync<LogEntry>(query, new { StartDate = startDate, EndDate = endDate, Offset = offset, PageSize = pageSize });
    }

    public async Task<LogEntry> GetLogByIdAsync(int id) {
        using var connection = new SqlConnection(_connectionString);

        var query = @"
            SELECT Id, Message, MessageTemplate, Level, TimeStamp, Exception, Properties
            FROM Logs
            WHERE Id = @Id";

        return await connection.QuerySingleOrDefaultAsync<LogEntry>(query, new { Id = id });
    }

    public async Task<int> GetTotalLogsCountAsync() {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Logs");
    }

    public async Task<int> GetTotalLogsCountByLevelAsync(string level) {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Logs WHERE Level = @Level", new { Level = level });
    }

    public async Task<int> GetTotalLogsCountByDateRangeAsync(DateTime startDate, DateTime endDate) {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM Logs WHERE TimeStamp BETWEEN @StartDate AND @EndDate",
        new { StartDate = startDate, EndDate = endDate });
    }

    public async Task DeleteLogsOlderThanAsync(int days) {
        using var connection = new SqlConnection(_connectionString);
        var query = "DELETE FROM Logs WHERE TimeStamp < @Date";
        var date = DateTime.Now.AddDays(-days);
        await connection.ExecuteAsync(query, new { Date = date });
    }
}
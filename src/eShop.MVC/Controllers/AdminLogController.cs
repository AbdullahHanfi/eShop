using Microsoft.AspNetCore.Mvc;
namespace eShop.MVC.Controllers;

using BLL.ViewModels.Log;
using Core.Entities;
using Core.Utilities;
using DAL.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = Roles.SuperAdmin)]
[Route("admin")]
public class AdminLogController : Controller {
    private readonly ILogRepository _logRepository;
    private readonly ILogger<AdminLogController> _logger;

    public AdminLogController(ILogRepository logRepository, ILogger<AdminLogController> logger) {
        _logRepository = logRepository;
        _logger = logger;
    }
    [HttpGet("logs")]
    public async Task<IActionResult> Logs(int page = 1, string level = null, DateTime? startDate = null, DateTime? endDate = null) {
        const int pageSize = 20;
        IEnumerable<LogEntry> logs;
        int totalCount;

        if (!string.IsNullOrEmpty(level)){
            logs = await _logRepository.GetLogsByLevelAsync(level, page, pageSize);
            totalCount = await _logRepository.GetTotalLogsCountByLevelAsync(level);
        }
        else if (startDate.HasValue && endDate.HasValue){
            logs = await _logRepository.GetLogsByDateRangeAsync(startDate.Value, endDate.Value, page, pageSize);
            totalCount = await _logRepository.GetTotalLogsCountByDateRangeAsync(startDate.Value, endDate.Value);
        }
        else{
            logs = await _logRepository.GetLogsAsync(page, pageSize);
            totalCount = await _logRepository.GetTotalLogsCountAsync();
        }

        var viewModel = new LogViewModel
        {
            Logs = logs.ToList(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            LevelFilter = level,
            StartDate = startDate,
            EndDate = endDate,
            TotalCount = totalCount
        };

        return View(viewModel);
    }
    [HttpGet("logs/details")]
    public async Task<IActionResult> LogDetails(int id) {
        var log = await _logRepository.GetLogByIdAsync(id);
        if (log == null){ return NotFound(); }

        var viewModel = new LogDetailsViewModel
        {
            Log = log,
            ParsedProperties = ParseProperties(log.Properties)
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CleanupLogs(int daysToKeep = 30) {
        try{
            await _logRepository.DeleteLogsOlderThanAsync(daysToKeep);
            TempData["SuccessMessage"] = $"Successfully deleted logs older than {daysToKeep} days.";
        }
        catch (Exception ex){
            _logger.LogError(ex, "Error cleaning up logs");
            TempData["ErrorMessage"] = "Error occurred while cleaning up logs.";
        }

        return RedirectToAction(nameof(Logs));
    }

    private Dictionary<string, string> ParseProperties(string properties) {
        var result = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(properties))
            return result;

        try{
            // Assuming properties are stored as JSON
            var json = System.Text.Json.JsonDocument.Parse(properties);
            foreach (var property in json.RootElement.EnumerateObject()){ result[property.Name] = property.Value.ToString(); }
        }
        catch{ result["Raw"] = properties; }

        return result;
    }
}
namespace eShop.BLL.ViewModels.Log;

using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

public class LogViewModel {
    public List<LogEntry> Logs { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public string LevelFilter { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TotalCount { get; set; }

    public List<SelectListItem> LevelFilterOptions { get; } = new()
    {
        new SelectListItem { Value = "Verbose", Text = "Verbose" },
        new SelectListItem { Value = "Debug", Text = "Debug" },
        new SelectListItem { Value = "Information", Text = "Information" },
        new SelectListItem { Value = "Warning", Text = "Warning" },
        new SelectListItem { Value = "Error", Text = "Error" },
        new SelectListItem { Value = "Fatal", Text = "Fatal" }
    };
}
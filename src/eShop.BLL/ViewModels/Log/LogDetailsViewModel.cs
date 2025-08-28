namespace eShop.BLL.ViewModels.Log;

using Core.Entities;

public class LogDetailsViewModel {
    public LogEntry Log { get; set; }
    public Dictionary<string, string> ParsedProperties { get; set; }
}
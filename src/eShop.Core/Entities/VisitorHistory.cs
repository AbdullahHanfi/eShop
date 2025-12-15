namespace eShop.Core.Entities;

public class VisitorHistory {
    public Guid Id { get; set; } 
    public string? UserAgent { get; set; }
    public string? Path { get; set; }
    public string? QueryString { get; set; }
    public string? Referrer { get; set; }
    public DateTime Timestamp { get; set; }
}
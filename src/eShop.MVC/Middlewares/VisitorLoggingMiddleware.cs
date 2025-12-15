using eShop.BLL.Services.Abstraction;
using eShop.Core.Entities;
using eShop.DAL.Data;

public class VisitorLoggingMiddleware(RequestDelegate next, ILogger<VisitorLoggingMiddleware> logger) {

    public async Task Invoke(HttpContext context, eShopDbContext dbContext, IEmailSender emailSender) {
        var visitorInfo = new
        {
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            Path = context.Request.Path,
            QueryString = context.Request.QueryString.ToString(),
            Referrer = context.Request.Headers["Referer"].ToString(),
            Timestamp = DateTime.UtcNow
        };
        if (visitorInfo != null){
            var visitor = await dbContext.Visitors.FindAsync(visitorInfo.IpAddress);
            if (visitor != null)
                visitor.Histories.Add(new()
                {
                    UserAgent = visitorInfo.UserAgent,
                    Path = visitorInfo.Path.ToString(),
                    QueryString = visitorInfo.QueryString,
                    Referrer = visitorInfo.Referrer,
                    Timestamp = visitorInfo.Timestamp
                });
            else{
                await dbContext.Visitors.AddAsync(
                new()
                {
                    IpAddress = visitorInfo.IpAddress,
                    Histories = new List<VisitorHistory>()
                    {
                        new()
                        {
                            UserAgent = visitorInfo.UserAgent,
                            Path = visitorInfo.Path.ToString(),
                            QueryString = visitorInfo.QueryString,
                            Referrer = visitorInfo.Referrer,
                            Timestamp = visitorInfo.Timestamp
                        }
                    }
                });
                await emailSender.SendEmailAsync("abdullah.hanfi@protonmail.com", "New Visitor", $"Visitor Ip {visitorInfo.IpAddress}");
            }
            await dbContext.SaveChangesAsync();
        }

        logger.LogInformation("Visitor: {@VisitorInfo}", visitorInfo);

        await next(context);
    }
}
using eShop.BLL.Services.Abstraction;
using Hangfire;
using System.Linq.Expressions;

namespace eShop.MVC.Infrastructure
{
    public class HangfireServices : IBackgroundJobServices
    {
        public string Enqueue(Expression<Action> call)
            => BackgroundJob.Enqueue(call);

        public void Schedule(Expression<Action> call, TimeSpan delay)
            => BackgroundJob.Schedule(call, delay);
    }
}
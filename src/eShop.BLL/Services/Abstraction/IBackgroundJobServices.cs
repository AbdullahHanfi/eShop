using System.Linq.Expressions;

namespace eShop.BLL.Services.Abstraction
{
    public interface IBackgroundJobServices
    {
        string Enqueue(Expression<Action> methodCall);
        void Schedule(Expression<Action> methodCall, TimeSpan delay);
    }
}

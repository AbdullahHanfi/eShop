using eShop.BLL.Services.Abstraction;
using eShop.DAL.Interface;
namespace eShop.BLL.Services.src
{
    internal class RoleService : IRoleService
    {
        private readonly IBackgroundJobServices _backgroundJob;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork,
                           IBackgroundJobServices backgroundJob)
        {
            _unitOfWork = unitOfWork;
            _backgroundJob = backgroundJob;
        }

        public async Task GrantRoleTemporarilyAsync(Guid userId, string role, TimeSpan duration)
        {
            await _unitOfWork.Users.AddToRoleAsync(userId, role);

            _backgroundJob.Schedule(() => RevokeRoleAsync(userId, role), duration);
        }

        public Task RevokeRoleAsync(Guid userId, string role)
            => _unitOfWork.Users.RemoveFromRolesAsync(userId, [role]);
    }
}

using System.Linq.Expressions;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Results;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetFirstOrDefaultAsync(Expression<Func<User, bool>> filter);
        Task<IEnumerable<User>> GetByFilterAsync(Expression<Func<User, bool>> filter);
        Task<User> GetByNetworkUserAsync(string networkUser);
        Task UpdateAsync(User user);
        Task<PermissionResult> GetPermissionResult(int userId);
        Task<User> GetByIdAsync(int id);
        Task CreateAsync(User user);
    }
}

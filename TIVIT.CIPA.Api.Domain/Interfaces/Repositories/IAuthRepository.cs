using System.Linq.Expressions;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<UserAuth> GetFirstOrDefaultAsync(Expression<Func<UserAuth, bool>> filter);
        Task CreateAsync(UserAuth userAuth);
        Task UpdateAsync(UserAuth userAuth);
    }
}

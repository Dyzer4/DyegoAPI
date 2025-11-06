using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IUserPermissionRepository
    {
        Task<IEnumerable<UserPermission>> GetByUserId(int id);
    }
}

using Microsoft.EntityFrameworkCore;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Repositories.Context;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class UserPermissionRepository(CIPAContext dbContext) : IUserPermissionRepository
    {
        public async Task<IEnumerable<UserPermission>> GetByUserId(int id)
        {
            var result = await (from usuario in dbContext.Users
                                join perfil in dbContext.Profiles on usuario.ProfileId equals perfil.Id
                                join perfilacao in dbContext.ProfileActions on perfil.Id equals perfilacao.ProfileId
                                where usuario.Id == id
                                select new { usuario, perfil,perfilacao }).ToListAsync();

            return result.Select(x => new UserPermission
            {
                UserId = x.usuario.Id,
                ProfileId = x.perfil.Id,
                ActionId = x.perfilacao.ActionId
            });

            
        }

    }
}

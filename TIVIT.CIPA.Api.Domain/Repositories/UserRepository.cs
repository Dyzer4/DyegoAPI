using Azure.Core;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Results;
using TIVIT.CIPA.Api.Domain.Repositories.Context;
using TIVIT.CIPA.Api.Domain.Settings;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CIPAContext _dbContext;
        private readonly IUserInfo _userInfo;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            CIPAContext dbContext,
            IUserInfo userInfo,
            ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _userInfo = userInfo;
            _logger = logger;
        }

        public async Task<User> GetFirstOrDefaultAsync(Expression<Func<User, bool>> filter)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<User>> GetByFilterAsync(Expression<Func<User, bool>> filter)
        {
            return await _dbContext.Users.Where(filter).ToListAsync();
        }

        public async Task<User> GetByNetworkUserAsync(string networkUser)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => (u.Email.ToUpper() == networkUser.ToUpper() || u.Email.ToUpper() == networkUser.ToUpper()) && u.IsActive);
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PermissionResult> GetPermissionResult(int userId)
        {
            var result = await
                    (from
                        user in _dbContext.Users
                     join
                        profile in _dbContext.Profiles on user.ProfileId equals profile.Id
                     join
                        profileAction in _dbContext.ProfileActions on profile.Id equals profileAction.ProfileId
                     join
                        action in _dbContext.Actions on profileAction.ActionId equals action.Id into a
                     from
                        action in a.DefaultIfEmpty()
                     where
                        user.Id == userId
                     select new { user, profile, action }).ToListAsync();

            if (result.Count == 0) return null;

            var first = result.First();
            var actions = first.action != null ? result.Select(x => x.action) : Enumerable.Empty<Action>();

            return PermissionResult.Create(first.user, first.profile, actions);
        }

        public async Task<IEnumerable<User>> GetByCompanyId(int conpanyId)
        {
            return await _dbContext.Users.ToArrayAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

    }
}

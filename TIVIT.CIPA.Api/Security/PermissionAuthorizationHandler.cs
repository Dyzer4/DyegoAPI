using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TIVIT.CIPA.Api.Security
{
    public class PermissionAuthorizationHandler : AttributeAuthorizationHandler<PermissionAuthorizationRequirement, PermissionAttribute>
    {
        //readonly IUserPermissionBusiness _userPermissionBusiness;

        //public PermissionAuthorizationHandler(IUserPermissionBusiness userPermissionBusiness)
        //{
        //    _userPermissionBusiness = userPermissionBusiness;
        //}

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement, IEnumerable<PermissionAttribute> attributes)
        {
            foreach (var permissionAttribute in attributes)
            {
                if (!await Authorize(context.User, permissionAttribute.Name))
                {
                    return;
                }
            }

            context.Succeed(requirement);
        }

        private async Task<bool> Authorize(ClaimsPrincipal user, string permission) // TODO: permission temporary disabled. 
        {
            return await Task.FromResult(true);

            //var hasPermission = await _userPermissionBusiness.CheckCurrentUser(permission);

            //if (!hasPermission.HasErrors && hasPermission.Data)
            //{
            //    return true;
            //}

            //return false;
        }
    }
}

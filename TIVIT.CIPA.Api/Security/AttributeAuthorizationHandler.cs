using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace TIVIT.CIPA.Api.Security
{
    public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement where TAttribute : Attribute
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var attributes = new List<TAttribute>();

            if (context.Resource is HttpContext httpContext)
            {
                var endPoint = httpContext.GetEndpoint();

                var action = endPoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

                if (action != null)
                {
                    attributes.AddRange(GetAttributes(action.ControllerTypeInfo.UnderlyingSystemType));
                    attributes.AddRange(GetAttributes(action.MethodInfo));
                }
            }

            return HandleRequirementAsync(context, requirement, attributes);
        }

        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, IEnumerable<TAttribute> attributes);

        private static IEnumerable<TAttribute> GetAttributes(MemberInfo memberInfo) => memberInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
    }
}

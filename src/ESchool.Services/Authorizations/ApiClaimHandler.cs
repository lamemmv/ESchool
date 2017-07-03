using System.Security.Claims;
using System.Threading.Tasks;
using ESchool.Services.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ESchool.Services.Authorizations
{
    public sealed class ApiClaimHandler : AuthorizationHandler<ApiClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiClaimRequirement requirement)
        {
            string type = requirement.ClaimName;
            string issuer = "http://localhost:59999";

            Claim claim = context.User.FindFirst(c => c.Type == type && c.Issuer == issuer);

            if (claim != null)
            {
                int permission;

                if (int.TryParse(claim.Value, out permission))
                {
                    if (((Permissions)permission).HasFlag(requirement.PermissionToCheck))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}

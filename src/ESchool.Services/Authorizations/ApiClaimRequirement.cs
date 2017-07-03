using ESchool.Services.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ESchool.Services.Authorizations
{
    public sealed class ApiClaimRequirement : IAuthorizationRequirement
    {
        public ApiClaimRequirement(string claimName, Permissions permissionToCheck)
        {
            ClaimName = claimName;
            PermissionToCheck = permissionToCheck;
        }

        public string ClaimName { get; set; }

        public Permissions PermissionToCheck { get; set; }
    }
}

namespace ESchool.Services.Infrastructure
{
    public static class ApplicationConsts
    {
        // Memory Cache Keys.
        public const string AccountLockoutOnFailureKey = "cache.memory.accountlockoutonfailure";

        public const string DefaultEmailAccountKey = "cache.sliding.defaultemailaccount";

        public const string RolesKey = "cache.sliding.roles";

        // Settings Names.
        public const string AccountLockoutOnFailureSettingName = "account.lockoutonfailure";
    }
}

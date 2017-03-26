namespace ESchool.Services.Infrastructure
{
    public static class ApplicationConsts
    {
        // Memory Cache Keys.
        public const string AccountLockoutOnFailureKey = "cache.memory.accountlockoutonfailure";

        public const string DefaultEmailAccountKey = "cache.memory.defaultemailaccount";

        public const string RolesKey = "cache.memory.roles";

        // Settings Names.
        public const string AccountLockoutOnFailureSettingName = "account.lockoutonfailure";
    }
}

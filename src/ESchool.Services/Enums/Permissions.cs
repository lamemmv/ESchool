using System;

namespace ESchool.Services.Enums
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1 << 0,
        Create = 1 << 1,
        Update = 1 << 2,
        Delete = 1 << 3,
        // Combinations.
        ReadCreate = Read ^ Create,
        ReadUpdate = Read ^ Update,
        ReadDelete = Read ^ Delete,
        Full = Read ^ Create ^ Update ^ Delete
    }
}

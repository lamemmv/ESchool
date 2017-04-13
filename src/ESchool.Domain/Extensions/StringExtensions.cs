namespace ESchool.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string TrimNull(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim();
        }
    }
}

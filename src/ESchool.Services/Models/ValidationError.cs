namespace ESchool.Services.Models
{
    public sealed class ValidationError
    {
        public ValidationError(string source, string message)
        {
            Source = source;
            Message = message;
        }

        public string Source { get; }

        public string Message { get; }
    }
}

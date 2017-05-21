namespace ESchool.Services.Models
{
    public sealed class AppSettings
    {
        public int MemoryCacheInMinutes { get; set; }

        public string ServerUploadFolder { get; set; }
    }
}

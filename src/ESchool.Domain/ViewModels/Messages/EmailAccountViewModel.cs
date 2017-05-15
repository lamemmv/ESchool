using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Messages
{
    public class EmailAccountViewModel
    {
        [Required]
        public string Email { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        [Range(0, ushort.MaxValue)]
        public int Port { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool EnableSsl { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool IsDefaultEmailAccount { get; set; }
    }
}

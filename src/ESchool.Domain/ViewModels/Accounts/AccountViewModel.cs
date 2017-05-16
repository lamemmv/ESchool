using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Accounts
{
    public class AccountViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string[] Roles { get; set; }
    }
}

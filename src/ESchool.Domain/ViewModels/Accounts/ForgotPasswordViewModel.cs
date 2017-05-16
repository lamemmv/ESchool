using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Url { get; set; }
    }
}

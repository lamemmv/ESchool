using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Systems
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}

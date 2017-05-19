namespace ESchool.Admin.ViewModels.Accounts
{
    public class CreateAccountViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string[] Roles { get; set; }
    }
}

namespace ESchool.Admin.ViewModels.Messages
{
    public class EmailAccountViewModel
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool EnableSsl { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool IsDefaultEmailAccount { get; set; }
    }
}

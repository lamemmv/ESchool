namespace ESchool.Domain.Entities.Systems
{
	public class EmailAccount : BaseEntity
    {
		public string Email { get; set; }

		public string Alias { get; set; }

		public string Host { get; set; }

		public int Port { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public bool EnableSsl { get; set; }

		public bool UseDefaultCredentials { get; set; }

		public string FriendlyName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Alias))
				{
					return Email;
				}

				return $"{Alias} ({Email})";
			}
		}
	}
}

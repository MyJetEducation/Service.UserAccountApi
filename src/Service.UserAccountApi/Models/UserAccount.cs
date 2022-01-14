using System.ComponentModel.DataAnnotations;

namespace Service.UserAccountApi.Models
{
	public class UserAccount
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public string Gender { get; set; }

		public string Phone { get; set; }

		[Required]
		public string Country { get; set; }
	}
}
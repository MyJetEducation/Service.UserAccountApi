using System.ComponentModel.DataAnnotations;

namespace Service.UserAccountApi.Models
{
	public class ChangeEmailRequest
	{
		[Required]
		public string Email { get; set; }
	}
}
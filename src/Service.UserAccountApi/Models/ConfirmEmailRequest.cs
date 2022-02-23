using System.ComponentModel.DataAnnotations;

namespace Service.UserAccountApi.Models
{
	public class ConfirmEmailRequest
	{
		[Required]
		public string Hash { get; set; }
	}
}
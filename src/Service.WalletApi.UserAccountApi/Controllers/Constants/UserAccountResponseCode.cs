namespace Service.WalletApi.UserAccountApi.Controllers.Constants
{
	public class UserAccountResponseCode
	{
		public const int NotValidEmail = -11;

		public const int EmailAlreadyRegistered = -17;
		
		public const int CantChangeToSameEmail = -18;		

		public const int HashExpired = -19;

		public const int HashAlreadyUsed = -20;
	}
}
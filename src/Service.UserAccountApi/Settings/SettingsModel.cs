using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.UserAccountApi.Settings
{
	public class SettingsModel
	{
		[YamlProperty("UserAccountApi.SeqServiceUrl")]
		public string SeqServiceUrl { get; set; }

		[YamlProperty("UserAccountApi.ZipkinUrl")]
		public string ZipkinUrl { get; set; }

		[YamlProperty("UserAccountApi.ElkLogs")]
		public LogElkSettings ElkLogs { get; set; }

		[YamlProperty("UserAccountApi.JwtAudience")]
		public string JwtAudience { get; set; }

		[YamlProperty("UserAccountApi.UserInfoCrudServiceUrl")]
		public string UserInfoCrudServiceUrl { get; set; }

		[YamlProperty("UserAccountApi.UserProfileServiceUrl")]
		public string UserProfileServiceUrl { get; set; }
	}
}
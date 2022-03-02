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

		[YamlProperty("UserAccountApi.UserAccountServiceUrl")]
		public string UserAccountServiceUrl { get; set; }
	}
}
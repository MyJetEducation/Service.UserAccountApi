using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.WalletApi.UserAccountApi.Settings
{
	public class SettingsModel
	{
		[YamlProperty("UserAccountApi.SeqServiceUrl")]
		public string SeqServiceUrl { get; set; }

		[YamlProperty("UserAccountApi.ZipkinUrl")]
		public string ZipkinUrl { get; set; }

		[YamlProperty("UserAccountApi.ElkLogs")]
		public LogElkSettings ElkLogs { get; set; }

		[YamlProperty("UserAccountApi.EnableApiTrace")]
		public bool EnableApiTrace { get; set; }

		[YamlProperty("UserAccountApi.MyNoSqlReaderHostPort")]
		public string MyNoSqlReaderHostPort { get; set; }

		[YamlProperty("UserAccountApi.AuthMyNoSqlReaderHostPort")]
		public string AuthMyNoSqlReaderHostPort { get; set; }

		[YamlProperty("UserAccountApi.SessionEncryptionKeyId")]
		public string SessionEncryptionKeyId { get; set; }

		[YamlProperty("UserAccountApi.MyNoSqlWriterUrl")]
		public string MyNoSqlWriterUrl { get; set; }

		[YamlProperty("UserAccountApi.UserAccountServiceUrl")]
		public string UserAccountServiceUrl { get; set; }
	}
}
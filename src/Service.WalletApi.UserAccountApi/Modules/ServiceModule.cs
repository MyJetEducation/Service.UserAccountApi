using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.Autofac;
using MyJetWallet.Sdk.RestApiTrace;
using MyJetWallet.Sdk.Service;
using Service.UserAccount.Client;

namespace Service.WalletApi.UserAccountApi.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// second parameter is null because we do not store api keys yet for wallet api
			builder.RegisterEncryptionServiceClient(ApplicationEnvironment.AppName, () => Program.Settings.MyNoSqlWriterUrl);

			builder.RegisterUserAccountClient(Program.Settings.UserAccountServiceUrl, Program.LoggerFactory.CreateLogger(typeof (UserAccountClientFactory)));

			if (Program.Settings.EnableApiTrace)
			{
				builder
					.RegisterInstance(new ApiTraceManager(Program.Settings.ElkLogs, "api-trace",
						Program.LoggerFactory.CreateLogger("ApiTraceManager")))
					.As<IApiTraceManager>()
					.As<IStartable>()
					.AutoActivate()
					.SingleInstance();
			}
		}
	}
}
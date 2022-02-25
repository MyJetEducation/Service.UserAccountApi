using Autofac;
using Microsoft.Extensions.Logging;
using Service.UserAccount.Client;

namespace Service.UserAccountApi.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterUserAccountClient(Program.Settings.UserAccountServiceUrl, Program.LogFactory.CreateLogger(typeof (UserAccountClientFactory)));
		}
	}
}
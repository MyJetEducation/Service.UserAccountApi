using Autofac;
using Microsoft.Extensions.Logging;
using Service.UserAccount.Client;
using Service.UserInfo.Crud.Client;

namespace Service.UserAccountApi.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterUserInfoCrudClient(Program.Settings.UserInfoCrudServiceUrl, Program.LogFactory.CreateLogger(typeof (UserInfoCrudClientFactory)));
			builder.RegisterUserAccountClient(Program.Settings.UserAccountServiceUrl, Program.LogFactory.CreateLogger(typeof (UserAccountClientFactory)));
		}
	}
}
using Autofac;
using Microsoft.Extensions.Logging;
using Service.UserInfo.Crud.Client;
using Service.UserProfile.Client;

namespace Service.UserAccountApi.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterUserInfoCrudClient(Program.Settings.UserInfoCrudServiceUrl, Program.LogFactory.CreateLogger(typeof (UserInfoCrudClientFactory)));
			builder.RegisterUserProfileClient(Program.Settings.UserProfileServiceUrl);
		}
	}
}
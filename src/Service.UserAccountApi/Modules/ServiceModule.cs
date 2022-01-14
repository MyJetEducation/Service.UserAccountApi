using Autofac;
using Service.UserInfo.Crud.Client;
using Service.UserProfile.Client;

namespace Service.UserAccountApi.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterUserInfoCrudClient(Program.Settings.UserInfoCrudServiceUrl);
			builder.RegisterUserProfileClient(Program.Settings.UserProfileServiceUrl);
		}
	}
}
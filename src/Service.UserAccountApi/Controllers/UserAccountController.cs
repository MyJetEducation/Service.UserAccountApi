using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Service.Core.Client.Constants;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.UserAccount.Grpc;
using Service.UserAccount.Grpc.Models;
using Service.UserAccountApi.Mappers;
using Service.UserAccountApi.Models;
using Service.UserInfo.Crud.Grpc;

namespace Service.UserAccountApi.Controllers
{
	[OpenApiTag("UserAccount", Description = "user account")]
	[Route("/api/v1/useraccount")]
	public class UserAccountController : BaseController
	{
		private readonly IGrpcServiceProxy<IUserAccountService> _userAccountService;

		public UserAccountController(IGrpcServiceProxy<IUserInfoService> userInfoService, IGrpcServiceProxy<IUserAccountService> userAccountService) : base(userInfoService) =>
			_userAccountService = userAccountService;

		[HttpPost("get")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<Models.UserAccount>), Description = "Ok")]
		public async ValueTask<IActionResult> GetAccountAsync()
		{
			Guid? userId = await GetUserIdAsync();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			AccountGrpcResponse account = await _userAccountService.Service.GetAccount(new GetAccountGrpcRequest {UserId = userId});

			AccountDataGrpcModel accountData = account?.Data;

			return accountData == null
				? StatusResponse.Error(ResponseCode.NoResponseData)
				: DataResponse<Models.UserAccount>.Ok(accountData.ToModel());
		}

		[HttpPost("put")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (StatusResponse), Description = "Status")]
		public async ValueTask<IActionResult> SaveAccountAsync([FromBody] Models.UserAccount account)
		{
			Guid? userId = await GetUserIdAsync();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userAccountService.TryCall(service => service.SaveAccount(account.ToGrpcModel(userId)));

			return StatusResponse.Result(response);
		}
	}
}
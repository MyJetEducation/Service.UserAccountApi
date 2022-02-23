using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Service.Authorization.Client.Services;
using Service.Core.Client.Constants;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.UserAccount.Grpc;
using Service.UserAccount.Grpc.Models;
using Service.UserAccountApi.Constants;
using Service.UserAccountApi.Mappers;
using Service.UserAccountApi.Models;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.UserAccountApi.Controllers
{
	[Authorize]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "Unauthorized")]
	[OpenApiTag("UserAccount", Description = "user account")]
	[Route("/api/v1/useraccount")]
	public class UserAccountController : ControllerBase
	{
		private readonly IGrpcServiceProxy<IUserAccountService> _userAccountService;
		private readonly IGrpcServiceProxy<IUserInfoService> _userInfoService;

		public UserAccountController(IGrpcServiceProxy<IUserInfoService> userInfoService, IGrpcServiceProxy<IUserAccountService> userAccountService)
		{
			_userInfoService = userInfoService;
			_userAccountService = userAccountService;
		}

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

			return StatusResponse.Result(response?.IsSuccess == true);
		}

		[HttpPost("change-email")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (StatusResponse), Description = "Status")]
		public async ValueTask<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
		{
			if (!UserDataRequestValidator.ValidateLogin(request.Email))
				return StatusResponse.Error(UserAccountResponseCode.NotValidEmail);

			Guid? userId = await GetUserIdAsync();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userAccountService.Service.ChangeEmailRequest(new ChangeEmailRequestGrpcRequest
			{
				UserId = userId,
				Email = request.Email
			});

			return StatusResponse.Result(response?.IsSuccess == true);
		}

		private async ValueTask<Guid?> GetUserIdAsync()
		{
			UserInfoResponse userInfoResponse = await _userInfoService.Service.GetUserInfoByLoginAsync(new UserInfoAuthRequest
			{
				UserName = User.Identity?.Name
			});

			return userInfoResponse?.UserInfo?.UserId;
		}
	}
}
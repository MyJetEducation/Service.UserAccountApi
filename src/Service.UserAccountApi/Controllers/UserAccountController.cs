using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Service.Core.Domain.Models.Constants;
using Service.Core.Grpc.Models;
using Service.UserAccountApi.Mappers;
using Service.UserAccountApi.Models;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserProfile.Grpc;
using Service.UserProfile.Grpc.Models;

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
		private readonly IUserProfileService _userProfileService;
		private readonly IUserInfoService _userInfoService;

		public UserAccountController(IUserInfoService userInfoService, IUserProfileService userProfileService)
		{
			_userInfoService = userInfoService;
			_userProfileService = userProfileService;
		}

		[HttpPost("get")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<UserAccount>), Description = "Ok")]
		public async ValueTask<IActionResult> GetAccountAsync()
		{
			Guid? userId = await GetUserIdAsync();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			AccountGrpcResponse account = await _userProfileService.GetAccount(new GetAccountGrpcRequest {UserId = userId});

			AccountDataGrpcModel accountData = account?.Data;

			return accountData == null
				? StatusResponse.Error(ResponseCode.NoResponseData)
				: DataResponse<UserAccount>.Ok(accountData.ToModel());
		}

		[HttpPost("put")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (StatusResponse), Description = "Status")]
		public async ValueTask<IActionResult> SaveAccountAsync([FromBody] UserAccount account)
		{
			Guid? userId = await GetUserIdAsync();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userProfileService.SaveAccount(account.ToGrpcModel(userId));

			return response?.IsSuccess == true ? StatusResponse.Ok() : StatusResponse.Error();
		}

		private async ValueTask<Guid?> GetUserIdAsync()
		{
			UserInfoResponse userInfoResponse = await _userInfoService.GetUserInfoByLoginAsync(new UserInfoAuthRequest
			{
				UserName = User.Identity?.Name
			});

			return userInfoResponse?.UserInfo?.UserId;
		}
	}
}
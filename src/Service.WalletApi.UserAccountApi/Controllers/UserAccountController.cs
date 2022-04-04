using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyJetWallet.Sdk.Authorization.Http;
using NSwag.Annotations;
using Service.Core.Client.Extensions;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.UserAccount.Grpc;
using Service.UserAccount.Grpc.Models;
using Service.WalletApi.UserAccountApi.Mappers;
using Service.Web;

namespace Service.WalletApi.UserAccountApi.Controllers
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

		public UserAccountController(IGrpcServiceProxy<IUserAccountService> userAccountService) => _userAccountService = userAccountService;

		[HttpPost("get")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<Contracts.UserAccount>), Description = "Ok")]
		public async ValueTask<IActionResult> GetAccountAsync()
		{
			Guid? userId = GetUserId();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			AccountGrpcResponse account = await _userAccountService.Service.GetAccount(new GetAccountGrpcRequest {UserId = userId});

			AccountDataGrpcModel accountData = account?.Data;

			return accountData == null
				? StatusResponse.Error(ResponseCode.NoResponseData)
				: DataResponse<Contracts.UserAccount>.Ok(accountData.ToModel());
		}

		[HttpPost("put")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (StatusResponse), Description = "Status")]
		public async ValueTask<IActionResult> SaveAccountAsync([FromBody] Contracts.UserAccount account)
		{
			Guid? userId = GetUserId();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userAccountService.TryCall(service => service.SaveAccount(account.ToGrpcModel(userId)));

			return StatusResponse.Result(response);
		}

		private Guid? GetUserId()
		{
			string clientId = this.GetClientId();
			if (clientId.IsNullOrWhiteSpace())
				return null;

			return Guid.TryParse(clientId, out Guid uid) ? (Guid?) uid : null;
		}
	}
}
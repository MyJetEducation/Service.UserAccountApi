using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Service.Authorization.Client.Services;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.UserAccount.Grpc;
using Service.UserAccount.Grpc.Models;
using Service.UserAccountApi.Constants;
using Service.UserAccountApi.Models;
using Service.Web;

namespace Service.UserAccountApi.Controllers
{
	[OpenApiTag("ChangeEmail", Description = "change user email")]
	[Route("/api/v1/useraccount/email")]
	public class ChangeEmailController : BaseController
	{
		private readonly IGrpcServiceProxy<IUserAccountService> _userAccountService;

		public ChangeEmailController(IGrpcServiceProxy<IUserAccountService> userAccountService) => _userAccountService = userAccountService;

		[HttpPost("change")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (StatusResponse), Description = "Status")]
		public async ValueTask<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
		{
			if (!UserDataRequestValidator.ValidateLogin(request.Email))
				return StatusResponse.Error(UserAccountResponseCode.NotValidEmail);

			Guid? userId = GetUserId();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userAccountService.Service.ChangeEmailRequest(new ChangeEmailRequestGrpcRequest
			{
				UserId = userId,
				Email = request.Email
			});

			return StatusResponse.Result(response);
		}

		[HttpPost("confirm")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (StatusResponse), Description = "Status")]
		public async ValueTask<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
		{
			Guid? userId = GetUserId();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userAccountService.Service.ChangeEmailConfirm(new ChangeEmailConfirmGrpcRequest
			{
				Hash = request.Hash
			});

			return StatusResponse.Result(response);
		}
	}
}
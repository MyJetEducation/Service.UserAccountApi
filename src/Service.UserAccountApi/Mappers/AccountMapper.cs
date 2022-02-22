using System;
using Service.UserAccount.Grpc.Models;

namespace Service.UserAccountApi.Mappers
{
	public static class AccountMapper
	{
		public static Models.UserAccount ToModel(this AccountDataGrpcModel grpcModel) => new Models.UserAccount
		{
			FirstName = grpcModel.FirstName,
			LastName = grpcModel.LastName,
			Gender = grpcModel.Gender,
			Phone = grpcModel.Phone,
			Country = grpcModel.Country
		};

		public static SaveAccountGrpcRequest ToGrpcModel(this Models.UserAccount model, Guid? userId) => new SaveAccountGrpcRequest
		{
			UserId = userId,
			FirstName = model.FirstName,
			LastName = model.LastName,
			Gender = model.Gender,
			Phone = model.Phone,
			Country = model.Country
		};
	}
}
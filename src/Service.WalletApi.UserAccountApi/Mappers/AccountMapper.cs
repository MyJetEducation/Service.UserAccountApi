using System;
using Service.UserAccount.Grpc.Models;

namespace Service.WalletApi.UserAccountApi.Mappers
{
	public static class AccountMapper
	{
		public static Controllers.Contracts.UserAccount ToModel(this AccountDataGrpcModel grpcModel) => new Controllers.Contracts.UserAccount
		{
			FirstName = grpcModel.FirstName,
			LastName = grpcModel.LastName,
			Gender = grpcModel.Gender,
			Phone = grpcModel.Phone,
			Country = grpcModel.Country
		};

		public static SaveAccountGrpcRequest ToGrpcModel(this Controllers.Contracts.UserAccount model, Guid? userId) => new SaveAccountGrpcRequest
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
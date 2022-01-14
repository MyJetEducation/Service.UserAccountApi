using System;
using Service.UserAccountApi.Models;
using Service.UserProfile.Grpc.Models;

namespace Service.UserAccountApi.Mappers
{
	public static class AccountMapper
	{
		public static UserAccount ToModel(this AccountDataGrpcModel grpcModel) => new UserAccount
		{
			FirstName = grpcModel.FirstName,
			LastName = grpcModel.LastName,
			Gender = grpcModel.Gender,
			Phone = grpcModel.Phone,
			Country = grpcModel.Country
		};

		public static SaveAccountGrpcRequest ToGrpcModel(this UserAccount model, Guid? userId) => new SaveAccountGrpcRequest
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
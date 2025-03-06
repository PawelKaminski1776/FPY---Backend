using System;
using InspectionBackend.Contracts.InspectionDtos;
using InspectionBackend.Contracts.UserDtos;
using InspectionBackend.UserDetails.Controllers.DtoFactory;

public class DtoFactory : IDtoFactory
{
    public object CreateDto(string dtoType, params object[] args)
    {
        if (args == null || args.Length == 0)
            throw new ArgumentException("Arguments cannot be null or empty.");

        switch (dtoType.ToLower())
        {
            case "inspectiondto":
                if (args.Length < 2 || !(args[0] is string) || !(args[1] is string[]))
                    throw new ArgumentException("Invalid arguments for InspectionRequest.");

                return new InspectionRequest
                {
                    InspectionName = (string)args[0],
                    InspectionVariables = (string[])args[1]
                };

            case "inspectionimagesdto":
                if (args.Length < 2 || !(args[0] is string) || !(args[1] is string[]))
                    throw new ArgumentException("Invalid arguments for InspectionImageRequest.");

                return new InspectionImageRequest
                {
                    InspectionName = (string)args[0],
                    NumOfImages = (int)args[1],
                    County = (string)args[2]
                };

            case "logindto":
                if (args.Length < 2 || !(args[0] is string) || !(args[1] is string))
                    throw new ArgumentException("Invalid arguments for LoginRequest.");

                return new LoginRequest
                {
                    Email = (string)args[0],
                    Password = (string)args[1]
                };

            default:
                throw new ArgumentException($"Invalid DTO type: {dtoType}");
        }
    }

    public object UseDto(string dtoType, object dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO object cannot be null.");

        switch (dtoType.ToLower())
        {
            case "usercreationdto":
            case "inspectiondto":
            case "inspectionimagesdto":
                return dto;
            default:
                throw new ArgumentException($"Invalid DTO type: {dtoType}");
        }
    }
}

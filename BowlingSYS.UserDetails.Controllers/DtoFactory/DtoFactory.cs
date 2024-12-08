using BowlingSys.Contracts.UserDtos;
using BowlingSYS.UserDetails.Controllers.DtoFactory;


public class DtoFactory : IDtoFactory
{
    public object CreateDto(string dtoType, params object[] args)
    {
        switch (dtoType.ToLower())
        {
            case "userexistsdto":
                return new UserExistsDto
                {
                    Username = args[0] as string,
                    Email = args[1] as string
                };
            case "logindto":
                if (args[0] != null)
                {
                    return new LoginDto
                    {
                        Username = args[0] as string,
                        Password = args[2] as string
                    };
                }
                else
                {
                    return new LoginDto
                    {
                        Email = args[1] as string,
                        Password = args[2] as string
                    };
                }
            default:
                throw new ArgumentException("Invalid DTO type.");
        }
    }

    public object UseDto(string dtoType, object dto)
    {
        switch (dtoType.ToLower())
        {
            case "usercreationdto":
            return dto;

        default:
            throw new ArgumentException("Invalid DTO type.");
        }
    }
}

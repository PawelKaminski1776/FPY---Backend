using NServiceBus;

namespace InspectionBackend.Contracts.UserDtos
{
    public class LoginRequest : IMessage
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class UserCreationRequest: IMessage
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }

    public class UserCreationResponse: IMessage
    {
        public bool Exists { get; set; }

    }


}

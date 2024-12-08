using NServiceBus;

namespace BowlingSys.Contracts.UserDtos
{
    public class LoginDto : IMessage
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class UserExistsDto : IMessage
    {
        public string Username { get; set; }
        public string Email { get; set; }

    }


    public class UserCreationDto : IMessage
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}

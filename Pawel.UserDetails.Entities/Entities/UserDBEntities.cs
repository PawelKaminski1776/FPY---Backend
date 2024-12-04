using NServiceBus;
namespace BowlingSys.Entities.UserDBEntities
{
    public class GetUserIDResult : IMessage
    {
        public Guid User_Id { get; set; }
    }

    public class GetLoginResult : IMessage
    {
        public bool Active { get; set; }
    }

    public class ErrorMessage : IMessage
    {
        public string message { get; set; }
    }
}

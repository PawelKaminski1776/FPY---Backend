using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System;
using MongoDB.Services.UserService;

namespace InspectionBackend.Handlers
{
    public class AddUserHandler : IHandleMessages<UserCreationRequest>
    {
        private readonly UserService userService;
        public AddUserHandler(UserService _userService)
        {
            this.userService = _userService;
        }

        public async Task Handle(UserCreationRequest message, IMessageHandlerContext context)
        {
            try
            {
                var userexists = await userService.CheckIfUserExists(message);

                if (userexists == "User Not Found")
                {
                    var response = await userService.InsertUser(message);

                    await context.Reply(new UserCreationResponse { message = response });
                }
                else
                {
                    await context.Reply(new UserCreationResponse { message = userexists });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");

                throw;
            }
        }
    }
}

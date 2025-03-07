using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System;
using MongoDB.Services.UserService;

namespace InspectionBackend.Handlers
{
    public class LoginHandler : IHandleMessages<LoginRequest>
    {
        private readonly UserService userService;
        public LoginHandler(UserService _userService)
        {
            this.userService = _userService;
        }

        public async Task Handle(LoginRequest message, IMessageHandlerContext context)
        {
            try
            {
                var userexists = await userService.CheckIfLogin(message);

                if(userexists == "User Exists")
                {
                    await context.Reply(new LoginResponse { Email = message.Email });
                }
                if(userexists == "User Not Found")
                {
                    await context.Reply(new LoginResponse { Email = userexists });
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

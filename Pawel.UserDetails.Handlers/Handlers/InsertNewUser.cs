using NServiceBus;
using BowlingSys.Contracts.UserDtos;
using BowlingSys.Services.UserService;
using BowlingSys.Entities.UserDBEntities;
using System;

namespace BowlingSys.Handlers.Handlers
{
    public class InsertUserHandler : IHandleMessages<UserCreationDto>
    {
        private readonly UserService _userService;

        public InsertUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(UserCreationDto message, IMessageHandlerContext context)
        {
            try
            {
                var Result = new ErrorMessage { message = "" };

                Result = await _userService.CallAddNewUser_SP(message);

                await context.Reply(Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");

                throw;
            }
        }


    }
}

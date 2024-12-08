using NServiceBus;
using BowlingSys.Contracts.UserDtos;
using BowlingSys.DBConnect;
using BowlingSys.Entities.UserDBEntities;
using System;

namespace BowlingSys.Handlers.Handlers
{
    public class UserExistsHandler : IHandleMessages<UserExistsDto>
    {
        private readonly UserService _userService;

        public UserExistsHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(UserExistsDto message, IMessageHandlerContext context)
        {
            try
            {
                var result = new GetLoginResult { Active = false };

                if (!string.IsNullOrEmpty(message.Username) || !string.IsNullOrEmpty(message.Email))
                {
                    var input = !string.IsNullOrEmpty(message.Username) ? message.Username : message.Email;
                    result = await _userService.CallCheckUserExists_SP(input);
                }
                else
                {
                    var error = new ErrorMessage
                    {
                        message = "Username or Email must be provided"
                    };
                    await context.Reply(error);
                    return;
                }

                if (result.Active)
                {
                    var error = new ErrorMessage
                    {
                        message = "Username or Email already exists"
                    };
                    await context.Reply(error);
                }
                else
                {
                    var success = new ErrorMessage
                    {
                        message = "Username or Email does not exist"
                    };
                    await context.Reply(success);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }


    }
}



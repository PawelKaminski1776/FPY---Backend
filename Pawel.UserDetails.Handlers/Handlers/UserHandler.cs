using NServiceBus;
using BowlingSys.Contracts.UserDtos;
using BowlingSys.Services.UserService;
using BowlingSys.Entities.UserDBEntities;
using System;

namespace BowlingSys.Handlers.Handlers
{
    public class MyMessageHandler : IHandleMessages<LoginDto>
    {
        private readonly UserService _userService;

        public MyMessageHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task Handle(LoginDto message, IMessageHandlerContext context)
        {
  
        var loginResult = new GetUserIDResult { User_Id = Guid.Empty };

        if (!string.IsNullOrEmpty(message.Username))
        {
            loginResult = await _userService.CallCheckUserLogin_SP(message.Username, message.Password);
        }
        else if (!string.IsNullOrEmpty(message.Email))
        {
            loginResult = await _userService.CallCheckUserLogin_SP(message.Email, message.Password);
        }

         await context.Reply(loginResult);
      
        }

       


    }
}

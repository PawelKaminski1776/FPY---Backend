using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System;

namespace InspectionBackend.Handlers
{
    public class MyMessageHandler : IHandleMessages<UserCreationRequest>
    {
        public MyMessageHandler()
        {
        }

        public async Task Handle(UserCreationRequest message, IMessageHandlerContext context)
        {
            try
            {
                await context.Reply("Hello");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");

                throw;
            }
        }
    }
}

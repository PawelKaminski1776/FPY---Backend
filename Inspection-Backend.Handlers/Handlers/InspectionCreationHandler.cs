using System.Reflection;
using InspectionBackend.Contracts.InspectionDtos;
using MongoDB.Helpers;
using MongoDB.Services;

namespace InspectionBackend.Handlers
{
    public class InspectionCreationHandler : IHandleMessages<InspectionImageRequest>
    {
        private readonly MongoConnect mongoConnect;
        public InspectionCreationHandler(MongoConnect mongoConnect)
        {
            this.mongoConnect = mongoConnect;
        }

        public async Task Handle(InspectionImageRequest message, IMessageHandlerContext context)
        {
            try
            {
                InspectionImageResponse response = new InspectionImageResponse();



                response.InspectionName = message.InspectionName;

                await context.Reply(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");

                throw;
            }
        }
    }
}

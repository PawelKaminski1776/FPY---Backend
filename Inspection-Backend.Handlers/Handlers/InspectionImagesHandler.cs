using InspectionBackend.Contracts.InspectionDtos;
using MongoDB.Helpers;
using MongoDB.Services;

namespace InspectionBackend.Handlers
{
    public class InspectionImagesHandler : IHandleMessages<InspectionImageRequest>
    {
        private readonly MongoConnect mongoConnect;
        private readonly ImageTrainingAPI imageTrainingAPI;
        public InspectionImagesHandler(MongoConnect mongoconnect, ImageTrainingAPI imagetrainingAPI)
        {
            this.mongoConnect = mongoconnect;
            this.imageTrainingAPI = imagetrainingAPI;
            
        }

        public async Task Handle(InspectionImageRequest message, IMessageHandlerContext context)
        {
            if (message == null)
            {
                Console.WriteLine("Error: The message is null.");
                // You might want to handle the failure case here more gracefully
                throw new ArgumentNullException("message", "The incoming message is null.");
            }
            try
            {
                InspectionImageResponse response = new InspectionImageResponse();

                response = await imageTrainingAPI.SendToImageTrainingAPI("/Webscraper", message);

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

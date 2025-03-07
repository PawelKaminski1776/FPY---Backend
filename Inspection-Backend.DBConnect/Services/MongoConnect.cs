using System;
using InspectionBackend.Contracts.InspectionDtos;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MongoDB.Services
{
    public class MongoConnect
    {
        protected readonly MongoClient dbClient;
        public MongoConnect(string ConnectionString)
        {
            dbClient = new MongoClient(ConnectionString);

        }

        public BsonArray SetUpImages(InspectionImage[] content)
        {

            BsonArray array = new BsonArray();

            foreach (InspectionImage image in content)
            {
                array.Add(new BsonDocument { { "imageurl", image.ImgUrl } });
            }

            return array;
        }

        public BsonArray SetUpDocument(string[] content)
        {

            BsonArray array = new BsonArray();

            foreach (string name in content)
            {
                array.Add(new BsonDocument { { "InspectionName", name } });
            }

            return array;
        }



        public async Task<string> SendToMongo(string[] content, string name)
        {
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("Inspections");

            var document = new BsonDocument { { "inspection_name",  name}, { "scores", SetUpDocument(content) } };

            try
            {
                await collection.InsertOneAsync(document);
                return "Request Successful";
            }
            catch(Exception e)
            {
                return "Failed" + e.Message;
            }

        }

        public async Task<string> SendImagesToMongo(InspectionImageResponse request)
        {
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("Inspection_images");
            
            var document = new BsonDocument { { "inspection_name", request.InspectionName }, { "images", SetUpImages(request.Images) } };

            try
            {
                await collection.InsertOneAsync(document);
                return "Request Successful";
            }
            catch (Exception e)
            {
                return "Failed" + e.Message;
            }

        }

        public async Task<List<string>> GetImagesFromMongo(string Inspection_Type)
        {
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("Inspection_images");

            var filter = Builders<BsonDocument>.Filter.Eq("inspection_name", Inspection_Type);

            try
            {
                var result = await collection.Find(filter).ToListAsync();

                var imageUrls = result.Select(doc => doc["image_url"].AsString).ToList();

                return imageUrls;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching images: {e.Message}");
                return new List<string> { "Error: " + e.Message };
            }
        }

    }
}
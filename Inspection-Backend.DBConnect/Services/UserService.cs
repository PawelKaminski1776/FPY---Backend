using InspectionBackend.Contracts.InspectionDtos;
using InspectionBackend.Contracts.UserDtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.Services.UserService
{
    public class UserService : MongoConnect
    {

        public UserService(string ConnectionString) : base(ConnectionString)
        {
        }

        public async Task<string> InsertUser(UserCreationRequest request)
        {
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("Users");

            var document = new BsonDocument { 
                { "forename", request.Forename },
                { "surname", request.Surname },
                { "company", request.Company },
                { "email", request.Email },
                // Will Need Encryption Later
                { "password", request.Password }
            };

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

        public async Task<string> CheckIfUserExists(UserCreationRequest request)
        {
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("Users");

            var filter = Builders<BsonDocument>.Filter.Eq("email", request.Email);

            try
            {
                var userExists = await collection.Find(filter).AnyAsync();

                Console.WriteLine($"User Exists: {userExists}");

                return userExists ? "User Exists" : "User Not Found";
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Checking For User: {e.Message}");
                return $"Error: {e.Message}";
            }
        }

        public async Task<string> CheckIfLogin(LoginRequest request)
        {
            Console.WriteLine(request.Email + request.Password);
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("Users");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("email", request.Email),
                Builders<BsonDocument>.Filter.Eq("password", request.Password)
            );


            try
            {
                var userExists = await collection.Find(filter).AnyAsync();

                Console.WriteLine($"User Exists: {userExists}");

                return userExists ? "User Exists" : "User Not Found";
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Checking For User: {e.Message}");
                return $"Error: {e.Message}";
            }
        }




    }
}
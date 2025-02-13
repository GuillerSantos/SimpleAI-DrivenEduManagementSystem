using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SimpleAI_DrivenEduManagementSystem.Server.Services
{
    public class DatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => _database;
    }
}
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MeetingManagement.Persistance.Context
{
    internal class MongoDbContext : IMongoDbContext
    {
        private IMongoDatabase _database { get; set; }
        private MongoClient _client { get; set; }
        public MongoDbContext(IOptions<MongoDbSettings> connectionSettings)
        {
            _client = new MongoClient(connectionSettings.Value.ConnectionString);
            _database = _client.GetDatabase(connectionSettings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}

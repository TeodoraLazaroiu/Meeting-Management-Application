using MongoDB.Driver;

namespace MeetingManagement.Persistence.Context
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}

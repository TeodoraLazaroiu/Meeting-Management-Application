using MongoDB.Driver;

namespace MeetingManagement.Persistance.Context
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}

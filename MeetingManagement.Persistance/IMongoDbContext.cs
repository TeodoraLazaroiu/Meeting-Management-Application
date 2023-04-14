using MongoDB.Driver;

namespace MeetingManagement.Persistance
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}

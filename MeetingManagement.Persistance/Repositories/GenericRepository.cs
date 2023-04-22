using MeetingManagement.Core.Common;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;
using MongoDB.Driver;

namespace MeetingManagement.Persistance.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IMongoDbContext _dbContext;
        protected IMongoCollection<T> _dbCollection;
        public GenericRepository(IMongoDbContext context)
        {
            _dbContext = context;
            _dbCollection = _dbContext.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var filter = Builders<T>.Filter.Empty;
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, new Guid(id));
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
            await _dbCollection.FindOneAndReplaceAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, new Guid(id));
            await _dbCollection.FindOneAndDeleteAsync(filter);
        }

    }
}

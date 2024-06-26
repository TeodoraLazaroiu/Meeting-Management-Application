﻿using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistence.Context;
using MongoDB.Driver;

namespace MeetingManagement.Persistence.Repositories
{
    internal class ResponseRepository : GenericRepository<ResponseEntity>, IResponseRepository
    {
        public ResponseRepository(IMongoDbContext context) : base(context) { }

        public async Task<ResponseEntity?> GetResponseByUserAndEvent(string userId, string eventId)
        {
            var filter = Builders<ResponseEntity>.Filter.Eq(x => x.UserId, new Guid(userId));
            filter &= Builders<ResponseEntity>.Filter.Eq(x => x.EventId, new Guid(eventId));
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<List<ResponseEntity>> GetResponsesByUser(string userId)
        {
            var filter = Builders<ResponseEntity>.Filter.Eq(x => x.UserId, new Guid(userId));
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task<List<ResponseEntity>> GetResponsesByEvent(string eventId)
        {
            var filter = Builders<ResponseEntity>.Filter.Eq(x => x.EventId, new Guid(eventId));
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task DeleteResponsesByEvent(string eventId)
        {
            var filter = Builders<ResponseEntity>.Filter.Eq(x => x.EventId, new Guid(eventId));
            await _dbCollection.DeleteManyAsync(filter);
        }
    }
}

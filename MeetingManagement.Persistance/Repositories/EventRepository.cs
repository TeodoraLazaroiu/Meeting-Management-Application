﻿using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;
using MongoDB.Driver;

namespace MeetingManagement.Persistance.Repositories
{
    internal class EventRepository : GenericRepository<EventEntity>, IEventRepository
    {
        public EventRepository(IMongoDbContext context) : base(context) { }

        public async Task<List<EventEntity>> GetEventsByUserId(string userId)
        {
            var filter = Builders<EventEntity>.Filter.AnyEq(x => x.Attendes, new Guid(userId));
            return await _dbCollection.Find(filter).ToListAsync();
        }
    }
}

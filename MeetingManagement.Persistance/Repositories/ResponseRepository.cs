using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;

namespace MeetingManagement.Persistance.Repositories
{
    internal class ResponseRepository : GenericRepository<ResponseEntity>, IResponseRepository
    {
        public ResponseRepository(IMongoDbContext context) : base(context) { }
    }
}

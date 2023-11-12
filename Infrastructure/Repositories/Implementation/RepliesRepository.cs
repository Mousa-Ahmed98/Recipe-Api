using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class RepliesRepository : BaseRepository<Reply>, IRepliesRepository
    {
        public RepliesRepository(StoreContext context) : base(context)
        {
        }
    }
}

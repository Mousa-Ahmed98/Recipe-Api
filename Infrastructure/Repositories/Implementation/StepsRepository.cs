using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class StepsRepository : BaseRepository<Step>, IStepsRepository
    {
        public StepsRepository(StoreContext context) : base(context)
        {
        }
    }
}

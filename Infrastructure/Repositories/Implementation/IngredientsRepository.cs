using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class IngredientsRepository : BaseRepository<Ingredient>, IIngredientsRepository
    {
        public IngredientsRepository(StoreContext context) : base(context)
        {
        }
    }
}

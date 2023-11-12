using System;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories.Implementation;

namespace Infrastructure.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly StoreContext _context;

        public UnitOfWork(StoreContext dbContext)
        {
            _context = dbContext;

            CategoriesRepository = new CategoriesRepository(dbContext);
            RecipeRepository = new RecipeRepository(dbContext);
            IngredientsRepository = new IngredientsRepository(dbContext);
            StepsRepository = new StepsRepository(dbContext);
            UsersRepository = new UsersRepository(dbContext);
            NotificationsRepository = new NotificationsRepository(dbContext);
            PlansRepository = new PlansRepository(dbContext);
            FollowsRepoisitory = new FollowsRepository(dbContext);
            RatingRepository = new RatingsRepository(dbContext);
            CommentsRepository = new CommentsRepository(dbContext);
            RepliesRepository = new RepliesRepository(dbContext);
        }

        public ICategoriesRepository CategoriesRepository { get; }
        public IRecipeRepository RecipeRepository { get; }
        public IIngredientsRepository IngredientsRepository { get; }
        public IStepsRepository StepsRepository { get; }
        public IUsersRepository UsersRepository { get; }
        public INotificationsRepository NotificationsRepository { get; }
        public IPlansRepository PlansRepository { get; }
        public IFollowsRepository FollowsRepoisitory { get; }
        public IRatingsRepository RatingRepository { get; }
        public ICommentsRepository CommentsRepository { get; }
        public IRepliesRepository RepliesRepository { get; }

        public async Task<bool> SaveAsync() 
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
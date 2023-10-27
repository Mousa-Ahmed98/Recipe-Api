using Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoriesRepository CategoriesRepository { get; }
        IRecipeRepository RecipeRepository { get; }
        IIngredientsRepository IngredientsRepository { get; }
        IStepsRepository StepsRepository { get; }
        IUsersRepository UsersRepository { get; }
        INotificationsRepository NotificationsRepository { get; }
        IPlansRepository PlansRepository { get; }
        IFollowsRepository FollowsRepoisitory { get; }
        IRatingsRepository RatingRepository { get; }

        Task<bool> SaveAsync();
    }
}

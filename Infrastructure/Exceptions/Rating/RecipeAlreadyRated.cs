namespace Infrastructure.Exceptions.User
{
    public class RecipeAlreadyRated : BadRequestException
    {
        public RecipeAlreadyRated(int id)
            : base($"Recipe with id {id} is already rated.")
        {
        }
    }
}

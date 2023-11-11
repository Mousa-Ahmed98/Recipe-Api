namespace Infrastructure.Exceptions.User
{
    public class RatingNotFoundException : NotFoundException
    {
        public RatingNotFoundException(int recipeId)
            : base($"No Rating was found for recipe with id = {recipeId}.")
        {
        }
    }
}

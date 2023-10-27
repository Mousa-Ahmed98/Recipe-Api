namespace Infrastructure.Exceptions.Recipe
{
    public class AlreadyInFavouritesException : BadRequestException
    {
        public AlreadyInFavouritesException(int recipeId) 
            : base($"Recipe with id {recipeId} is already in favourites.")
        {
        }
    }
}

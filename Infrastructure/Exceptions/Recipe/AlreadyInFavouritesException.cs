namespace Infrastructure.Exceptions.Recipe
{
    internal class AlreadyInFavouritesException : BadRequestException
    {
        public AlreadyInFavouritesException(int recipeId) 
            : base($"Recipe with id {recipeId} is already in favourites.")
        {
        }
    }
}

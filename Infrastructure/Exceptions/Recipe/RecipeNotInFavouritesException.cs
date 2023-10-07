namespace Infrastructure.Exceptions.Recipe
{
    internal class RecipeNotInFavouritesException : BadRequestException
    {
        public RecipeNotInFavouritesException(int recipeId) 
            : base($"Recipe with id {recipeId} is not in favourites.")
        {
        }
    }
}

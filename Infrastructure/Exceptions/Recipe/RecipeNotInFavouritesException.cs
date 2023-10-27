namespace Infrastructure.Exceptions.Recipe
{
    public class RecipeNotInFavouritesException : BadRequestException
    {
        public RecipeNotInFavouritesException(int recipeId) 
            : base($"Recipe with id {recipeId} is not in favourites.")
        {
        }
    }
}

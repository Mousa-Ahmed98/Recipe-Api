namespace Infrastructure.Exceptions.Recipe
{
    public class RecipeNotFoundException : NotFoundException
    {
        public RecipeNotFoundException(int id)
            : base($"Recipe with {id} was not found.")
        {

        }
    }
}

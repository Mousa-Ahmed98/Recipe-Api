
namespace Core.Entities
{
    public class Step
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}

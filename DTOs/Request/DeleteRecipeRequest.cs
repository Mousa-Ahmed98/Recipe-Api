using Microsoft.IdentityModel.Tokens;

namespace Recipe.DTOs.Request
{
    public class DeleteRequest
{
    public bool DeleteWholeRecipe { get; set; }
    public List<int> StepIds { get; set; }
    public List<int> IngredientIds { get; set; }
}
}
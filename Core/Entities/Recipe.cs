using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public bool InFavourites { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public  ICollection<Step> Steps { get; set; }
        public ICollection<Plan> Plans { get; set; }
        
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}

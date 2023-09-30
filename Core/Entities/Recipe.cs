using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public bool InFavourites { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public  ICollection<Step> Steps { get; set; }
        public  ICollection<Plan> Plans { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}

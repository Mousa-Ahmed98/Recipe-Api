using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Image { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public required ICollection<Ingredient> Ingredients { get; set; }
        public required ICollection<Step> Steps { get; set; }
    }
}

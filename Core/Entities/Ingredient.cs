using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        public required string Description { get; set; }

        public int RecipeId { get; set; }
    }
}

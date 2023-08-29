using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }

        public ICollection<Ingredient> ingredients { get; set; }
        public ICollection<Step> steps { get; set; }
    }
}

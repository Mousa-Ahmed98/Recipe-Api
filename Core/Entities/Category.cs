using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}

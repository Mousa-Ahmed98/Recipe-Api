using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public byte rate { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public int RecipeId { get; set; }

        public ApplicationUser Author { get; set; }
        public Recipe Recipe { get; set; }
    }
}

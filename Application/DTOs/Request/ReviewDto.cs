using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class ReviewDto
    {
        [Required]
        public byte rate { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public int RecipeId { get; set; }
    }
}

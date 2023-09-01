using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Step
    {
        public int Id { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public byte Order { get; set; }
    }
}

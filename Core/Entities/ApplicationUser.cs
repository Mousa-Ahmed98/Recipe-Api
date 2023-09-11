using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        //public int Id { get; set; }
        [Required]
        public override string UserName { get; set; }
        [Required]
        public override string Email { get; set; }
        
    }
}

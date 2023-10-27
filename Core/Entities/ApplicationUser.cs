using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public bool IsFollowed { get; set; }
        [NotMapped]
        public int Followers { get; set; }
        [NotMapped]
        public int Following { get; set; }
    }
}

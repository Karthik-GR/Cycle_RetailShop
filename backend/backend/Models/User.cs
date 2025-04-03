using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public byte[]? ImageFile { get; set; }

        [Required]    
        public string email { get; set; }

        [Required]  
        public string Username { get; set; }

        [Required]  
        public string Password { get; set; }

        public string Role { get; set; } 

    }
}

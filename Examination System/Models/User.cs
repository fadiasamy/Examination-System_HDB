using System.ComponentModel.DataAnnotations;

namespace Examination_System.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}

using Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Accounts
{
    public class CreateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EnumDataType(typeof(SystemRole))]
        public string SystemRole { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
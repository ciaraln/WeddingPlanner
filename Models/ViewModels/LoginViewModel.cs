using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Email must be valid email format.")]

        public string Email { get; set; }

        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Range(0, 0, ErrorMessage = "Password does not match password on record!")]
        public string PasswordConfirmation { get; set; }

        [Range(0, 0, ErrorMessage = "Email does not exist! Enter correct Email or Register.")]
        public int? Found { get; set; }
        public int? Count { get; internal set; }
    }
}
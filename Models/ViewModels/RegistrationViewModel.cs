using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Display(Name = "First Name: ")]
        [Required(ErrorMessage = "First Name required.")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name: ")]
        [Required(ErrorMessage = "Last Name required.")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters")]
        public string LastName { get; set; }


        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Email must be valid email format.")]
        // [Range(0, 0, ErrorMessage = "Email already exists, please use unique email or login!")]

        public string Email { get; set; }

        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Password is required!")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Passwrod Confirm: ")]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }

        public int IsUnique { get; internal set; }
    }
}
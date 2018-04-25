using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WeddingPlanner.Models.CustomValidations
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (WeddingPlannerContext)validationContext.GetService(typeof(WeddingPlannerContext));
            var allUsers = _context.users;
            foreach (var each in allUsers)
            {
                if ((string)value == (string)each.Email)
                {
                    return new ValidationResult("Email already exists in database");
                }
            }
            return ValidationResult.Success;
        }
    }
}
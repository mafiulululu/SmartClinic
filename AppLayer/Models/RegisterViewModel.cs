using System.ComponentModel.DataAnnotations;

namespace SmartClinic.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = string.Empty;

        // Required only when Role = Patient
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        // Required only when Role = Patient
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateOnly? Dob { get; set; }
    }
}
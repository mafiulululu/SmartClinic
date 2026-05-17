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

        // Patient fields
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateOnly? Dob { get; set; }

        // Doctor fields
        [Display(Name = "Speciality")]
        public string? Speciality { get; set; }

        [Display(Name = "Consultation Fee")]
        [Range(1, 100000, ErrorMessage = "Consultation fee must be greater than 0.")]
        public decimal? ConsultationFee { get; set; }

        [Display(Name = "Available for Appointments")]
        public bool IsAvailable { get; set; } = true;
    }
}
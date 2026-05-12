using System.ComponentModel.DataAnnotations;

namespace SmartClinic.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public string Role { get; set; }
    }
}
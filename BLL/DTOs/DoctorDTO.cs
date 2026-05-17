using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Speciality is required.")]
        [StringLength(50)]
        public string Speciality { get; set; } = string.Empty;

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(1, 100000, ErrorMessage = "Consultation fee must be greater than 0.")]
        public decimal ConsultationFee { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
    }
}
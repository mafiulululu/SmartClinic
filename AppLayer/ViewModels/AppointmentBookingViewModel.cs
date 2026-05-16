using System.ComponentModel.DataAnnotations;

namespace SmartClinic.Web.ViewModels
{
    public class AppointmentBookingViewModel
    {
        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please select appointment date and time.")]
        [Display(Name = "Appointment Date & Time")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please enter symptoms or notes.")]
        [StringLength(500, ErrorMessage = "Symptoms notes cannot exceed 500 characters.")]
        [Display(Name = "Symptoms / Notes")]
        public string SymptomsNotes { get; set; } = string.Empty;
    }
}
using System;
using System.Collections.Generic;

namespace DAL.EF.Table;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public byte[] FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Speciality { get; set; } = null!;

    public decimal ConsultationFee { get; set; }

    public string Email { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

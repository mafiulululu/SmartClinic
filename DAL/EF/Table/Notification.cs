using System;
using System.Collections.Generic;

namespace DAL.EF.Table;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public string Message { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string SendStatus { get; set; } = null!;

    public DateTime ScheduledTime { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace DAL.EF.Table;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int AppointmentId { get; set; }

    public decimal BaseAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal Discount { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}

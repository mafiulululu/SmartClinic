using System;
using System.Collections.Generic;
using DAL.EF.Table;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public partial class SmartClinicContext : DbContext
{
    public SmartClinicContext()
    {
    }

    public SmartClinicContext(DbContextOptions<SmartClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DbConn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA23652E34C");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SymptomsNotes).HasColumnName("Symptoms_Notes");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Patient");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctor__2DC00EDFE47C8F6F");

            entity.ToTable("Doctor");

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.ConsultationFee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FirstNAME");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LastNAME");
            entity.Property(e => e.Speciality)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__D796AAD545F26F53");

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.BaseAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Appointment");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E325097FE6A");

            entity.ToTable("Notification");

           entity.Property(e => e.NotificationId)
             .ValueGeneratedOnAdd()
             .HasColumnName("NotificationID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Message).HasMaxLength(200);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.ScheduledTime).HasColumnType("datetime");
            entity.Property(e => e.SendStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Patient");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patient");

            entity.Property(e => e.PatientId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PatientID");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Dob)
                .HasColumnName("DOB");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FirstNAME");

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LastNAME");

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB8424627");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C26179A8").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())", "DF__Users__CreatedAt__628FA481")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

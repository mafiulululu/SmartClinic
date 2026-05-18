using DAL.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BLL.Services
{
    public class InvoicePdfService : IInvoicePdfService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoicePdfService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<byte[]?> GenerateInvoicePdfAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
            {
                return null;
            }

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);

                    page.Header().Column(column =>
                    {
                        column.Item().Text("SmartClinic Management System")
                            .Bold()
                            .FontSize(22)
                            .FontColor(Colors.Blue.Medium);

                        column.Item().Text("Appointment Invoice")
                            .FontSize(13)
                            .FontColor(Colors.Grey.Darken2);

                        column.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Spacing(14);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text($"Invoice ID: #{invoice.InvoiceId}").Bold();
                                left.Item().Text($"Appointment ID: #{invoice.AppointmentId}");
                                left.Item().Text($"Generated At: {invoice.GeneratedAt:yyyy-MM-dd hh:mm tt}");
                                left.Item().Text($"Payment Status: {invoice.PaymentStatus}");
                            });

                            row.RelativeItem().AlignRight().Column(right =>
                            {
                                right.Item().Text("SmartClinic").Bold().FontSize(16);
                                right.Item().Text("Bangladesh");
                            });
                        });

                        column.Item().LineHorizontal(1);

                        column.Item().Text("Patient Information")
                            .Bold()
                            .FontSize(14)
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Patient Name").Bold();
                            table.Cell().Text($"{invoice.Appointment.Patient.FirstName} {invoice.Appointment.Patient.LastName}");

                            table.Cell().Text("Patient Email").Bold();
                            table.Cell().Text(invoice.Appointment.Patient.Email);

                            table.Cell().Text("Phone").Bold();
                            table.Cell().Text(invoice.Appointment.Patient.Phone);
                        });

                        column.Item().Text("Doctor & Appointment Information")
                            .Bold()
                            .FontSize(14)
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Text("Doctor").Bold();
                            table.Cell().Text($"Dr. {invoice.Appointment.Doctor.FirstName} {invoice.Appointment.Doctor.LastName}");

                            table.Cell().Text("Speciality").Bold();
                            table.Cell().Text(invoice.Appointment.Doctor.Speciality);

                            table.Cell().Text("Appointment Date").Bold();
                            table.Cell().Text($"{invoice.Appointment.AppointmentDate:yyyy-MM-dd hh:mm tt}");

                            table.Cell().Text("Appointment Status").Bold();
                            table.Cell().Text(invoice.Appointment.Status);
                        });

                        column.Item().Text("Billing Summary")
                            .Bold()
                            .FontSize(14)
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(130);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(6).Text("Description").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(6).AlignRight().Text("Amount").Bold();
                            });

                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Doctor Consultation Fee");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).AlignRight().Text($"{invoice.BaseAmount:0.00} BDT");

                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Tax Amount 5%");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).AlignRight().Text($"{invoice.TaxAmount:0.00} BDT");

                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Discount");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).AlignRight().Text($"{invoice.Discount:0.00} BDT");

                            table.Cell().Background(Colors.Blue.Lighten4).Padding(6).Text("Total Amount").Bold();
                            table.Cell().Background(Colors.Blue.Lighten4).Padding(6).AlignRight().Text($"{invoice.TotalAmount:0.00} BDT").Bold();
                        });

                        column.Item().PaddingTop(10).Text(text =>
                        {
                            text.Span("Payment Status: ").Bold();

                            if (invoice.PaymentStatus == "Paid")
                            {
                                text.Span("Payment Done").FontColor(Colors.Green.Darken2).Bold();
                            }
                            else
                            {
                                text.Span("Unpaid").FontColor(Colors.Red.Darken2).Bold();
                            }
                        });

                        column.Item().PaddingTop(20).Text("Note: This PDF invoice was generated automatically by SmartClinic Management System.")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Generated by SmartClinic | Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
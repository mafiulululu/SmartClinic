using System.ComponentModel.DataAnnotations;

namespace SmartClinic.Web.ViewModels
{
    public class PaymentViewModel
    {
        public int InvoiceId { get; set; }

        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Display(Name = "bKash / Nagad Mobile Number")]
        [RegularExpression(@"^01[3-9]\d{8}$", ErrorMessage = "Enter a valid Bangladeshi mobile number. Example: 01712345678")]
        public string? MobileWalletNumber { get; set; }

        [Display(Name = "Transaction Reference")]
        public string? TransactionReference { get; set; }
    }
}
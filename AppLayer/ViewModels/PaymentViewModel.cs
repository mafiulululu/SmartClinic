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

        [Required(ErrorMessage = "Please enter transaction/card reference.")]
        [Display(Name = "Transaction Reference")]
        public string TransactionReference { get; set; } = string.Empty;
    }
}
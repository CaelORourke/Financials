namespace Financials.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string Recipient { get; set; }

        [Required]
        public decimal TransactionValue { get; set; }

        public bool SoftDelete { get; set; }
    }
}

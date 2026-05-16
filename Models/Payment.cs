using System;

namespace FoodOrderingSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

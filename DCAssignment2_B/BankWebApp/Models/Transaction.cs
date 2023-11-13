namespace BankWebApp.Models
{
    public class Transaction
    {
        public uint TransactionId { get; set; }
        public uint FromAccount { get; set; }
        public uint ToAccount { get; set; }
        public double Amount { get; set; }
    }
}

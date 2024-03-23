namespace ShaTask.Core.Models
{
    public class Cashier
    {
        public int CashierId { get; set; }
        public string Name { get; set; }
        public ICollection<Invoice> Invoices { get; set; } // One-to-Many relationship with Invoice
       
    }
}
